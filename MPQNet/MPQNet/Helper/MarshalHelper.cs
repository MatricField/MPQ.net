//MIT License

//Copyright(c) 2018 Mingxi "Lucien" Du

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MPQNet.Helper
{
    /// <summary>
    /// Helper class to marshal objects from stream
    /// </summary>
    public static class MarshalHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MarshalObjectFromBuffer<T>(this byte[] buffer)
        {
            var hBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                return Marshal.PtrToStructure<T>(hBuffer.AddrOfPinnedObject());
            }
            finally
            {
                if (hBuffer.IsAllocated)
                {
                    hBuffer.Free();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] MarshalArrayFromBuffer<T>(this byte[] buffer, int count)
        {
            var pBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                var result = new T[count];
                var pObject = pBuffer.AddrOfPinnedObject();
                for (int i = 0; i < count; ++i)
                {
                    result[i] = Marshal.PtrToStructure<T>(pObject);
                    pObject += Marshal.SizeOf<T>();
                }
                return result;
            }
            finally
            {
                if (pBuffer.IsAllocated)
                {
                    pBuffer.Free();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<T> MarshalObjectFromStreamAsync<T>(this Stream stream)
        {
            var buffer = new byte[Marshal.SizeOf<T>()];
            await stream.ReadAsync(buffer, 0, buffer.Length);
            return buffer.MarshalObjectFromBuffer<T>();
        }

        public static async Task<T[]> MarshalArrayFromStreamAsync<T>(this Stream stream, int count)
        {
            var buffer = new byte[Marshal.SizeOf<T>()];
            var result = new T[count];
            for(int i = 0; i < count; ++i)
            {
                await stream.ReadAsync(buffer, 0, buffer.Length);
                result[i] = buffer.MarshalObjectFromBuffer<T>();
            }
            return result;
        }

        public static T[] MarshalArrayFromStream<T>(this Stream stream, int count)
        {
            var buffer = new byte[Marshal.SizeOf<T>()];
            var result = new T[count];
            for (int i = 0; i < count; ++i)
            {
                stream.Read(buffer, 0, buffer.Length);
                result[i] = buffer.MarshalObjectFromBuffer<T>();
            }
            return result;
        }

        public static Task<T[]> MarshalArrayFromStreamAsync<T>(this MemoryStream stream, int count)
        {
            return Task.FromResult(stream.MarshalArrayFromStream<T>(count));
        }

        public static T[] MarshalArrayFromStream<T>(this MemoryStream stream, int count)
        {
            return stream.GetBuffer().MarshalArrayFromBuffer<T>(count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MarshalObjectFromStream<T>(this Stream stream)
        {
            var buffer = new byte[Marshal.SizeOf<T>()];
            stream.Read(buffer, 0, buffer.Length);
            return buffer.MarshalObjectFromBuffer<T>();
        }
    }
}
