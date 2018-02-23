using System;
using System.Collections.Generic;
using System.Text;

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
