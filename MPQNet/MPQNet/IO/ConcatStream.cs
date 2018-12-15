using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MPQNet.IO
{
    public class ConcatStream :
        Stream
    {
        public override bool CanRead { get; }

        public override bool CanSeek { get; }

        public override bool CanWrite { get; }

        private long _Length;

        public override long Length =>
            CanSeek ? _Length : throw new NotSupportedException();

        private long _Position;

        public override long Position
        {
            get =>
                CanSeek ? _Position : throw new NotSupportedException();
            set =>
                _Position = CanSeek ? _Position : throw new NotSupportedException();
        }

        protected IReadOnlyList<Stream> UnderlyingStreams { get; set; }

        protected Stream CurrentStream { get; set; }

        protected int CurrentStreamIndex;

        public ConcatStream(IReadOnlyList<Stream> streams)
        {
            UnderlyingStreams = streams.Count > 0 ? streams : new[] { new MemoryStream() };
            CanRead = streams.All(s => s.CanRead);
            CanSeek = streams.All(s => s.CanSeek);
            CanWrite = streams.All(s => s.CanWrite);
            if(CanSeek)
            {
                _Length = streams.Sum(s => s.Length);
            }
            CurrentStream = UnderlyingStreams[0];
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if(!CanRead)
            {
                throw new NotSupportedException();
            }
            var alreadyRead = 0;
            while(CurrentStreamIndex < UnderlyingStreams.Count - 1 && alreadyRead < count)
            {
                alreadyRead += CurrentStream.Read(buffer, offset + alreadyRead, count - alreadyRead);
                ++CurrentStreamIndex;
                CurrentStream = UnderlyingStreams[CurrentStreamIndex];
            }
            return alreadyRead;
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (!CanRead)
            {
                throw new NotSupportedException();
            }
            var alreadyRead = 0;
            while (CurrentStreamIndex < UnderlyingStreams.Count && alreadyRead < count)
            {
                alreadyRead += await CurrentStream.ReadAsync(buffer, offset + alreadyRead, count - alreadyRead);
                ++CurrentStreamIndex;
                CurrentStream = UnderlyingStreams[CurrentStreamIndex];
            }
            return alreadyRead;
        }

        public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            if(!CanRead)
            {
                throw new NotSupportedException();
            }

            foreach(var stream in UnderlyingStreams)
            {
                await stream.CopyToAsync(destination, bufferSize, cancellationToken);
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
