using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MPQNet.Cryptography
{
    public sealed class MPQCryptor :
        MPQCryptorBase
    {
        public MPQCryptor(uint key, uint hashKey = 4) :
            base(key, hashKey)
        {
        }

        public uint Decrypt(uint encrypted)
        {
            IV2 += CryptTable[(int)(HashKey + (IV1 & 0xFF))];
            var decrypted = encrypted ^ (IV1 + IV2);
            IV1 = ((~IV1 << 0x15) + 0x11111111) | (IV1 >> 0x0B);
            IV2 = decrypted + IV2 + (IV2 << 5) + 3;
            return decrypted;
        }

        public uint Encrypt(uint decrypted)
        {
            IV2 += CryptTable[(int)(HashKey + (IV1 & 0xFF))];
            var encrypted = decrypted ^ (IV1 + IV2);
            IV1 = ((~IV1 << 0x15) + 0x11111111) | (IV1 >> 0x0B);
            IV2 = decrypted + IV2 + (IV2 << 5) + 3;
            return encrypted;
        }

        public void Encrypt(BinaryReader input, BinaryWriter output)
        {
            try
            {
                for (; ; )
                {
                    var decrypted = input.ReadUInt32();
                    output.Write(Encrypt(decrypted));
                }
            }
            catch (EndOfStreamException)
            {

            }
        }

        public void Decrypt(BinaryReader input, BinaryWriter output)
        {
            try
            {
                for (; ; )
                {
                    var encrypted = input.ReadUInt32();
                    output.Write(Decrypt(encrypted));
                }
            }
            catch (EndOfStreamException)
            {

            }
        }

        public void DecryptDataInplace(byte[] data)
        {
            var reader = new BinaryReader(new MemoryStream(data));
            var writer = new BinaryWriter(new MemoryStream(data));
            Decrypt(reader, writer);
        }

        public void DecryptDataInplace(byte[] data, int offset, int count)
        {
            var reader = new BinaryReader(new MemoryStream(data, offset, count));
            var writer = new BinaryWriter(new MemoryStream(data, offset, count));
            Decrypt(reader, writer);
        }

        public static uint GetFileKey(string path)
        {
            var name = Path.GetFileName(path);
            return MPQHash.HashName(name, HashType.FileKey);
        }
    }
}
