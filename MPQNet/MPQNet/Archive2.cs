using MPQNet.Definition;
using MPQNet.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MPQNet
{
    public class Archive2 :
        Archive
    {
        public ArchiveHeader2 Header2 { get; protected set; }

        public Archive2(string path) :
            base(path)
        {
        }

        protected override async Task ReadMPQHeaderAsync(FormatVersions version, BinaryReader inputReader)
        {
            var stream = inputReader.BaseStream;
            switch (version)
            {
                case FormatVersions.V2:
                    Header = Header2 = await stream.MarshalObjectFromStreamAsync<ArchiveHeader2>();
                    break;
                default:
                    await base.ReadMPQHeaderAsync(version, inputReader);
                    break;
            }
        }
    }
}
