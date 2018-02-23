using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MPQNet.Helper
{
    public sealed class MPQCryptor
    {
        private const int BUFFER_SIZE = 0x500;

        private uint[] Buffer;

        public MPQCryptor()
        {
            Buffer = new uint[BUFFER_SIZE];
            uint seed = 0x00100001;

            for (uint index1 = 0; index1 < 0x100; index1++)
            {
                for (uint index2 = index1, i = 0; i < 5; i++, index2 += 0x100)
                {
                    uint temp1, temp2;

                    seed = (seed * 125 + 3) % 0x2AAAAB;
                    temp1 = (seed & 0xFFFF) << 0x10;

                    seed = (seed * 125 + 3) % 0x2AAAAB;
                    temp2 = (seed & 0xFFFF);

                    Buffer[index2] = (temp1 | temp2);
                }
            }
        }
    }
}
