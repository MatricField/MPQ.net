using MPQNet.Definition;
using System;
using System.Collections.Generic;
using System.Text;

namespace MPQNet.IO
{
    public interface IMPQFileInfo
    {
        /// <summary>
        ///  Offset of the beginning of the file, relative to the beginning of the archive.
        /// </summary>
        uint FilePos { get; set; }
        /// <summary>
        /// Compressed file size
        /// </summary>
        uint CompressedSize { get; set; }
        /// <summary>
        /// Only valid if the block is a file; otherwise meaningless, and should be 0.
        /// If the file is compressed, this is the size of the uncompressed file data.
        /// </summary>
        uint FileSize { get; set; }
        /// <summary>
        /// Flags for the file.
        /// </summary>
        MPQFileFlags Flags { get; set; }
    }
}
