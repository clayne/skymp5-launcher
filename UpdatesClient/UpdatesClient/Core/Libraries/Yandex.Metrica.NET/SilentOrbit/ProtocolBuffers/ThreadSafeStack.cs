using System;
using System.Collections.Generic;
using System.IO;

namespace SilentOrbit.ProtocolBuffers
{
    public class ThreadSafeStack : IMemoryStreamStack, IDisposable
    {
        private readonly Stack<MemoryStream> stack = new Stack<MemoryStream>();

        public MemoryStream Pop()
        {
            lock (this.stack)
                return this.stack.Count == 0 ? new MemoryStream() : this.stack.Pop();
        }

        public void Push(MemoryStream stream)
        {
            lock (this.stack)
                this.stack.Push(stream);
        }

        public void Dispose()
        {
            lock (this.stack)
                this.stack.Clear();
        }
    }
}
