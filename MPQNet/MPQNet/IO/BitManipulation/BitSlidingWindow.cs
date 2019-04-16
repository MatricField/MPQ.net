using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MPQNet.IO.BitManipulation
{
    public class BitSlidingWindow
    {
        private const int BITS_IN_UNIT = 8;

        private int offset = 0;

        public BitSlidingWindow(int initialOffset = 0)
        {
            if(initialOffset >= BITS_IN_UNIT || initialOffset < 0)
            {
                throw new ArgumentException();
            }
            offset = initialOffset;
        }

        public void Copy(Span<byte> source, Span<byte> dest, int bitCount)
        {
            Debug.Assert(offset >= 0);

            if(source.Length * BITS_IN_UNIT < bitCount || dest.Length * BITS_IN_UNIT < bitCount || bitCount < 0)
            {
                throw new ArgumentException();
            }

            if(offset == 0)
            {
                var writeByteCount = bitCount / BITS_IN_UNIT;
                source.Slice(0, writeByteCount).CopyTo(dest);
                bitCount %= BITS_IN_UNIT;

                if(bitCount > 0)
                {
                    var b = source[writeByteCount];
                    var mask = MaskTable[bitCount];
                    dest[writeByteCount] = (byte)(b & mask);
                    offset = bitCount;
                }

                Debug.Assert(bitCount > 0 && offset > 0 && offset < 8);
            }
            else
            {
                var size1 = offset;
                var size2 = BITS_IN_UNIT - offset;
                var mask2 = MaskTable[size2];
                var si = 0;
                var di = 0;
                bool isStopInChunck2;
                for(; ; )
                {
                    if(bitCount >= size2)
                    {
                        var b = source[si];
                        dest[di] |= (byte)(b & mask2);
                        bitCount -= size2;
                        ++si;
                    }
                    else
                    {
                        isStopInChunck2 = true;
                        break;
                    }

                    if(bitCount >= size1)
                    {
                        var b = source[si];
                        dest[di] |= (byte)(b << size1);
                        bitCount -= size1;
                        ++di;
                    }
                    else
                    {
                        isStopInChunck2 = false;
                        break;
                    }
                }
                if(isStopInChunck2)
                {
                    var b = source[si];
                    dest[di] |= (byte)(b & MaskTable[bitCount]);
                }
                else
                {
                    var b = source[si];
                    dest[di] |= (byte)(b << bitCount);
                }
                offset = bitCount;
            }
        }

        private static readonly byte[] MaskTable =
            new byte[]
            {
                0b00000000,
                0b00000001,
                0b00000011,
                0b00000111,
                0b00001111,
                0b00011111,
                0b00111111,
                0b01111111,
                0b11111111
            };
    }
}
