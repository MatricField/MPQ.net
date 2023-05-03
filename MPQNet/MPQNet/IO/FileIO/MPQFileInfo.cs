using MPQNet.Definition;
using System;
using System.Collections.Generic;
using System.Text;

namespace MPQNet.IO.FileIO
{
    internal sealed class MPQFileInfo
    {
        public string Name { get; set; }

        public RawBlockEntry Block { get; set; }
    }
}
