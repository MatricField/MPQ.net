using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPQNet;
using System.IO;
using MPQNet.Header;
using IronPython.Hosting;
using System;

namespace MPQNet.UnitTests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {

            ArchiveHeader headerNet;
            using (var replayfile = File.Open(@"Replays\StandardBio.SC2Replay", FileMode.Open, FileAccess.Read))
            {
                var searcher = new HeaderSearcher();
                searcher.Search(replayfile).Wait();
                headerNet = searcher.Header;
            }
            dynamic Replay = MPYQ.Instance.MPQArchive(@"Replays\StandardBio.SC2Replay");
            dynamic headerPY = Replay.header;
            switch (headerNet)
            {
                case ArchiveHeaderV4 v4:

                    break;
                case ArchiveHeaderV3 v3:
                    break;
                case ArchiveHeaderV2 v2:
                    break;
                case ArchiveHeaderV1 v1:
                    Assert.AreEqual(headerPY["magic"], v1.ID, nameof(ArchiveHeaderV1.ID));
                    Assert.AreEqual(headerPY["header_size"], v1.HeaderSize, nameof(ArchiveHeaderV1.HeaderSize));
                    break;
            }
        }
    }
}
