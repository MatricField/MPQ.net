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

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MPQNet.Definition
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class ArchiveHeader2 : ArchiveHeader, IEquatable<ArchiveHeader2>
    {
        /// <summary>
        /// Offset to the beginning of array of 16-bit high parts of file offsets.
        /// </summary>
        public ulong ExtendedBlockTableOffset { get; }

        /// <summary>
        /// High 16 bits of the hash table offset for large archives.
        /// </summary>
        public ushort HashTableOffsetHigh { get; }

        /// <summary>
        /// High 16 bits of the block table offset for large archives.
        /// </summary>
        public ushort BlockTableOffsetHigh { get; }

        #region Structural Equality
        public override bool Equals(object obj)
        {
            return Equals(obj as ArchiveHeader2);
        }

        public bool Equals(ArchiveHeader2 other)
        {
            return other != null &&
                   base.Equals(other) &&
                   ExtendedBlockTableOffset == other.ExtendedBlockTableOffset &&
                   HashTableOffsetHigh == other.HashTableOffsetHigh &&
                   BlockTableOffsetHigh == other.BlockTableOffsetHigh;
        }

        public override int GetHashCode()
        {
            var hashCode = 445357405;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + ExtendedBlockTableOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + HashTableOffsetHigh.GetHashCode();
            hashCode = hashCode * -1521134295 + BlockTableOffsetHigh.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ArchiveHeader2 header1, ArchiveHeader2 header2)
        {
            return EqualityComparer<ArchiveHeader2>.Default.Equals(header1, header2);
        }

        public static bool operator !=(ArchiveHeader2 header1, ArchiveHeader2 header2)
        {
            return !(header1 == header2);
        }
        #endregion
    }
}
