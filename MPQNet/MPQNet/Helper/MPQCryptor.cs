//MIT License

//Copyright(c) 2018 Mingxi "Lucien" Du

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.IO;

namespace MPQNet.Helper
{
    public static class MPQCryptor
    {
        private const int BUFFER_SIZE = 0x500;

        private static readonly uint[] CryptTable;

        static MPQCryptor()
        {
            CryptTable = new uint[BUFFER_SIZE];
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

                    CryptTable[index2] = (temp1 | temp2);
                    index2 += 0x100;
                }
            }

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
        public static uint HashString(string str, uint hashKey)
        {
            uint seed1 = 0x7FED7FED;
            uint seed2 = 0xEEEEEEEE;
            foreach(var chRaw in str)
            {
                var ch = '/' == chRaw ? '\\' : char.ToUpper(chRaw);
                seed1 = CryptTable[hashKey * 0x100 + ch] ^ (seed1 + seed2);
                seed2 = ch + seed1 + seed2 + (seed2 << 5) + 3;
            }
            return seed1;
        }

        /// <summary>
        /// Hash a string. NOT convertint slash (0x2F) to backslash (0x5C).
        /// </summary>
        public static uint HashStringSlash(string str, uint hashKey)
        {
            uint seed1 = 0x7FED7FED;
            uint seed2 = 0xEEEEEEEE;
            foreach (var chRaw in str)
            {
                var ch = char.ToUpper(chRaw);
                seed1 = CryptTable[hashKey * 0x100 + ch] ^ (seed1 + seed2);
                seed2 = ch + seed1 + seed2 + (seed2 << 5) + 3;
            }
            return seed1;
        }

        public static void DecryptDataInplace(byte[] data, uint key)
        {
            var reader = new BinaryReader(new MemoryStream(data));
            var writer = new BinaryWriter(new MemoryStream(data));
            uint seed = 0xEEEEEEEE;
            try
            {
                for (; ; )
                {
                    seed += CryptTable[HashType.Key2Mix * 0x100 + (key & 0xFF)];
                    var decrypted = reader.ReadUInt32() ^ (key + seed);  
                    key = ((~key << 0x15) + 0x11111111) | (key >> 0x0B);
                    seed = decrypted + seed + (seed << 5) + 3;
                    writer.Write(decrypted);
                }
            }
            catch (EndOfStreamException)
            {

            }
        }
    }
}
