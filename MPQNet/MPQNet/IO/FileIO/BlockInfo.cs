//using MPQNet.Definition;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace MPQNet.IO.FileIO
//{
//    internal class BlockInfo
//    {
//        public RawBlockEntry Entry { get; set; }

//        public uint DecryptionKey { get; set; }

//        public bool IsCompressed =>
//            Entry.Flags.HasFlag(MPQFileFlags.COMPRESS) &&
//            Entry.FileSize > Entry.CompressedSize;

//        public bool IsEncrypted =>
//            Entry.Flags.HasFlag(MPQFileFlags.ENCRYPTED);
//    }
//}
