using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MPQNet.Definition;
using MPQNet.IO;

namespace MPQNet
{
    public class BlockTableArchive :
        MPQArchiveBase
    {
        public BlockTableArchive(ILowLevelIOHandler IOHandler, IMPQHashTable hashTable, UserDataHeader userData = null)
            : base(IOHandler, hashTable, userData)
        {
        }

        protected override Stream OpenFileStream(IMPQFileInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
