using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MPQNet.UnitTests.StormLib
{
    [StructLayout(LayoutKind.Sequential)]
    public class StormPtr :
        IDisposable
    {
        protected IntPtr ptr { get; }

        public T MarshalAs<T>()
        {
            return Marshal.PtrToStructure<T>(ptr);
        }

        public T[] MarshalAsArray<T>(int count)
        {
            var diff = Marshal.SizeOf<T>();
            var result = new T[count];
            var pObj = ptr;
            for(int i = 0; i < count; ++i)
            {
                result[i] = Marshal.PtrToStructure<T>(pObj);
                pObj += diff;
            }
            return result;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void DoDispose()
        {
            if (!disposedValue)
            {
                Interop.free(ptr);
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~StormPtr()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            DoDispose();
        }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            DoDispose();
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}
