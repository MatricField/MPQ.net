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
    /// Common header for HET and BET tables
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class ExtTableHeaderCommon : HeaderCommon, IEquatable<ExtTableHeaderCommon>
    {
        /// <summary>
        /// Version. Seems to be always 1
        /// </summary>
        public uint Version { get; }

        /// <summary>
        /// Size of the contained table
        /// </summary>
        public uint DataSize { get; }

        #region Structural Equality
        public override bool Equals(object obj)
        {
            return Equals(obj as ExtTableHeaderCommon);
        }

        public bool Equals(ExtTableHeaderCommon other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Version == other.Version &&
                   DataSize == other.DataSize;
        }

        public override int GetHashCode()
        {
            var hashCode = -184249525;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Version.GetHashCode();
            hashCode = hashCode * -1521134295 + DataSize.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ExtTableHeaderCommon common1, ExtTableHeaderCommon common2)
        {
            return EqualityComparer<ExtTableHeaderCommon>.Default.Equals(common1, common2);
        }

        public static bool operator !=(ExtTableHeaderCommon common1, ExtTableHeaderCommon common2)
        {
            return !(common1 == common2);
        } 
        #endregion
    }
}
