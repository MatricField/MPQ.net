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

namespace MPQNet.Header
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class ArchiveHeader3 : ArchiveHeader2, IEquatable<ArchiveHeader3>
    {
        /// <summary>
        /// 64-bit version of the archive size
        /// </summary>
        public ulong ArchiveSize64 { get; }

        /// <summary>
        /// 64-bit position of the BET table
        /// </summary>
        public ulong BetTableOffset { get; }

        /// <summary>
        /// 64-bit position of the HET table
        /// </summary>
        public ulong HetTableOffset { get; }

        #region Structural Equality
        public override bool Equals(object obj)
        {
            return Equals(obj as ArchiveHeader3);
        }

        public bool Equals(ArchiveHeader3 other)
        {
            return other != null &&
                   base.Equals(other) &&
                   ArchiveSize64 == other.ArchiveSize64 &&
                   BetTableOffset == other.BetTableOffset &&
                   HetTableOffset == other.HetTableOffset;
        }

        public override int GetHashCode()
        {
            var hashCode = 2113465942;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + ArchiveSize64.GetHashCode();
            hashCode = hashCode * -1521134295 + BetTableOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + HetTableOffset.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ArchiveHeader3 header1, ArchiveHeader3 header2)
        {
            return EqualityComparer<ArchiveHeader3>.Default.Equals(header1, header2);
        }

        public static bool operator !=(ArchiveHeader3 header1, ArchiveHeader3 header2)
        {
            return !(header1 == header2);
        } 
        #endregion
    }
}
