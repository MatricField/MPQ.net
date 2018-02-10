﻿//MIT License

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
using System.Runtime.InteropServices;


namespace MPQNet.Header
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class UserDataHeader
    {
        /// <summary>
        /// The ID_MPQ_USERDATA ('MPQ\x1B') signature
        /// </summary>
        public ArchiveHeaderSignatures ID { get; }

        /// <summary>
        /// Maximum size of the user data
        /// </summary>
        public uint UserDataSize { get; }

        /// <summary>
        /// Offset of the MPQ header, relative to the begin of this header
        /// </summary>
        public uint HeaderOffset { get; }

        /// <summary>
        /// Appears to be size of user data header (Starcraft II maps)
        /// </summary>
        public uint UserDataHeaderSize { get; }
    }
}
