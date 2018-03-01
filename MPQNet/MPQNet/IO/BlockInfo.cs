using MPQNet.Compression;
using MPQNet.Cryptography;
using MPQNet.Definition;
using System;
using System.Collections.Generic;
using System.IO;

namespace MPQNet.IO
{
    /// <summary>
    /// Represent a block of data in a MPQ Archive
    /// </summary>
    public class BlockInfo
    {
        public string FullPath { get; }

        public string FileName =>
            Path.GetFileName(FullPath);

        public uint Size { get; }

        public uint OriginalSize { get; }

        public uint Offset { get; }

        public bool Encrypted { get; }

        protected bool KeyFixed { get; }

        public uint DecryptionKey
        {
            get
            {
                if(Encrypted)
                {
                    var key = MPQHash.HashName(FileName, HashType.FileKey);
                    if(KeyFixed)
                    {
                        key = (key + Offset) ^ OriginalSize;
                    }
                    return key;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public bool Compressed { get; }

        public BlockInfo(
            string fullPath,
            uint size,
            uint originalSize,
            uint offset,
            bool compressed = false,
            bool encrypted = false,
            bool keyFixed = false)
        {
            FullPath = fullPath;
            Size = size;
            OriginalSize = originalSize;
            Offset = offset;
            Compressed = compressed;
            Encrypted = encrypted;
            KeyFixed = keyFixed;
        }


    }
}
