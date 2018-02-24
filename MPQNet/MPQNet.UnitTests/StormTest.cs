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
using MPQNet.Definition;
using System;
using System.Text;
using MPQNet.UnitTests.StormLib;

namespace MPQNet.UnitTests
{
    [TestClass]
    public class StormTest
    {
        private const string replayPath = @"Replays\StandardBio.SC2Replay";

        [TestMethod]
        public void CanReadArchiveHeader()
        {
            var MyArchive = new Archive4(replayPath);
            using (var stormArchive = new StormArchive(replayPath))
            {
                switch(stormArchive.Header.FormatVersion)
                {
                    case FormatVersions.V4:
                        Assert.AreEqual(MyArchive.Header4, stormArchive.Header as ArchiveHeader4);
                        goto case FormatVersions.V3;
                    case FormatVersions.V3:
                        Assert.AreEqual(MyArchive.Header3, stormArchive.Header as ArchiveHeader3);
                        goto case FormatVersions.V2;
                    case FormatVersions.V2:
                        Assert.AreEqual(MyArchive.Header2, stormArchive.Header as ArchiveHeader2);
                        goto case FormatVersions.V1;
                    case FormatVersions.V1:
                        Assert.AreEqual(MyArchive.Header, stormArchive.Header);
                        break;
                }
                
                Assert.AreEqual(stormArchive.UserData, MyArchive.UserDataHeader);
            }
        }

        [TestMethod]
        public void CanLoadHashTable()
        {
            var myArchive = new Archive4(replayPath);
            using (var stormArchive = new StormArchive(replayPath))
            {
                try
                {
                    for (int i = 0; i < stormArchive.HashTable.Count; ++i)
                    {
                        Assert.AreEqual(stormArchive.HashTable[i], myArchive.HashTable[i], "hash entry not equal");
                    }
                }
                catch(IndexOutOfRangeException)
                {
                    Assert.Fail("hash table size mismatch");
                }
            }
        }
    }
}
