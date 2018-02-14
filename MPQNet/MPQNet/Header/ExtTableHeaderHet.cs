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
    /// <summary>
    /// Structure for HET table header
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class ExtTableHeaderHet : ExtTableHeaderCommon, IEquatable<ExtTableHeaderHet>
    {
        /// <summary>
        /// Size of the entire HET table, including HET_TABLE_HEADER (in bytes)
        /// </summary>
        public uint TableSize { get; }

        /// <summary>
        /// Number of occupied entries in the HET table
        /// </summary>
        public uint EntryCount { get; }

        /// <summary>
        /// Total number of entries in the HET table
        /// </summary>
        public uint TotalCount { get; }

        /// <summary>
        /// Size of the name hash entry (in bits)
        /// </summary>
        public uint NameHashBitSize { get; }

        /// <summary>
        /// Total size of file index (in bits)
        /// </summary>
        public uint IndexSizeTotal { get; }

        /// <summary>
        /// Extra bits in the file index
        /// </summary>
        public uint IndexSizeExtra { get; }

        /// <summary>
        /// Effective size of the file index (in bits)
        /// </summary>
        public uint IndexSize { get; }

        /// <summary>
        /// Size of the block index subtable (in bytes)
        /// </summary>
        public uint IndexTableSize { get; }

        #region Structual Equality
        public override bool Equals(object obj)
        {
            return Equals(obj as ExtTableHeaderHet);
        }

        public bool Equals(ExtTableHeaderHet other)
        {
            return other != null &&
                   base.Equals(other) &&
                   TableSize == other.TableSize &&
                   EntryCount == other.EntryCount &&
                   TotalCount == other.TotalCount &&
                   NameHashBitSize == other.NameHashBitSize &&
                   IndexSizeTotal == other.IndexSizeTotal &&
                   IndexSizeExtra == other.IndexSizeExtra &&
                   IndexSize == other.IndexSize &&
                   IndexTableSize == other.IndexTableSize;
        }

        public override int GetHashCode()
        {
            var hashCode = 1657946346;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + TableSize.GetHashCode();
            hashCode = hashCode * -1521134295 + EntryCount.GetHashCode();
            hashCode = hashCode * -1521134295 + TotalCount.GetHashCode();
            hashCode = hashCode * -1521134295 + NameHashBitSize.GetHashCode();
            hashCode = hashCode * -1521134295 + IndexSizeTotal.GetHashCode();
            hashCode = hashCode * -1521134295 + IndexSizeExtra.GetHashCode();
            hashCode = hashCode * -1521134295 + IndexSize.GetHashCode();
            hashCode = hashCode * -1521134295 + IndexTableSize.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ExtTableHeaderHet het1, ExtTableHeaderHet het2)
        {
            return EqualityComparer<ExtTableHeaderHet>.Default.Equals(het1, het2);
        }

        public static bool operator !=(ExtTableHeaderHet het1, ExtTableHeaderHet het2)
        {
            return !(het1 == het2);
        } 
        #endregion
    }
}
