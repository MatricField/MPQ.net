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

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MPQNet.Definition
{
    /// <summary>
    /// Common fields for all headers
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public abstract class HeaderCommon : IEquatable<HeaderCommon>
    {
        /// <summary>
        /// Indicates that the file is a MoPaQ archive.
        /// </summary>
        public Signatures ID { get; }

        #region Structural Equality
        public override bool Equals(object obj)
        {
            return Equals(obj as HeaderCommon);
        }

        public bool Equals(HeaderCommon other)
        {
            return other != null &&
                   ID == other.ID;
        }

        public override int GetHashCode()
        {
            return 1213502048 + ID.GetHashCode();
        }

        public static bool operator ==(HeaderCommon common1, HeaderCommon common2)
        {
            return EqualityComparer<HeaderCommon>.Default.Equals(common1, common2);
        }

        public static bool operator !=(HeaderCommon common1, HeaderCommon common2)
        {
            return !(common1 == common2);
        }
        #endregion
    }
}
