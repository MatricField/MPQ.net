//MIT License

//Copyright(c) 2023 Mingxi "Lucien" Du

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

using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace MPQNet.Cryptography
{
    /// <summary>
    /// Encrypting/Decrypting MPQ data block
    /// </summary>
    internal class DataBlockCypher :
        ICryptoTransform
    {
        private const int BLOCK_SIZE = sizeof(uint);

        private const uint DEFAULT_KEY_2 = 0xEEEEEEEE;

        private uint dwKey1;

        private uint dwKey2;


        public DataBlockCypher(uint key)
        {
            dwKey1 = key;
            dwKey2 = DEFAULT_KEY_2;
        }

        public bool CanReuseTransform => false;

        public bool CanTransformMultipleBlocks => true;

        public int InputBlockSize => BLOCK_SIZE;

        public int OutputBlockSize => BLOCK_SIZE;

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            //CheckBuffer(inputBuffer, inputOffset, inputCount);
            //CheckBuffer(outputBuffer, outputOffset);
            //var buffer = BitConverter.ToUInt32(inputBuffer, inputOffset);
            //buffer = Transform(buffer);
            //BitConverter.GetBytes(buffer).CopyTo(outputBuffer, outputOffset);
            //return OutputBlockSize;

            var inputSpan = inputBuffer.AsSpan(inputOffset, inputCount);
            var outputSpan = outputBuffer.AsSpan(outputOffset);

            var input = MemoryMarshal.Cast<byte, uint>(inputSpan);
            var output = MemoryMarshal.Cast<byte, uint>(outputSpan);

            int i = 0;
            foreach(var block in input)
            {
                output[i] = Transform(block);
                i++;
            }
            return i * BLOCK_SIZE;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if(inputCount == 0)
            {
                return Array.Empty<byte>();
            }
            CheckBuffer(inputBuffer, inputOffset, inputCount);
            var buffer = BitConverter.ToUInt32(inputBuffer, inputOffset);
            buffer = Transform(buffer);
            return BitConverter.GetBytes(buffer);
        }

        public void Dispose()
        {
            dwKey1 = default;
            dwKey2 = default;
        }

        /// <summary>
        /// Encrypt or decrypt 1 DWORD
        /// </summary>
        private uint Transform(uint original)
        {
            dwKey2 += Common.CryptTable[(int)(HashType.Key2Mix + (dwKey1 & 0xFF))];
            var result = original ^ (dwKey1 + dwKey2);
            dwKey1 = ((~dwKey1 << 0x15) + 0x11111111) | (dwKey1 >> 0x0B);
            dwKey2 = result + dwKey2 + (dwKey2 << 5) + 3;
            return result;
        }

        private void CheckBuffer(byte[] buffer, int offset, int byteCount = BLOCK_SIZE)
        {
            if (byteCount != BLOCK_SIZE)
            {
                throw new ArgumentException("Invalid buffer size");
            }
            if(offset + byteCount > buffer.Length)
            {
                throw new ArgumentException("Buffer too short");
            }
        }
    }
}
