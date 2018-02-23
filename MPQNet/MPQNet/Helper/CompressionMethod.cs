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

namespace MPQNet.Helper
{
    /// <summary>
    /// Compression types for multiple compressions
    /// </summary>
    public enum CompressionMethod : uint
    {
        /// <summary>
        /// Huffmann compression (used on WAVE files only)
        /// </summary>
        HUFFMANN = 0x01,

        /// <summary>
        /// ZLIB compression
        /// </summary>
        ZLIB = 0x02,

        /// <summary>
        /// PKWARE DCL compression
        /// </summary>
        PKWARE = 0x08,

        /// <summary>
        /// BZIP2 compression (added in Warcraft III)
        /// </summary>
        BZIP2 = 0x10,

        /// <summary>
        /// Sparse compression (added in Starcraft 2)
        /// </summary>
        SPARSE = 0x20,

        /// <summary>
        /// IMA ADPCM compression (mono)
        /// </summary>
        ADPCM_MONO = 0x40,

        /// <summary>
        /// IMA ADPCM compression (stereo)
        /// </summary>
        ADPCM_STEREO = 0x80,

        /// <summary>
        /// LZMA compression. Added in Starcraft 2. This value is NOT a combination of flags.
        /// </summary>
        LZMA = 0x12,

        /// <summary>
        /// Same compression
        /// </summary>
        NEXT_SAME = 0xFFFFFFFF,
    }
}
