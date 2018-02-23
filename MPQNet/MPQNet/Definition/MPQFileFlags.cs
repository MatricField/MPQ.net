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

namespace MPQNet.Definition
{
    [Flags]
    public enum MPQFileFlags : uint
    {
        /// <summary>
        /// Implode method (By PKWARE Data Compression Library)
        /// </summary>
        IMPLODE = 0x00000100,

        /// <summary>
        /// Compress methods (By multiple methods)
        /// </summary>
        COMPRESS = 0x00000200,

        /// <summary>
        /// Indicates whether file is encrypted 
        /// </summary>
        ENCRYPTED = 0x00010000,

        /// <summary>
        /// File decryption key has to be fixed
        /// </summary>
        FIX_KEY = 0x00020000,

        /// <summary>
        /// The file is a patch file. Raw file data begin with TPatchInfo structure
        /// </summary>
        PATCH_FILE = 0x00100000,

        /// <summary>
        /// File is stored as a single unit, rather than split into sectors (Thx, Quantam)
        /// </summary>
        SINGLE_UNIT = 0x01000000,

        /// <summary>
        /// File is a deletion marker. Used in MPQ patches, indicating that the file no longer exists.
        /// </summary>
        DELETE_MARKER = 0x02000000,

        /// <summary>
        /// File has checksums for each sector. Ignored if file is not compressed or imploded.
        /// </summary>
        SECTOR_CRC = 0x04000000,

        /// <summary>
        /// Present on STANDARD.SNP\(signature). The only occurence ever observed
        /// </summary>
        SIGNATURE = 0x10000000,

        /// <summary>
        /// Set if file exists, reset when the file was deleted
        /// </summary>
        EXISTS = 0x80000000,

        /// <summary>
        /// Replace when the file exist (SFileAddFile)
        /// </summary>
        REPLACEEXISTING = 0x80000000,

        /// <summary>
        /// Mask for a file being compressed
        /// </summary>
        COMPRESS_MASK = 0x0000FF00,

        /// <summary>
        /// Use default flags for internal files
        /// </summary>
        DEFAULT_INTERNAL = 0xFFFFFFFF,
    }
}
