using MPQNet.Definition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.BZip2;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using MPQNet.Helper;

namespace MPQNet.IO
{
    public class MPQFileStream :
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

        public MPQFileStream(Archive archive, MPQFileFlags flags, long fileOffset, int fileSize, string fileName = null)
        {
            Initialize(archive, flags, fileOffset, fileSize, fileName);
        }

        protected MPQFileStream()
        {
        }

        protected virtual void Initialize(Archive archive, MPQFileFlags flags, long fileOffset, int fileSize, string fileName = null)
        {
            MyArchive = archive;
            var actualOffset = archive.ArchiveOffset + fileOffset + 1;
            if(!flags.HasFlag(MPQFileFlags.SINGLE_UNIT))
            {
                // TODO: Add multiblock file support
                throw new NotImplementedException();
            }

            using (var accessor = archive.GetAccessorView(actualOffset - 1, fileSize))
            {
                var compressionMask = (CompressionMethodMasks)accessor.ReadByte(0);
                Stream underlyingStream = archive.GetStreamView(actualOffset, fileSize);
                if (compressionMask.HasFlag(CompressionMethodMasks.LZMA))
                {
                    throw new NotImplementedException();
                    BaseStream = underlyingStream;
                    return;
                }
                if (compressionMask.HasFlag(CompressionMethodMasks.BZIP2))
                {
                    underlyingStream = new BZip2InputStream(underlyingStream);
                }
                if (compressionMask.HasFlag(CompressionMethodMasks.IMPLODED))
                {
                    throw new NotImplementedException();
                }
                if(compressionMask.HasFlag(CompressionMethodMasks.DEFLATED))
                {
                    underlyingStream = new DeflateStream(underlyingStream, CompressionMode.Decompress);
                }
                if (compressionMask.HasFlag(CompressionMethodMasks.SPARSE))
                {
                    throw new NotImplementedException();
                }
                if (compressionMask.HasFlag(CompressionMethodMasks.HUFFMANN))
                {
                    throw new NotImplementedException();
                }
                if (compressionMask.HasFlag(CompressionMethodMasks.ADPCM_STEREO))
                {
                    throw new NotImplementedException();
                }
                if (compressionMask.HasFlag(CompressionMethodMasks.ADPCM_MONO))
                {
                    throw new NotImplementedException();
                }

                if(flags.HasFlag(MPQFileFlags.ENCRYPTED))
                {
                    var buffer = new MemoryStream();
                    underlyingStream.CopyTo(buffer);
                    MPQCryptor.DecryptDataInplace(buffer.GetBuffer(), MPQCryptor.HashString(fileName, HashType.FileKey));
                    underlyingStream.Dispose();
                    underlyingStream = buffer;
                }
                BaseStream =  underlyingStream;
            }
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
