using MPQNet.Definition;
using MPQNet.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static MPQNet.Helper.MarshalHelper;
using System.Text;

namespace MPQNet
{
    public class Archive3:
        IArchive
    {
        private ILowLevelIOHandler _IOHandler;
        private HetBetHashTable _HashTable;

        private UserDataHeader UserDataHeader { get; }

        public Archive3(ILowLevelIOHandler IOHandler, HetBetHashTable hashTable, UserDataHeader userDataHeader)
        {
            _IOHandler = IOHandler;
            _HashTable = hashTable;
            UserDataHeader = userDataHeader;
        }

        public static HetBetHashTable LoadHashTable(ILowLevelIOHandler IOHandler, ArchiveHeader3 header)
        {
            var hetStream = IOHandler.GetStream(Convert.ToInt64(header.HetTableOffset), 0);
            var betStream = IOHandler.GetStream(Convert.ToInt64(header.BetTableOffset), 0);

            return new HetBetHashTable(hetStream, betStream);
        }

        public static bool TryReadArchive(ILowLevelIOHandler IOHandler, UserDataHeader userData, out Archive3 archive)
        {
            using (var headerStream = IOHandler.GetStream(0, Marshal.SizeOf<ArchiveHeader3>()))
            {
                var header = headerStream.MarshalObjectFromStream<ArchiveHeader3>();
                if(header.HetTableOffset != 0)
                {
                    var hashTable = LoadHashTable(IOHandler, header);
                    archive = new Archive3(IOHandler, hashTable, userData);
                    return true;
                }
                else
                {
                    archive = null;
                    return false;
                }
            }
        }
    }
}
