using MPQNet.IO;
using System;
using System.Collections.Generic;
using System.Text;
using MPQNet.Helper;
using MPQNet.Definition;
using System.Runtime.InteropServices;
using System.IO;

namespace MPQNet
{
    public static class Archive
    {
        public static bool TryLoadArchiveFromFile(string path, out IArchive result)
        {
            var io = new MemoryMappedFileIO(path);
            using (var stream = io.GetFullArchiveStream())
            {
                var infoSize = Marshal.SizeOf<ArchiveInfo>();
                UserDataHeader userDataHeader = default;
                bool notStop = true;
                while(notStop)
                {
                    var info = stream.MarshalObjectFromStream<ArchiveInfo>();
                    stream.Seek(-infoSize, SeekOrigin.Current);
                    switch(info.ID)
                    {
                        case Signatures.MPQ_UserData:
                            var userDataHeaderOffset = stream.Position;
                            userDataHeader = stream.MarshalObjectFromStream<UserDataHeader>();
                            stream.Seek(userDataHeaderOffset + userDataHeader.HeaderOffset, SeekOrigin.Begin);
                            break;
                        case Signatures.MPQ:
                            io.BaseOffset = stream.Position;
                            switch(info.FormatVersion)
                            {
                                case FormatVersions.V4:
                                    if(false)
                                    {

                                    }
                                    else
                                    {
                                        goto case FormatVersions.V3;
                                    }
                                case FormatVersions.V3:
                                    if(Archive3.TryReadArchive3(io, userDataHeader, out var archive3))
                                    {
                                        result = archive3;
                                        return true;
                                    }
                                    else
                                    {
                                        goto case FormatVersions.V2;
                                    }
                                case FormatVersions.V2:
                                    throw new NotImplementedException();
                                case FormatVersions.V1:
                                    throw new NotImplementedException();
                                default:
                                    throw new NotSupportedException();
                            }
                        default:
                            var pos = stream.Position;
                            if(pos == stream.Seek(0x200, SeekOrigin.Current))
                            {
                                notStop = false;
                            }
                            break;
                    }
                }
            }
            result = default;
            return false;
        }
    }
}
