using System;
using System.Collections.Generic;
using System.Text;
using MPQNet.Helper;

namespace MPQNet.Helper
{
    /// <summary>
    /// Different types of hashes to make with <see cref="MPQCryptor"/>
    /// </summary>
    public static class HashType
    {
        public const uint TableOffset = 0;
        public const uint NameA = 1;
        public const uint NameB = 2;
        public const uint FileKey = 3;
        public const uint Key2Mix = 4;
    }
}
