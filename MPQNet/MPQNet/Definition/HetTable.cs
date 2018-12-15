using System;
using System.Collections.Generic;
using System.Text;

namespace MPQNet.Definition
{
    public class HetTable
    {
        public ExtTableHeaderHet Header { get; }

        public IReadOnlyList<Byte> HetHashTable { get; }

        public HetTable(ExtTableHeaderHet header, IReadOnlyList<Byte> hetHashTable)
        {
            Header = header;
            HetHashTable = hetHashTable;
        }
    }
}
