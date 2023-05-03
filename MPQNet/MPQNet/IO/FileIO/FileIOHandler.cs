//using MPQNet.Compression;
//using MPQNet.Cryptography;
//using MPQNet.Definition;
//using MPQNet.IO.Streams;
//using System;
//using System.IO;
//using Ionic.BZip2;
//using System.IO.Compression;
//using System.Security.Cryptography;

//namespace MPQNet.IO.FileIO
//{
//    public class FileIOHandler :
//        IFileIOHandler
//    {
//        protected long SectorSize { get; }
//        public ILowLevelIOHandler LowLevelIO { get; }

//        public FileIOHandler(ILowLevelIOHandler lowLevelIO, long sectorSize)
//        {
//            LowLevelIO = lowLevelIO;
//            SectorSize = sectorSize;
//        }

//        public Stream OpenFile(MPQFileInfo info)
//        {
//            if (info.Block.Flags.HasFlag(MPQFileFlags.SINGLE_UNIT))
//            {
//                return OpenSingleUnitStream(info);
//            }
//            else
//            {
//                return OpenCompositeStream(info);
//            }
//        }

//        protected virtual Stream OpenSingleUnitStream(MPQFileInfo info)
//        {
//            var key = GetFileKey(info);
//            var blockInfo =
//                new BlockInfo()
//                {
//                    Entry = info.Block,
//                    DecryptionKey = key
//                };
//            return OpenMPQBlock(blockInfo);
//        }

//        protected virtual Stream OpenCompositeStream(MPQFileInfo info)
//        {
//            //TODO: AddImpl
//            throw new NotImplementedException();
            
//            long fileSize = info.Block.FileSize;
//            var sectorCount = fileSize / SectorSize;
//            if(fileSize % SectorSize != 0)
//            {
//                ++sectorCount;
//            }

//            if (sectorCount <= 0)
//            {
//                throw new InvalidDataException();
//            }

            
//        }

//        protected virtual Stream OpenMPQBlock(BlockInfo info)
//        {

//            var stream = LowLevelIO.GetStream(info.Entry.FilePos + 1, info.Entry.CompressedSize);
//            if(info.IsEncrypted)
//            {
//                stream = Decrypt(stream, info.DecryptionKey);
//            }
//            using (var infoStream = LowLevelIO.GetStream(info.Entry.FilePos, 16))
//            {
//                var compressionFlags = (CompressionFlags)infoStream.ReadByte();
//                if (info.IsCompressed)
//                {
//                    stream = Decompress(stream, compressionFlags);
//                }
//            }
//            return stream;
//        }

//        //protected virtual Stream Decrypt(Stream underlyingStream, uint key) =>
//        //    new DecryptStream(underlyingStream, key);
//        protected virtual Stream Decrypt(Stream underlyingStream, uint key) =>
//            new CryptoStream(underlyingStream, new MPQCryptoTransform(key), CryptoStreamMode.Read);

//        protected virtual Stream Decompress(Stream underlyingStream, CompressionFlags flags)
//        {
//            if (flags.HasFlag(CompressionFlags.LZMA))
//            {
//                throw new NotImplementedException();
//            }
//            if (flags.HasFlag(CompressionFlags.BZIP2))
//            {
//                underlyingStream = new BZip2InputStream(underlyingStream);
//            }
//            if (flags.HasFlag(CompressionFlags.IMPLODED))
//            {
//                throw new NotImplementedException();
//            }
//            if (flags.HasFlag(CompressionFlags.DEFLATED))
//            {
//                underlyingStream = new DeflateStream(underlyingStream, CompressionMode.Decompress);
//            }
//            if (flags.HasFlag(CompressionFlags.SPARSE))
//            {
//                throw new NotImplementedException();
//            }
//            if (flags.HasFlag(CompressionFlags.HUFFMANN))
//            {
//                throw new NotImplementedException();
//            }
//            if (flags.HasFlag(CompressionFlags.ADPCM_STEREO))
//            {
//                throw new NotImplementedException();
//            }
//            if (flags.HasFlag(CompressionFlags.ADPCM_MONO))
//            {
//                throw new NotImplementedException();
//            }
//            return underlyingStream;
//        }

//        protected virtual uint GetFileKey(MPQFileInfo info)
//        {
//            var key = MPQHash.HashName(info.Name, HashType.FileKey);
//            if(info.Block.Flags.HasFlag(MPQFileFlags.FIX_KEY))
//            {
//                key = (key + info.Block.FilePos) ^ info.Block.FileSize;
//            }
//            return key;
//        }
//    }
//}
