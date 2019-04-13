using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MPQNet.Definition;
using System.Data.HashFunction.Jenkins;

namespace MPQNet.IO
{
    public class V3HashTable
    {
        private HetTableHeader _HetHeader;
        private BetTableHeader _BetHeader;
        private byte[] _HetHashes;
        private byte[] _BetHashes;
        private MPQFileInfo[] _FileInfos;

        public V3HashTable(Stream hetStream, Stream betStream)
        {

        }

        public MPQFileInfo this[string key]
        {
            get
            {
                if(TryGetValue(key, out var val))
                {
                    return val;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out MPQFileInfo value)
        {
            throw new NotImplementedException();
        }
    }
}
