﻿using System;
using System.IO;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Threading;
using System.Threading.Tasks;
using MPQNet.Compression;

namespace MPQNet.IO
{
    public class DataStream :
        Stream
    {
        protected int BlockSize { get; private set; }

        protected Archive MyArchive { get; private set; }

        protected Stream BaseStream { get; set; }

        public override bool CanRead => BaseStream.CanRead;

        public override bool CanSeek => BaseStream.CanSeek;

        public override bool CanWrite => BaseStream.CanWrite;

        public override long Length => BaseStream.Length;

        public override long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public override bool CanTimeout => base.CanTimeout;

        public override int ReadTimeout { get => base.ReadTimeout; set => base.ReadTimeout = value; }
        public override int WriteTimeout { get => base.WriteTimeout; set => base.WriteTimeout = value; }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                BaseStream.Dispose();
            }
            BaseStream = null;
            base.Dispose(disposing);
        }

        public DataStream(Archive archive, SectorInfo info)
        {
            Initialize(archive, info);
        }

        protected DataStream()
        {
        }

        /// <remarks>
        /// When files are added to MPQ archive they are
        /// first compressed, then encrypted (if applicable).
        /// Therefore unpacking files so do it in reverse order.
        /// </remarks>
        protected virtual void Initialize(Archive archive, SectorInfo block)
        {
            MyArchive = archive;
            var actualOffset = archive.ArchiveOffset + block.Position + 1;

            using (var accessor = archive.GetAccessorView(actualOffset - 1, block.Size))
            {
                var compressionFlags = (CompressionFlags)accessor.ReadByte(0);
                Stream underlyingStream = archive.GetStreamView(actualOffset, block.Size);

                if (block.Encrypted)
                {
                    underlyingStream = new DecryptStream(underlyingStream, block.DecryptionKey);
                }
                if(block.Compressed)
                {
                    underlyingStream = Decompress(underlyingStream, compressionFlags);
                }
                BaseStream = underlyingStream;
            }
        }

        protected virtual Stream Decompress(Stream underlyingStream, CompressionFlags flags)
        {
            if (flags.HasFlag(CompressionFlags.LZMA))
            {
                //var decoder = new LZMA.Decoder();
                //var buffer = new MemoryStream();
                //var proterties = new byte[5];
                //underlyingStream.Read(proterties, 0, 5);
                //decoder.SetDecoderProperties(proterties);
                //var lenBytes = new byte[8];
                //underlyingStream.Read(lenBytes, 0, 8);
                //var len = BitConverter.ToInt64(lenBytes, 0);
                //decoder.Code(underlyingStream, buffer, underlyingStream.Length, len, null);
                throw new NotImplementedException();
            }
            if (flags.HasFlag(CompressionFlags.BZIP2))
            {
                underlyingStream = new BZip2InputStream(underlyingStream);
            }
            if (flags.HasFlag(CompressionFlags.IMPLODED))
            {
                throw new NotImplementedException();
            }
            if (flags.HasFlag(CompressionFlags.DEFLATED))
            {
                underlyingStream = new InflaterInputStream(underlyingStream);
            }
            if (flags.HasFlag(CompressionFlags.SPARSE))
            {
                throw new NotImplementedException();
            }
            if (flags.HasFlag(CompressionFlags.HUFFMANN))
            {
                throw new NotImplementedException();
            }
            if (flags.HasFlag(CompressionFlags.ADPCM_STEREO))
            {
                throw new NotImplementedException();
            }
            if (flags.HasFlag(CompressionFlags.ADPCM_MONO))
            {
                throw new NotImplementedException();
            }
            return underlyingStream;
        }

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) =>
            BaseStream.CopyToAsync(destination, bufferSize, cancellationToken);

        public override void Flush() => BaseStream.Flush();

        public override Task FlushAsync(CancellationToken cancellationToken) => BaseStream.FlushAsync(cancellationToken);

        public override int Read(byte[] buffer, int offset, int count) => BaseStream.Read(buffer, offset, count);

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state) =>
            BaseStream.BeginRead(buffer, offset, count, callback, state);

        public override int EndRead(IAsyncResult asyncResult) => BaseStream.EndRead(asyncResult);

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
            BaseStream.ReadAsync(buffer, offset, count, cancellationToken);

        public override int ReadByte() => BaseStream.ReadByte();

        public override long Seek(long offset, SeekOrigin origin) => BaseStream.Seek(offset, origin);

        public override void SetLength(long value) => BaseStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) => BaseStream.Write(buffer, offset, count);

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) => BaseStream.BeginWrite(buffer, offset, count, callback, state);

        public override void EndWrite(IAsyncResult asyncResult) => BaseStream.EndWrite(asyncResult);

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => BaseStream.WriteAsync(buffer, offset, count, cancellationToken);

        public override void WriteByte(byte value) => BaseStream.WriteByte(value);

        public override object InitializeLifetimeService() => BaseStream.InitializeLifetimeService();

        public override void Close()
        {
            BaseStream.Close();
            base.Close();
        }

    }
}