using System;
using System.IO;

namespace SilentOrbit.ProtocolBuffers
{
    public class PositionStream : Stream
    {
        private readonly Stream stream;

        public int BytesRead { get; private set; }

        public PositionStream(Stream baseStream)
        {
            this.stream = baseStream;
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int num = this.stream.Read(buffer, offset, count);
            this.BytesRead += num;
            return num;
        }

        public override int ReadByte()
        {
            int num = this.stream.ReadByte();
            ++this.BytesRead;
            return num;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (this.stream.CanSeek)
                return this.stream.Seek(offset, origin);
            if (origin != SeekOrigin.Current || offset < 0L)
                throw new NotImplementedException();
            byte[] buffer = new byte[Math.Min(offset, 10000L)];
            int num;
            for (long index = (long)this.BytesRead + offset; (long)this.BytesRead < index; this.BytesRead += num)
            {
                num = this.stream.Read(buffer, 0, (int)Math.Min(index - (long)this.BytesRead, (long)buffer.Length));
                if (num == 0)
                    break;
            }
            return (long)this.BytesRead;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override long Length
        {
            get
            {
                return this.stream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return (long)this.BytesRead;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.stream.Dispose();
            base.Dispose(disposing);
        }
    }
}
