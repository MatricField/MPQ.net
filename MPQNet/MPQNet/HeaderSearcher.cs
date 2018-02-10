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
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MPQNet.Header;
using MPQNet.Helper;

namespace MPQNet
{
    public class HeaderSearcher
    {
        public Header Header { get; private set; }

        public UserDataHeader UserDataHeader { get; private set; }

        protected long UserDataOffset { get; set; }

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
                            InputStream.Seek(UserDataOffset + UserDataHeader.HeaderOffset, SeekOrigin.Begin);
                            break;
                        case SearchResult.Found:
                            Header = await ReadHeader();
                            return;
                        case SearchResult.NotFound:
                            throw new InvalidDataException("Unable to find MPQ Archive header");
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
                    var headerSignature = (ArchiveHeaderSignatures)InputReader.ReadUInt32();
                    switch (headerSignature)
                    {
                        case ArchiveHeaderSignatures.MPQ_UserData:
                            InputStream.Seek(-sizeof(ArchiveHeaderSignatures), SeekOrigin.Current);
                            UserDataOffset = InputStream.Position;
                            return SearchResult.UserDataFound;
                        case ArchiveHeaderSignatures.MPQ:
                        case ArchiveHeaderSignatures.MPK:
                            InputStream.Seek(-sizeof(ArchiveHeaderSignatures), SeekOrigin.Current);
                            return SearchResult.Found;
                        default:
                            InputStream.Seek(0x200 - sizeof(ArchiveHeaderSignatures), SeekOrigin.Current);
                            break;
                    }
                }
            }
            catch(EndOfStreamException)
            {
                return SearchResult.NotFound;
            }
        }

        protected virtual async Task<Header.ArchiveHeader> ReadHeader()
        {

            InputStream.Seek(FORMAT_VERSION_OFFSET, SeekOrigin.Current);
            var formatVersion = (FormatVersions)InputReader.ReadUInt16();
            InputStream.Seek(-FORMAT_VERSION_OFFSET - sizeof(FormatVersions), SeekOrigin.Current);
            switch(formatVersion)
            {
                case FormatVersions.V1:
                    return await InputStream.MarshalObjectFromBytesAsync<ArchiveHeaderV1>();
                case FormatVersions.V2:
                    return await InputStream.MarshalObjectFromBytesAsync<ArchiveHeaderV2>();
                case FormatVersions.V3:
                    return await InputStream.MarshalObjectFromBytesAsync<ArchiveHeaderV3>();
                case FormatVersions.V4:
                    return await InputStream.MarshalObjectFromBytesAsync<ArchiveHeaderV4>();
                default:
                    throw new NotSupportedException("NotSupported format version");
            }
        }

        protected virtual Task<UserDataHeader> ReadUserData()
        {
            return InputStream.MarshalObjectFromBytesAsync<UserDataHeader>();
        }
    }
}
