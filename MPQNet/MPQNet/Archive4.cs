using MPQNet.Definition;
using MPQNet.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MPQNet
{
    public class Archive4 :
        Archive3
    {
        public ArchiveHeader4 Header4 { get; protected set; }

        public Archive4(string path) :
            base(path)
        {
        }

        protected override async Task ReadMPQHeaderAsync(FormatVersions version, BinaryReader inputReader)
        {
            var stream = inputReader.BaseStream;
            switch (version)
            {
                case FormatVersions.V4:
                    Header = Header2 = Header3 = Header4 = await stream.MarshalObjectFromStreamAsync<ArchiveHeader4>();
                    break;
                default:
                    await base.ReadMPQHeaderAsync(version, inputReader);
                    break;
            }
        }
    }
}
