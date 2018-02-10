//MIT License

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

namespace MPQNet.Archive
{
    /// <summary>
    /// Header signatures of MPQ headers
    /// </summary>
    public enum HeaderSignatures : uint
    {
        /// <summary>
        /// MPQ archive header ID ('MPQ\x1A')
        /// </summary>
        MPQ = 0x1A51504D,

        /// <summary>
        /// MPQ userdata entry ('MPQ\x1B')
        /// </summary>
        MPQ_UserData = 0x1B51504D,

        /// <summary>
        /// MPK archive header ID ('MPK\x1A')
        /// </summary>
        MPK = 0x1A4B504D,
    }
}
