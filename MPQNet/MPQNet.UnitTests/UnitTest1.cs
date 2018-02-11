//MIT License

//Copyright(c) 2018 Mingxi "Lucien" Du

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MPQNet.Header;

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
                    Assert.AreEqual(headerPY["format_version"], v1.FormatVersion, nameof(ArchiveHeaderV1.FormatVersion));
                    Assert.AreEqual(headerPY["sector_size_shift"], v1.SectorSizeShift, nameof(ArchiveHeaderV1.SectorSizeShift));
                    Assert.AreEqual(headerPY["hash_table_offset"], v1.HashTableOffset, nameof(ArchiveHeaderV1.HashTableOffset));
                    Assert.AreEqual(headerPY["block_table_offset"], v1.BlockTableOffset, nameof(ArchiveHeaderV1.BlockTableOffset));
                    Assert.AreEqual(headerPY["hash_table_entries"], v1.HashTableEntriesCount, nameof(ArchiveHeaderV1.HashTableEntriesCount));
                    Assert.AreEqual(headerPY["block_table_entries"], v1.BlockTableEntriesCount, nameof(ArchiveHeaderV1.BlockTableEntriesCount));
                    break;
            }
        }
    }
}
