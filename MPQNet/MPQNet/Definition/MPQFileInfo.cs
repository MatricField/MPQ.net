using MPQNet.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MPQNet.Definition
{
    /// <summary>
    /// Descript a file in a MPQ archive
    /// </summary>
    public sealed class MPQFileInfo
    {
        public string FullPath { get; }
        public string FileName =>
            Path.GetFileName(FullPath);
        public uint CompressedSize { get; }
        public uint OriginalSize { get; }
        public MPQFileFlags Flags { get;}
        public uint FileOffset { get; }

        public uint NameHashA { get; }

        public uint NameHashB { get; }

        public uint FileKey { get; }

        public MPQFileInfo(string fullPath, uint compressedSize, uint originalSize, MPQFileFlags flags, uint fileOffset)
        {
            FullPath = fullPath;
            CompressedSize = compressedSize;
            OriginalSize = originalSize;
            Flags = flags;
            FileOffset = fileOffset;
            NameHashA = MPQHash.HashPath(FullPath, HashType.NameA);
            NameHashB = MPQHash.HashPath(FullPath, HashType.NameB);
            FileKey = MPQCryptor.GetFileKey(this);
        }
    }
}
