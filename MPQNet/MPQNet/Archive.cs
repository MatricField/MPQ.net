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
        public static IArchive LoadArchiveFromFile(string path)
        {
            var io = new MemoryMappedFileIO(path);
            IArchive result = null;
            using (var stream = io.GetFullArchiveStream())
            {
                var size = Marshal.SizeOf<ArchiveInfo>();
                UserDataHeader userDataHeader;
                bool notStop = true;
                while(notStop)
                {
                    var info = stream.MarshalObjectFromStream<ArchiveInfo>();
                    switch(info.ID)
                    {
                        case Signatures.MPQ_UserData:
                            var userDataHeaderOffset = stream.Position;
                            userDataHeader = stream.MarshalObjectFromStream<UserDataHeader>();
                            stream.Seek(userDataHeaderOffset + userDataHeader.HeaderOffset, SeekOrigin.Begin);
                            break;
                        case Signatures.MPQ:
                            throw new NotImplementedException();
                            //TODO: implement LoadArchive
                            break;
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
            return result;
        }
    }
}
