using MPQNet.Definition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MPQNet.IO
{
    public class MPQFileStream :
        Stream
    {
        protected int BlockSize { get; private set; }

        protected Stream BaseStream { get; private set; }

        public override bool CanRead => throw new NotImplementedException();

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => throw new NotImplementedException();

        public override long Position { get; set; }

        public override void Flush()
        {

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
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
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {

            }
            base.Dispose(disposing);
        }

        protected MPQFileStream()
        {
        }

        protected virtual void Initialize(Stream data, MPQFileFlags flags, int sectorSize, int fileSize)
        {
            throw new NotImplementedException();
        }
    }
}
