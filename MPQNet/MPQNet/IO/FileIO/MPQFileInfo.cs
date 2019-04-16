using MPQNet.Definition;
using System;
using System.Collections.Generic;
using System.Text;

namespace MPQNet.IO.FileIO
{
    public sealed class MPQFileInfo
    {
        public string Name { get; set; }

        public BlockEntry Block { get; set; }
    }
}
