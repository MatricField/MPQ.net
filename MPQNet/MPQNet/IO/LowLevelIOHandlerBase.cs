using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MPQNet.IO
{
    public abstract class LowLevelIOHandlerBase :
        ILowLevelIOHandler
    {
        private long _BaseOffset;
        public long BaseOffset
        {
            get => _BaseOffset;
            set => Interlocked.Exchange(ref _BaseOffset, value);
        }

        public abstract Stream GetStream(long offset, long size);
        public abstract Task<Stream> GetStreamAsync(long offset, long size);
    }
}
