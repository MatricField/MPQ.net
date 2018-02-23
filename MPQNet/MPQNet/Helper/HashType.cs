using System;
using System.Collections.Generic;
using System.Text;
using MPQNet.Helper;

namespace MPQNet.Helper
{
    /// <summary>
    /// Different types of hashes to make with <see cref="MPQCryptor"/>
    /// </summary>
    public enum HashType : uint
    {
        TableOffset = 0,
        NameA = 1,
        NameB = 2,
        FileKey = 3,
        Key2Mix = 4
    }
}
