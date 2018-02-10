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

using System.Runtime.InteropServices;

namespace MPQNet.Archive
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class HeaderV4 : HeaderV3
    {
        /// <summary>
        /// Compressed size of the hash table
        /// </summary>
        public ulong HashTableSize64 { get; }

        /// <summary>
        /// Compressed size of the block table
        /// </summary>
        public ulong BlockTableSize64 { get; }

        /// <summary>
        /// Compressed size of the hi-block table
        /// </summary>
        public ulong HiBlockTableSize64 { get; }

        /// <summary>
        /// Compressed size of the HET block
        /// </summary>
        public ulong HetTableSize64 { get; }

        /// <summary>
        /// Compressed size of the BET block
        /// </summary>
        public ulong BetTableSize64 { get; }
    }
}
