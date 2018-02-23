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
    /// <summary>
    /// File description block contains informations about the file
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BlockEntry : IEquatable<BlockEntry>
    {
        /// <summary>
        ///  Offset of the beginning of the file, relative to the beginning of the archive.
        /// </summary>
        public uint FilePos { get; }

        /// <summary>
        /// Compressed file size
        /// </summary>
        public uint CompressedSize { get; }

        /// <summary>
        /// Only valid if the block is a file; otherwise meaningless, and should be 0.
        /// If the file is compressed, this is the size of the uncompressed file data.
        /// </summary>
        public uint FileSize { get; }

        /// <summary>
        /// Flags for the file.
        /// </summary>
        public MPQFileFlags Flags { get; }

        public BlockEntry(uint filePos, uint compressedSize, uint fileSize, MPQFileFlags flags)
        {
            FilePos = filePos;
            CompressedSize = compressedSize;
            FileSize = fileSize;
            Flags = flags;
        }

        #region Structural Equality
        public override bool Equals(object obj)
        {
            return (obj is BlockEntry) && (Equals((BlockEntry)(obj)));
        }

        public bool Equals(BlockEntry other)
        {
            return other != null &&
                   FilePos == other.FilePos &&
                   CompressedSize == other.CompressedSize &&
                   FileSize == other.FileSize &&
                   Flags == other.Flags;
        }

        public override int GetHashCode()
        {
            var hashCode = -1133767338;
            hashCode = hashCode * -1521134295 + FilePos.GetHashCode();
            hashCode = hashCode * -1521134295 + CompressedSize.GetHashCode();
            hashCode = hashCode * -1521134295 + FileSize.GetHashCode();
            hashCode = hashCode * -1521134295 + Flags.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(BlockEntry entry1, BlockEntry entry2)
        {
            return EqualityComparer<BlockEntry>.Default.Equals(entry1, entry2);
        }

        public static bool operator !=(BlockEntry entry1, BlockEntry entry2)
        {
            return !(entry1 == entry2);
        }
        #endregion
    }
}
