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
using System;
using System.Text;

namespace MPQNet.UnitTests
{
    [TestClass]
    public class HeaderTest
    {
        [TestMethod]
        public void CanReadArchiveHeader()
        {
            HeaderSearcher Searcher;
            using (var stream = File.Open(@"Replays\StandardBio.SC2Replay", FileMode.Open, FileAccess.Read))
            {
                Searcher = new HeaderSearcher();
                Searcher.Search(stream).Wait();
            }
            dynamic ReplayArchivePY = MPYQ.Instance.MPQArchive(@"Replays\StandardBio.SC2Replay");
            var headerNet = Searcher.Header;
            
            dynamic headerPY = ReplayArchivePY.header;
            switch (headerNet)
            {
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
            if(Searcher.UserDataHeader != null)
            {
                dynamic userDataPY = headerPY["user_data_header"];
                var userData = Searcher.UserDataHeader;
                Assert.AreEqual(userDataPY["magic"], Encoding.ASCII.GetString(BitConverter.GetBytes((uint)userData.ID)), nameof(UserDataHeader.ID));
                Assert.AreEqual(userDataPY["user_data_size"], (int)userData.UserDataSize, nameof(UserDataHeader.UserDataSize));
                Assert.AreEqual(userDataPY["mpq_header_offset"], (int)userData.HeaderOffset, nameof(UserDataHeader.HeaderOffset));
                Assert.AreEqual(userDataPY["user_data_header_size"], (int)userData.UserDataHeaderSize, nameof(UserDataHeader.UserDataHeaderSize));
            }
        }
    }
}
