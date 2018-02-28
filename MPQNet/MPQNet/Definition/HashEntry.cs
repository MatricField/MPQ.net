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
    /// Hash table entry. All files in the archive are searched by their hashes.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HashEntry : IEquatable<HashEntry>
    {
        /// <summary>
        /// The hash of the file path, using method A.
        /// </summary>
        public uint Name1 { get; }

        /// <summary>
        /// The hash of the file path, using method B.
        /// </summary>
        public uint Name2 { get; }

        /// <summary>
        /// The language of the file. This is a Windows LANGID data type, and uses the same values.
        /// 0 indicates the default language (American English), or that the file is language-neutral.
        /// </summary>
        public ushort Locale { get; }

        /// <summary>
        /// The platform the file is used for. 0 indicates the default platform.
        /// No other values have been observed.
        /// </summary>
        public byte Platform { get; }

        public byte Reserved { get; }

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

        /// <summary>
        /// If the hash table entry is valid, this is the index into the block table of the file.
        /// Otherwise, one of the values listed in remarks.
        /// </summary>
        /// <remarks>
        /// Possible special values:
        /// <see cref="HASH_ENTRY_IS_EMPTY"/>
        /// <see cref="HASH_ENTRY_NO_LONGER_VALID"/>
        /// </remarks>
        public int BlockIndex { get; }

        public HashEntry(uint name1, uint name2, ushort locale, byte platform, byte reserved, int blockIndex)
        {
            Name1 = name1;
            Name2 = name2;
            Locale = locale;
            Platform = platform;
            Reserved = reserved;
            BlockIndex = blockIndex;
        }

        public override string ToString()
        {
            return
                $"Name1: {Name1:X}\nName2: {Name2:X}\nLocale: {Locale:X}\nBlockIndex: {BlockIndex}";
        }

        #region Structural Equality
        public override bool Equals(object obj)
        {
            return (obj is HashEntry) && (Equals((HashEntry)(obj)));
        }

        public bool Equals(HashEntry other)
        {
            return other != null &&
                   Name1 == other.Name1 &&
                   Name2 == other.Name2 &&
                   Locale == other.Locale &&
                   Platform == other.Platform &&
                   Reserved == other.Reserved &&
                   BlockIndex == other.BlockIndex;
        }

        public override int GetHashCode()
        {
            var hashCode = 1733344741;
            hashCode = hashCode * -1521134295 + Name1.GetHashCode();
            hashCode = hashCode * -1521134295 + Name2.GetHashCode();
            hashCode = hashCode * -1521134295 + Locale.GetHashCode();
            hashCode = hashCode * -1521134295 + Platform.GetHashCode();
            hashCode = hashCode * -1521134295 + Reserved.GetHashCode();
            hashCode = hashCode * -1521134295 + BlockIndex.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(HashEntry entry1, HashEntry entry2)
        {
            return EqualityComparer<HashEntry>.Default.Equals(entry1, entry2);
        }

        public static bool operator !=(HashEntry entry1, HashEntry entry2)
        {
            return !(entry1 == entry2);
        } 
        #endregion
    }
}
