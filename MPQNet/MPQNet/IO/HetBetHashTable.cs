using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MPQNet.Definition;
using MPQNet.Helper;
using System.Data.HashFunction.Jenkins;
using System.Runtime.InteropServices;

namespace MPQNet.IO
{
    internal class HetBetHashTable: 
        IMPQHashTable
    {
        private HetTableHeader _HetHeader;
        private BetTableHeader _BetHeader;
        private byte[] _HetHashes;
        private byte[] _BetHashes;

        public HetBetHashTable(Stream hetStream, Stream betStream)
        {
            _HetHeader = hetStream.MarshalObjectFromStream<HetTableHeader>();
            _BetHeader = betStream.MarshalObjectFromStream<BetTableHeader>();
            _HetHashes = new byte[_HetHeader.TotalCount];

            hetStream.Read(_HetHashes, 0, _HetHashes.Length);
            {
                var buffer = new byte[_HetHeader.TotalCount];

            }


            var fileFlags = new MPQFileFlags[_BetHeader.FlagCount];
            {
                var reader = new BinaryReader(betStream);
                for(int i = 0; i < fileFlags.Length; ++i)
                {
                    fileFlags[i] = (MPQFileFlags)reader.ReadUInt32();
                }
            }

        }

        public RawBlockEntry this[string key]
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

        public bool TryGetValue(string key, out RawBlockEntry value)
        {
            throw new NotImplementedException();
        }
    }
}
