using MPQNet.Cryptography;
using MPQNet.Definition;
using MPQNet.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace MPQNet.IO
{
    public class BlockHashTable : IMPQHashTable
    {
        private HashEntry[] _HashTable;

        private BlockEntry[] _BlockTable;

        public BlockHashTable(ILowLevelIOHandler IOHandler, long hashOffset, int hashCount, long blockOffset, int blockCount)
        {
            _HashTable = ReadTable<HashEntry>(IOHandler, hashOffset, hashCount, SpecialFiles.HashTableKey);
            _BlockTable = ReadTable<BlockEntry>(IOHandler, blockOffset, blockCount, SpecialFiles.BlockTableKey);
        }

        public BlockEntry this[string key]
        {
            get
            {
                if (TryGetValue(key, out var val))
                {
                    return val;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        public bool TryGetValue(string key, out BlockEntry value)
        {
            var index = MPQHash.HashPath(key, HashType.TableOffset);
            var name1 = MPQHash.HashPath(key, HashType.NameA);
            var name2 = MPQHash.HashPath(key, HashType.NameB);

            for (var i = index & (_HashTable.Length - 1); ; ++i)
            {
                var currentBlock = _HashTable[(int)i];
                if (currentBlock.Name1 == name1 &&
                    currentBlock.Name2 == name2)
                {
                    if (currentBlock.BlockIndex == HashEntry.HASH_ENTRY_NO_LONGER_VALID)
                    {
                        value = default;
                        return false;
                    }
                    value = _BlockTable[currentBlock.BlockIndex];
                    return true;
                }
                if (currentBlock.BlockIndex == HashEntry.HASH_ENTRY_IS_EMPTY)
                {
                    value = default;
                    return false;
                }
            }
        }

        private T[] ReadTable<T>(ILowLevelIOHandler IOHandler, long offset, int count, uint key)
        {
            var size = count * Marshal.SizeOf<T>();
            using (var stream = IOHandler.GetStream(offset, size))
            using (var decrypted = new CryptoStream(stream, new MPQCryptoTransform(key), CryptoStreamMode.Read))
            {
                //var data = new byte[size];
                //stream.Read(data, 0, data.Length);
                //var decryptor = new MPQCryptor(key);
                //decryptor.DecryptDataInplace(data);
                return decrypted.MarshalArrayFromStream<T>(count);
            }
        }
    }
}
