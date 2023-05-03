//MIT License

//Copyright(c) 2023 Mingxi "Lucien" Du

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

using System;
using System.Collections.Generic;
using System.Text;

namespace MPQNet.Cryptography
{
    internal static class Common
    {
        private const int BUFFER_SIZE = 0x500;

        private static readonly uint[] cryptTable;

        static Common()
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
            Common.cryptTable = cryptTable;
        }

        public static ReadOnlySpan<uint> CryptTable => cryptTable;

        public static ReadOnlyMemory<uint> CryptTableMemory => cryptTable;
    }
}
