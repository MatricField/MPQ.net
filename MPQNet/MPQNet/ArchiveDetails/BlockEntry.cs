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

using MPQNet.Definition;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace MPQNet.ArchiveDetails
{
    /// <summary>
    /// File description block contains informations about the file
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal record struct BlockEntry
    {
        private uint _FilePos;
        private uint _CompressedSize;
        private uint _FileSize;
        private MPQFileFlags _Flags;

        /// <summary>
        ///  Offset of the beginning of the file, relative to the beginning of the archive.
        /// </summary>
        public required uint FilePos
        {
            get => _FilePos;
            init => _FilePos = value;
        }

        /// <summary>
        /// Compressed file size
        /// </summary>
        public required uint CompressedSize
        {
            get => _CompressedSize;
            init => _CompressedSize = value;
        }

        /// <summary>
        /// Only valid if the block is a file; otherwise meaningless, and should be 0.
        /// If the file is compressed, this is the size of the uncompressed file data.
        /// </summary>
        public required uint FileSize
        {
            get => _FileSize;
            init => _FileSize = value;
        }

        /// <summary>
        /// Flags for the file.
        /// </summary>
        public required MPQFileFlags Flags
        {
            get => _Flags;
            init => _Flags = value;
        }
    }
}
