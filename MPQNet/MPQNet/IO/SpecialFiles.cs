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

using MPQNet.Cryptography;

namespace MPQNet.IO
{
    public static class SpecialFiles
    {
        /// <summary>
        /// Not a file, containing the hash table
        /// </summary>
        public const string HashTable = "(hash table)";

        public static readonly uint HashTableKey = HashString.HashDefault(HashTable, HashType.FileKey);

        /// <summary>
        /// Not a file, containing the block table
        /// </summary>
        public const string BlockTable = "(block table)";

        public static readonly uint BlockTableKey = HashString.HashDefault(BlockTable, HashType.FileKey);

        /// <summary>
        /// Simply a text file with file paths separated by ';', '\n', '\r', or some combination of these. The file "(listfile)" may not be listed in the listfile.
        /// </summary>
        public const string ListFile = "(listfile)";
    }
}
