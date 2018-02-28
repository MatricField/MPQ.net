using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MPQNet.Cryptography
{
    public abstract class MPQCryptorBase
    {
        private const int BUFFER_SIZE = 0x500;

        protected static readonly IReadOnlyList<uint> CryptTable;

        static MPQCryptorBase()
        {
            // Initialzie CryptTable
            var cryptTable = new uint[BUFFER_SIZE];
            uint seed = 0x00100001;
            for (var index1 = 0u; index1 < 0x100; index1++)
            {
                var index2 = index1;
                for (int i = 0; i < 5; i++)
                {
                    uint temp1, temp2;

                    seed = (seed * 125 + 3) % 0x2AAAAB;
                    temp1 = (seed & 0xFFFF) << 0x10;

                    seed = (seed * 125 + 3) % 0x2AAAAB;
                    temp2 = (seed & 0xFFFF);

                    cryptTable[index2] = (temp1 | temp2);
                    index2 += 0x100;
                }
            }
            CryptTable = cryptTable;
        }

        protected uint HashKey;

        /// <summary>
        /// Initialization Vector 1. Set to key
        /// </summary>
        protected uint IV1;

        /// <summary>
        /// Remember the original key
        /// </summary>
        protected readonly uint Key;

        /// <summary>
        /// Initialization Vector 2.
        /// Set to <see cref="DEFAULT_SEED"/>.
        /// </summary>
        protected uint IV2 = DEFAULT_SEED;

        public MPQCryptorBase(uint key, uint hashKey = HashType.Key2Mix)
        {
            IV1 = key;
            HashKey = hashKey * 0x100;
        }

        public void Reset()
        {
            IV1 = Key;
            IV2 = DEFAULT_SEED;
        }

        /// <summary>
        /// Default value of <see cref="IV2"/>
        /// </summary>
        private const uint DEFAULT_SEED = 0xEEEEEEEE;
    }
}
