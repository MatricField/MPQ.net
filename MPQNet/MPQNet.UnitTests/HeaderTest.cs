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
    public class HeaderTest
    {
        private const string replayPath = @"Replays\StandardBio.SC2Replay";
        [TestMethod]
        public void CanReadArchiveHeader()
        {
            HeaderSearcher Searcher;
            using (var stream = File.Open(replayPath, FileMode.Open, FileAccess.Read))
            {
                Searcher = new HeaderSearcher();
                Searcher.Search(stream).Wait();
            }
            using (var stormArchive = new StormArchive(replayPath))
            {
                Assert.AreEqual(stormArchive.Header, Searcher.Header);
                Assert.AreEqual(stormArchive.UserData, Searcher.UserDataHeader);
            }
        }
    }
}
