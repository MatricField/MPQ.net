using MPQNet.Cryptography;
using System;
using System.IO;

namespace MPQNet.IO.Streams
{
    public class DecryptStream:
        Stream
    {
        public override bool CanRead => BaseStream.CanRead;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => BaseStream.Length;

        private long _Position;

        public override long Position
        {
            get => _Position;
            set => throw new NotSupportedException();
        }

        private MPQCryptor Cryptor;

        private Stream BaseStream;

        private const int BUFFER_SIZE = 256;

        private MemoryStream DecryptBuffer;

        public DecryptStream(Stream baseStream, uint key)
        {
            BaseStream = baseStream;
            Cryptor = new MPQCryptor(key);
            DecryptBuffer = new MemoryStream();
            ReadAndDecrypt();
        }

        public override void Flush()
        {

        }

        public override int ReadByte()
        {
            if (DecryptBuffer.Position == DecryptBuffer.Length)
            {
                ReadAndDecrypt();
            }
            var ret = DecryptBuffer.ReadByte();
            ++_Position;
            return ret;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var readCount = 0;
            while (Position != Length && count - readCount > 0)
            {
                readCount += DecryptBuffer.Read(buffer, offset + readCount, count - readCount);
                ReadAndDecrypt(count - readCount);
            }
            _Position += readCount;
            return readCount;
        }

        private void ReadAndDecrypt(int count = BUFFER_SIZE)
        {
            var readCount = Convert.ToInt32(BaseStream.Length - BaseStream.Position);
            DecryptBuffer.SetLength(count);
            if (readCount < count)
            {
                DecryptBuffer.Position = 0;
                BaseStream.CopyTo(DecryptBuffer);
            }
            else
            {
                readCount = BaseStream.Read(DecryptBuffer.GetBuffer(), 0, count);
            }
            Cryptor.DecryptDataInplace(DecryptBuffer.GetBuffer(), 0, readCount);
            DecryptBuffer.Position = 0;
            DecryptBuffer.SetLength(readCount);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (0 == offset && origin == SeekOrigin.Begin)
            {
                Reset();
                return 0;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override void SetLength(long value)
        {
            if (Position > value)
            {
                throw new NotSupportedException();
            }
            BaseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public void Reset()
        {
            BaseStream.Seek(0, SeekOrigin.Begin);
            Cryptor.Reset();
        }
    }
}
