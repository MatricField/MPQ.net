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
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MPQNet.HeaderDefinition;

namespace MPQNet
{
    public class HeaderSearcher
    {
        public Header Header { get; private set; }

        public UserDataHeader UserDataHeader { get; private set; }

        protected Stream InputStream { get; private set; }

        protected BinaryReader InputReader { get; private set; }

        private const int FORMAT_VERSION_OFFSET = 0xC;

        protected enum SearchResult
        {
            Found,
            UserDataFound,
            NotFound
        }

        public async Task Search(Stream input)
        {
            try
            {
                InputStream = input;
                InputReader = new BinaryReader(input);
                for(; ; )
                {
                    switch (SearchHeader())
                    {
                        case SearchResult.UserDataFound:
                            UserDataHeader = await ReadUserData();
                            InputStream.Seek(UserDataHeader.HeaderOffset, SeekOrigin.Begin);
                            break;
                        case SearchResult.Found:
                            Header = await ReadHeader();
                            return;
                        case SearchResult.NotFound:
                            throw new FormatException("Unable to find MPQ Archive header");
                    }
                }
            }
            finally
            {
                InputStream = null;
                InputReader = null;
            }
        }

        protected virtual SearchResult SearchHeader()
        {
            try
            {
                for (; ; )
                {
                    var headerSignature = (HeaderSignatures)InputReader.ReadUInt32();
                    switch (headerSignature)
                    {
                        case HeaderSignatures.MPQ_UserData:
                            InputStream.Seek(-sizeof(HeaderSignatures), SeekOrigin.Current);
                            return SearchResult.UserDataFound;
                        case HeaderSignatures.MPQ:
                        case HeaderSignatures.MPK:
                            InputStream.Seek(-sizeof(HeaderSignatures), SeekOrigin.Current);
                            return SearchResult.Found;
                        default:
                            InputStream.Seek(0x200 - sizeof(HeaderSignatures), SeekOrigin.Current);
                            break;
                    }
                }
            }
            catch(EndOfStreamException)
            {
                return SearchResult.NotFound;
            }
        }

        protected virtual async Task<Header> ReadHeader()
        {

            InputStream.Seek(FORMAT_VERSION_OFFSET, SeekOrigin.Current);
            var formatVersion = (FormatVersions)InputReader.ReadUInt16();
            InputStream.Seek(-FORMAT_VERSION_OFFSET - sizeof(FormatVersions), SeekOrigin.Current);
            switch(formatVersion)
            {
                case FormatVersions.V1:
                    var buffer = new byte[Marshal.SizeOf<HeaderV1>()];
                    await InputStream.ReadAsync(buffer, 0, buffer.Length);
                    unsafe
                    {
                        fixed (byte* pBuffer = buffer)
                        {
                            return Marshal.PtrToStructure<HeaderV1>((IntPtr)pBuffer);
                        }
                    }
                case FormatVersions.V2:
                    buffer = new byte[Marshal.SizeOf<HeaderV2>()];
                    await InputStream.ReadAsync(buffer, 0, buffer.Length);
                    unsafe
                    {
                        fixed (byte* pBuffer = buffer)
                        {
                            return Marshal.PtrToStructure<HeaderV2>((IntPtr)pBuffer);
                        }
                    }
                case FormatVersions.V3:
                    buffer = new byte[Marshal.SizeOf<HeaderV3>()];
                    await InputStream.ReadAsync(buffer, 0, buffer.Length);
                    unsafe
                    {
                        fixed (byte* pBuffer = buffer)
                        {
                            return Marshal.PtrToStructure<HeaderV3>((IntPtr)pBuffer);
                        }
                    }
                case FormatVersions.V4:
                    buffer = new byte[Marshal.SizeOf<HeaderV4>()];
                    await InputStream.ReadAsync(buffer, 0, buffer.Length);
                    unsafe
                    {
                        fixed (byte* pBuffer = buffer)
                        {
                            return Marshal.PtrToStructure<HeaderV4>((IntPtr)pBuffer);
                        }
                    }
                default:
                    throw new NotSupportedException("NotSupported format version");
            }
        }

        protected virtual async Task<UserDataHeader> ReadUserData()
        {
            var buffer = new byte[Marshal.SizeOf<UserDataHeader>()];
            await InputStream.ReadAsync(buffer, 0, buffer.Length);
            unsafe
            {
                fixed (byte* pBuffer = buffer)
                {
                    return Marshal.PtrToStructure<UserDataHeader>((IntPtr)pBuffer);
                }
            }
        }
    }
}
