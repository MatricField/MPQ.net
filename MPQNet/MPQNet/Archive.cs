using MPQNet.ArchiveDetails;
using MPQNet.Definition;
using MPQNet.IO;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MPQNet
{
    public static class Archive
    {
            
        public static IArchive OpenArchive(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
            using (var stream = FileIO.OpenShared(path))
            {
                var binaryReader = new BinaryReader(stream);
                long headerBaseAddress = 0, userDataBaseAddress = 0;
                UserDataHeader1 userDataHeader = null;
                for(; ; )
                {
                    var sig = (Signatures)binaryReader.ReadUInt32();
                    switch (sig)
                    {
                        case Signatures.MPQ:
                            if (0 == headerBaseAddress)
                            {
                                headerBaseAddress = stream.Position - 4;
                            }
                            var headerSize = binaryReader.ReadUInt32();
                            var archiveSize = binaryReader.ReadUInt32();
                            var version = binaryReader.ReadUInt16();
                            var headerData = new byte[208];
                            stream.Read(headerData, 0, (int)(headerSize - 12));
                            var rawHeader = MemoryMarshal.AsRef<RawHeader>(headerData);
                            switch(version)
                            {
                                case 0:
                                    
                                case 1:
                                case 2:
                                case 3:
                                    return new Archive1(path, new Header4(rawHeader, headerBaseAddress), userDataHeader);
                                default:
                                    throw new NotSupportedException();
                            }
                        case Signatures.MPQ_UserData:
                            userDataBaseAddress = stream.Position - 4;
                            var userDataHeaderBytes = binaryReader.ReadBytes(0xC);
                            var userDataHeaderRaw = MemoryMarshal.AsRef<RawUserData>(userDataHeaderBytes);
                            userDataHeader = new UserDataHeader1(userDataHeaderRaw, userDataBaseAddress);
                            headerBaseAddress = userDataBaseAddress + userDataHeader.HeaderOffset;
                            stream.Seek(headerBaseAddress, SeekOrigin.Begin);
                            break;
                        default:
                            stream.Seek(0x1FC, SeekOrigin.Current);
                            break;
                    }

                }

                throw new InvalidDataException();
            }
        }
    }
}
