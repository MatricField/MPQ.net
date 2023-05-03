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


using System.Diagnostics.CodeAnalysis;

namespace MPQNet.Definition
{
    internal record class Header3: Header2
    {

        protected ulong _ArchiveSize64;
        protected ulong _BetTableOffset;
        protected ulong _HetTableOffset;

        /// <summary>
        /// 64-bit version of the archive size
        /// </summary>
        public required virtual ulong ArchiveSize64
        {
            get => _ArchiveSize64;
            init => _ArchiveSize64 = value;
        }

        /// <summary>
        /// 64-bit position of the BET table
        /// </summary>
        public required virtual ulong BetTableOffset
        {
            get => _BetTableOffset;
            init => _BetTableOffset = value;
        }

        /// <summary>
        /// 64-bit position of the HET table
        /// </summary>
        public required virtual ulong HetTableOffset
        {
            get => _HetTableOffset;
            init => _HetTableOffset = value;
        }

        public Header3()
            :base()
        {

        }

        [SetsRequiredMembers]
        public Header3(in RawHeader raw, long baseAddress)
            : base(raw, baseAddress)
        {
            _ArchiveSize64 = raw.ArchiveSize64;
            _BetTableOffset = raw.BetTablePos64;
            _HetTableOffset = raw.HetTablePos64;
        }
    }
}
