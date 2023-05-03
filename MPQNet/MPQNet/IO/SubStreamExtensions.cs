//Copyright 2021 Connor Haigh

//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
using System.IO;

namespace SubstreamSharp
{
    /// <summary>
    /// Provides <see cref="Stream"/> extension methods.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Returns a substream instance sourcing from this stream at the specified offset with the specified length.
        /// </summary>
        /// <param name="stream">The sourcing stream.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns>A substream.</returns>
        public static Substream Substream(this Stream stream, long offset, long length, bool ownUnderlyingStream = false) => new Substream(stream, offset, length, ownUnderlyingStream: ownUnderlyingStream);
    }
}