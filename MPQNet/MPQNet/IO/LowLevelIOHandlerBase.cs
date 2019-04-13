using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MPQNet.IO
{
    public abstract class LowLevelIOHandlerBase :
        ILowLevelIOHandler, IDisposable
    {
        private long _BaseOffset;
        public long BaseOffset
        {
            get => _BaseOffset;
            set => Interlocked.Exchange(ref _BaseOffset, value);
        }

        public abstract Stream GetFullArchiveStream();

        public abstract Stream GetStream(long offset, long size);
        public abstract Task<Stream> GetStreamAsync(long offset, long size);

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void DoDispose(bool disposing)
        {

        }

        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                DoDispose(disposing);
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            DoDispose(true);
        }
        #endregion
    }
}
