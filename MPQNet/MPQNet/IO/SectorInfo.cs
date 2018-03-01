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
    public struct SectorInfo
    {
        /// <summary>
        /// The beginning of the sector, relative to the beginning of the actual archive
        /// </summary>
        public uint Position { get; }

        /// <summary>
        /// The size of the sector, in bytes
        /// </summary>
        public uint Size { get; }

        /// <summary>
        /// Indicate whether the sector is compressed
        /// </summary>
        public bool Compressed { get; }

        /// <summary>
        /// Indicate whether the sector is encrypted
        /// </summary>
        public bool Encrypted { get; }

        private readonly uint _DecryptionKey;

        public uint DecryptionKey =>
            Encrypted ? _DecryptionKey : throw new InvalidOperationException();

        private SectorInfo(uint position, uint size, bool compressed, bool encrypted, uint decryptionKey)
        {
            Size = size;
            Position = position;
            Compressed = compressed;
            Encrypted = encrypted;
            _DecryptionKey = decryptionKey;
        }

        public SectorInfo(uint position, uint size)
            :this(position, size, false, false, 0)
        {

        }

        public SectorInfo(uint position, uint size, bool compressed)
            : this(position, size, compressed, false, 0)
        {

        }

        public SectorInfo(uint position, uint size, uint decryptionKey)
            : this(position, size, false, true, decryptionKey)
        {

        }

        public SectorInfo(uint position, uint size, bool compressed, uint decryptionKey)
            : this(position, size, compressed, true, decryptionKey)
        {

        }
    }
}
