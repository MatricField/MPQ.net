using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPQNet;
using MPQNet.HeaderDefinition;



namespace Test0001
{
    class Program
    {

        static void Main(string[] args)
        {
            Header header;
            using (var stream = File.Open("TestReplay.SC2Replay", FileMode.Open))
            {
                var searcher = new HeaderSearcher();
                searcher.Search(stream).Wait();
                header = searcher.Header;
            }
            switch(header)
            {
                case HeaderV2 h2:
                    break;
                case HeaderV1 h1:
                    break;
                default:
                    break;
            }
        }
    }
}
