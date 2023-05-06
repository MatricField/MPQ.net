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
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal record struct HashEntry
    {
        public const int BLOCK_INDEX_EMPTY_END = unchecked((int)0xFFFFFFFF);

        public const int BLOCK_INDEX_EMPTY_CONTINUE = unchecked((int)0xFFFFFFFE);

        /// <summary>
        /// Hash table entry is empty, and has always been empty.
        /// Terminates searches for a given file.
        /// </summary>
        public const int HASH_ENTRY_IS_EMPTY = unchecked((int)0xFFFFFFFF);

        /// <summary>
        /// Hash table entry is empty, but was valid at some point (a deleted file).
        /// Does not terminate searches for a given file.
        /// </summary>
        public const int HASH_ENTRY_NO_LONGER_VALID = unchecked((int)0xFFFFFFFE);

        private uint _Name1;
        private uint _Name2;
        private ushort _Locale;
        private byte _Platform;
        private byte _Reserved;
        private int _BlockIndex;

        /// <summary>
        /// The hash of the file path, using method A.
        /// </summary>
        public required uint Name1 { get => _Name1; init => _Name1 = value; }

        /// <summary>
        /// The hash of the file path, using method B.
        /// </summary>
        public required uint Name2 { get => _Name2; init => _Name2 = value; }

        /// <summary>
        /// The language of the file. This is a Windows LANGID data type, and uses the same values.
        /// 0 indicates the default language (American English), or that the file is language-neutral.
        /// </summary>
        public required ushort Locale { get => _Locale; init => _Locale = value; }

        /// <summary>
        /// The platform the file is used for. 0 indicates the default platform.
        /// No other values have been observed.
        /// </summary>
        public required byte Platform { get => _Platform; init => _Platform = value; }

        /// <summary>
        /// If the hash table entry is valid, this is the index into the block table of the file.
        /// Otherwise, one of the values listed in remarks.
        /// </summary>
        /// <remarks>
        /// Possible special values:
        /// <see cref="HASH_ENTRY_IS_EMPTY"/>
        /// <see cref="HASH_ENTRY_NO_LONGER_VALID"/>
        /// </remarks>
        public required int BlockIndex { get => _BlockIndex; init => _BlockIndex = value; }
    }
}
