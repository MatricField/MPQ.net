using System;
using System.Collections.Generic;
using System.Text;
using MPQNet.Cryptography;
using MPQNet.Definition;

namespace MPQNet.IO
{
    /// <summary>
    /// Represent a single-unit file in a MPQ Archive
    /// </summary>
    public class SingleUnitFileInfo :
        BlockInfo
    {
        public MPQFileFlags Flags { get; }

        public SingleUnitFileInfo(string fullPath, uint size, uint originalSize, uint offset, MPQFileFlags flags)
            : base(
                  fullPath,
                  size,
                  originalSize,
                  offset,
                  flags.HasFlag(MPQFileFlags.COMPRESS) && (originalSize > size),
                  flags.HasFlag(MPQFileFlags.ENCRYPTED),
                  flags.HasFlag(MPQFileFlags.FIX_KEY))
        {

        }

        public SingleUnitFileInfo(string fullName, Archive archive, BlockEntry entry)
            : this(
                  fullPath: fullName,
                  size: entry.CompressedSize,
                  originalSize: entry.FileSize,
                  offset: entry.FilePos,
                  flags: entry.Flags)
        {

        }
    }
}
