using MPQNet.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MPQNet.IO
{
    public class MPQDecryptStream :
        Stream
    {

        public override bool CanRead => BaseStream.CanRead;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => BaseStream.Length;

        public override long Position
        {
            get => BaseStream.Position;
            set
            {
                if(0 == value)
                {
                    Reset();
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        private MPQCryptor Cryptor;

        private Stream BaseStream;

        private byte[] DecryptBuffer = new byte[256];

        public MPQDecryptStream(Stream baseStream, uint key)
        {
            BaseStream = baseStream;
            Cryptor = new MPQCryptor(key);
        }

        public override void Flush()
        {

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            GarunteeBufferSize(count);
            var acrualCount = BaseStream.Read(DecryptBuffer, 0, count);
            Cryptor.DecryptDataInplace(DecryptBuffer, 0, acrualCount);
            Buffer.BlockCopy(DecryptBuffer, 0, buffer, offset, acrualCount);
            return acrualCount;
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
            if(Position > value)
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

        private void GarunteeBufferSize(int count)
        {
            var len = DecryptBuffer.Length;
            while (len < count)
            {
                len *= 2;
            }
            DecryptBuffer = new byte[len];
        }
    }
}
