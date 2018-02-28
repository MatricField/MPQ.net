using System;
using System.Collections.Generic;
using System.Text;

namespace MPQNet.Cryptography
{
    public sealed class MPQHash :
        MPQCryptorBase
    {
        private MPQHash(uint hashKey):
            base(STRING_HASH_ENCRYPTION_KEY, hashKey)
        {

        }

        private void AddToHashValue(char ch)
        {
            IV1 = CryptTable[Convert.ToInt32(HashKey + ch)] ^ (IV1 + IV2);
            IV2 = ch + IV1 + IV2 + (IV2 << 5) + 3;
        }

        /// <summary>
        /// Hash a string. Convertint slash (0x2F) to backslash (0x5C).
        /// </summary>
        /// 
        /// <remarks>
        /// Implementation of this function in WorldEdit.exe and storm.dll
        /// incorrectly treats the character as signed, which leads to the 
        /// a buffer underflow if the character in the file name >= 0x80:
        /// The following steps happen when rawCh == 0xBF and dwHashType == 0x0000
        /// (calculating hash index)
        ///
        /// 1) ch is sign-extended to 0xffffffbf
        /// 2) The "ch" is added to dwHashType (0xffffffbf + 0x0000 => 0xffffffbf)
        /// 3) The result is used as index to the StormBuffer table,
        /// thus dereferences a random value BEFORE the begin of StormBuffer.
        ///
        /// As result, MPQs containing files with non-ANSI characters will not work between
        /// various game versions and localizations. Even WorldEdit, after importing a file
        /// with Korean characters in the name, cannot open the file back.
        /// </remarks>
        public static uint HashPath(string str, uint hashKey)
        {
            var cryptor = new MPQHash(hashKey);
            foreach (var chRaw in str)
            {
                var ch = '/' == chRaw ? '\\' : char.ToUpper(chRaw);
                cryptor.AddToHashValue(ch);
            }
            return cryptor.IV1;
        }

        /// <summary>
        /// Hash a string. NOT convertint slash (0x2F) to backslash (0x5C).
        /// </summary>
        public static uint HashName(string str, uint hashKey)
        {

            var cryptor = new MPQHash(hashKey);
            foreach (var ch in str)
            {
                cryptor.AddToHashValue(char.ToUpper(ch));
            }
            return cryptor.IV1;
        }

        /// <summary>
        /// <see cref="IV1"/> value used in <see cref="HashPath(string, uint)"/>
        /// and <see cref="HashName(string, uint)"/>
        /// </summary>
        private const uint STRING_HASH_ENCRYPTION_KEY = 0x7FED7FED;
    }
}
