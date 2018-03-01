using MPQNet.Cryptography;
using MPQNet.Definition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MPQNet.IO
{
    public class MPQFileInfo
    {
        public string FullPath { get; }

        public string FileName { get; }

        public uint Size { get; }

        public uint OriginalSize { get; }

        public uint Offset { get; }

        public MPQFileFlags Flags { get; }

        public virtual uint FileKey { get; }

        public MPQFileInfo(string path, uint size, uint originalSize, uint offset, MPQFileFlags flags)
        {
            FullPath = path;
            FileName = Path.GetFileName(FullPath);
            Size = size;
            OriginalSize = originalSize;
            Offset = offset;
            Flags = flags;
            var key = MPQHash.HashName(FileName, HashType.FileKey);
            if (Flags.HasFlag(MPQFileFlags.FIX_KEY))
            {
                key = (key + Offset) ^ OriginalSize;
            }
            FileKey = key;
        }

        public MPQFileInfo(string path, Archive archive, BlockEntry entry)
            :this(path, entry.CompressedSize, entry.FileSize, entry.FilePos, entry.Flags)
        {

        }

        public SectorInfo AsSingleUnit()
        {
            if(Flags.HasFlag(MPQFileFlags.SINGLE_UNIT))
            {
                var isCompressed = Flags.HasFlag(MPQFileFlags.COMPRESS) && OriginalSize > Size;
                if (Flags.HasFlag(MPQFileFlags.ENCRYPTED))
                {
                    return new SectorInfo(Offset, Size, isCompressed, FileKey);
                }
                else
                {
                    return new SectorInfo(Offset, Size, isCompressed);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
