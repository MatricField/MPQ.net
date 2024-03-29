﻿//Copyright 2021 Connor Haigh

//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
using System;
using System.IO;

namespace SubstreamSharp
{
    /// <summary>
    /// Represents a substream of an underlying <see cref="Stream" />.
    /// </summary>
    public class Substream : Stream
    {
        /// <summary>
        /// Creates a new substream instance using the specified underlying stream at the specified offset with the specified length.
        /// </summary>
        /// <param name="stream">The underlying stream.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        public Substream(Stream stream, long offset, long length, bool ownUnderlyingStream = false)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            // Streams must support seeking for the concept of substreams to work.
            // At a pinch in the future we may support a poor man's seek (forward) by reading until the position is correct.

            if (!stream.CanSeek)
            {
                throw new NotSupportedException("Stream does not support seeking.");
            }

            this.stream = stream;

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be less than zero.");
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be less than zero.");
            }

            this.offset = offset;
            this.length = length;
            OwnUnderlyingStream = ownUnderlyingStream;
        }

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!this.stream.CanRead)
            {
                throw new NotSupportedException("Underlying stream does not support reading.");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be less than zero.");
            }

            this.stream.Seek(this.offset + this.position, SeekOrigin.Begin);
            this.stream.Read(buffer, offset, count = Convert.ToInt32(Math.Min(count, this.length - this.position)));

            this.position += count;

            return count;
        }

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!this.stream.CanWrite)
            {
                throw new NotSupportedException("Underlying stream does not support writing.");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be less than zero.");
            }

            this.stream.Seek(this.offset + this.position, SeekOrigin.Begin);
            this.stream.Write(buffer, offset, count = Convert.ToInt32(Math.Min(count, this.length - this.position)));

            this.position += count;
        }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    if (offset < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be less than zero when seeking from the beginning.");
                    }

                    if (offset > this.length)
                    {
                        throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be greater than the length of the substream.");
                    }

                    this.stream.Seek(this.offset + (this.position = offset), SeekOrigin.Begin);

                    break;
                case SeekOrigin.End:
                    if (offset > 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be greater than zero when seeking from the end.");
                    }

                    if (offset < -this.length)
                    {
                        throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be less than the length of the substream.");
                    }

                    this.stream.Seek(this.position = (this.length + offset), SeekOrigin.End);

                    break;
                case SeekOrigin.Current:
                    if (this.position + offset < 0)
                    {
                        throw new NotSupportedException("Attempted to seek before the start of the substream.");
                    }

                    if (this.position + offset > this.length)
                    {
                        throw new NotSupportedException("Attempted to seek beyond the end of the substream.");
                    }

                    this.stream.Seek(this.position += offset, SeekOrigin.Current);

                    break;
            }

            return this.position;
        }

        /// <inheritdoc />
        public override void SetLength(long value)
        {
            // While other Stream implementations allow the caller to set the length, this does not make much sense in the context of a substream.
            // Perhaps, in the future, we can allow callers to reduce the length, but not expand the length.

            throw new NotSupportedException("Cannot set the length of a fixed substream.");
        }

        /// <inheritdoc />
        public override void Flush() => this.stream.Flush();

        /// <inheritdoc />
        public override long Length => this.length;

        /// <inheritdoc />
        public override bool CanRead => this.stream.CanRead;

        /// <inheritdoc />
        public override bool CanSeek => this.stream.CanSeek;

        /// <inheritdoc />
        public override bool CanWrite => this.stream.CanWrite;

        /// <inheritdoc />
        public override bool CanTimeout => this.stream.CanTimeout;

        /// <inheritdoc />
        public override int ReadTimeout
        {
            get => this.stream.ReadTimeout;
            set => throw new NotSupportedException("Cannot set the read timeout of a substream.");
        }

        /// <inheritdoc />
        public override int WriteTimeout
        {
            get => this.stream.WriteTimeout;
            set => throw new NotSupportedException("Cannot set the write timeout of a substream.");
        }

        /// <inheritdoc />
        public override long Position
        {
            get => this.position;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Position cannot be less than zero.");
                }

                if (value > this.length)
                {
                    throw new ArgumentOutOfRangeException("Position cannot be greater than the length.");
                }

                this.stream.Position = this.offset + (this.position = value);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing && OwnUnderlyingStream)
            {
                stream.Dispose();
            }
        }

        protected readonly Stream stream = null;

        protected readonly long offset = 0L;
        protected readonly long length = 0L;

        protected long position = 0L;
        protected bool OwnUnderlyingStream;
    }
}