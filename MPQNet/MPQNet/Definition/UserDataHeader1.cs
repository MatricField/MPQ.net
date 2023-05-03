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
using System.Diagnostics.CodeAnalysis;

namespace MPQNet.Definition
{
    internal record class UserDataHeader1 : UserDataHeader
    {
        protected uint _UserDataSize;
        protected uint _HeaderOffset;
        protected uint _UserDataHeaderSize;

        /// <summary>
        /// Maximum size of the user data
        /// </summary>
        public virtual required uint UserDataSize { get => _UserDataSize; init => _UserDataSize = value; }

        /// <summary>
        /// Offset of the MPQ header, relative to the begin of this header
        /// </summary>
        public virtual required uint HeaderOffset { get => _HeaderOffset; init => _HeaderOffset = value; }

        /// <summary>
        /// Appears to be size of user data header (Starcraft II maps)
        /// </summary>
        public virtual required uint UserDataHeaderSize { get => _UserDataHeaderSize; init => _UserDataHeaderSize = value; }

        public UserDataHeader1() :
            base()
        {

        }

        [SetsRequiredMembers]
        public UserDataHeader1(in RawUserData raw, long baseAddress) :
            base(baseAddress)
        {
            _UserDataSize = raw.cbUserDataSize;
            _HeaderOffset = raw.dwHeaderOffs;
            _UserDataHeaderSize = raw.cbUserDataHeader;
        }
    }
}
