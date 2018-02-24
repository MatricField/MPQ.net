using MPQNet.Definition;
using MPQNet.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MPQNet
{
    public class Archive3 :
        Archive2
    {
        public ArchiveHeader3 Header3 { get; protected set; }

        public Archive3(string path) :
            base(path)
        {
        }

        protected override async Task ReadMPQHeaderAsync(FormatVersions version, BinaryReader inputReader)
        {
            var stream = inputReader.BaseStream;
            switch (version)
            {
                case FormatVersions.V3:
                    Header = Header2 = Header3 = await stream.MarshalObjectFromStreamAsync<ArchiveHeader3>();
                    break;
                default:
                    await base.ReadMPQHeaderAsync(version, inputReader);
                    break;
            }
        }
    }
}
