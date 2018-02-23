using MPQNet.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MPQNet.Definition
{
    public static class TableInfo
    {
        public const string HashTableName = "(hash table)";
        public static readonly uint HashKey = MPQCryptor.HashString(HashTableName, HashType.FileKey);
    }
}
