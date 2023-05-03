using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MPQNet.Cryptography
{
    /// <summary>
    /// Default string hash algorithm used to hash file name
    /// in hash table
    /// </summary>
    public class HashString :
        HashAlgorithm
    {
        private const uint DEFAULT_SEED1 = 0x7FED7FED;

        private const uint DEFAULT_SEED2 = 0xEEEEEEEE;

        private uint dwSeed1;

        private uint dwSeed2;

        private readonly uint HashType;

        protected HashString(uint hashType)
        {
            HashSizeValue = sizeof(uint);
            HashType = hashType;
            Initialize();
        }

        public override void Initialize()
        {
            dwSeed1 = DEFAULT_SEED1;
            dwSeed2 = DEFAULT_SEED2;
        }

        protected override void Dispose(bool disposing)
        {
            dwSeed1 = default;
            dwSeed2 = default;
            base.Dispose(disposing);
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            HashCore(array.AsSpan(ibStart, cbSize));
        }

        protected override void HashCore(ReadOnlySpan<byte> source)
        {
            foreach (uint ch in source)
            {
                dwSeed1 = Common.CryptTable[(int)(HashType + ch)] ^ (dwSeed1 + dwSeed2);
                dwSeed2 = ch + dwSeed1 + dwSeed2 + (dwSeed2 << 5) + 3;
            }
        }

        protected override byte[] HashFinal()
        {
            return BitConverter.GetBytes(dwSeed1);
        }

        /// <summary>
        /// Default hash function for file names.
        /// Convert the input character to uppercase,
        /// Convert slash (0x2F) to backslash (0x5C)
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static uint HashDefault(string fileName, uint hashType)
        {
            var hasher = new HashString(hashType);
            var bytes = Encoding.ASCII.GetBytes(fileName.ToUpper().Replace('/', '\\'));
            hasher.HashCore(bytes);
            return hasher.dwSeed1;
        }

        /// <summary>
        /// Hash function for file names.
        /// Convert the input character to uppercase,
        /// DON'T convert slash (0x2F) to backslash (0x5C)
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static uint HashSlash(string fileName, uint hashType)
        {
            var hasher = new HashString(hashType);
            var bytes = Encoding.ASCII.GetBytes(fileName.ToUpper());
            hasher.HashCore(bytes);
            return hasher.dwSeed1;
        }

        /// <summary>
        /// Hash function for file names.
        /// Convert the input character to lowercase,
        /// DON'T convert slash (0x2F) to backslash (0x5C)
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static uint HashLower(string fileName, uint hashType)
        {
            var hasher = new HashString(hashType);
            var bytes = Encoding.ASCII.GetBytes(fileName.ToLower());
            hasher.HashCore(bytes);
            return hasher.dwSeed1;
        }
    }
}
