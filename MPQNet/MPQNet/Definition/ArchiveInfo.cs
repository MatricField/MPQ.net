//MIT License

//Copyright(c) 2019 Mingxi "Lucien" Du

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

namespace MPQNet.Definition
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class ArchiveInfo : MPQSectionBase
    {
        private readonly uint _HeaderSize;
        private readonly uint _ArchiveSize;
        private readonly FormatVersions _FormatVersion;

        /// <summary>
        /// Size of the archive header.
        /// </summary>
        public virtual uint HeaderSize => _HeaderSize;
        /// <summary>
        /// Size of the whole archive, including the header.
        /// Does not include the strong digital signature, 
        /// if present. This size is used, among other things,
        /// for determining the region to hash in computing 
        /// the digital signature.
        /// This field is deprecated in the Burning Crusade MoPaQ format,
        /// and the size of the archive is calculated 
        /// as the size from the beginning of the archive to the end 
        /// of the hash table, block table, or extended block table(whichever is largest).
        /// </summary>
        public virtual uint ArchiveSize => _ArchiveSize;
        /// <summary>
        /// MoPaQ format version.
        /// </summary>
        public virtual FormatVersions FormatVersion => _FormatVersion;
    }
}
