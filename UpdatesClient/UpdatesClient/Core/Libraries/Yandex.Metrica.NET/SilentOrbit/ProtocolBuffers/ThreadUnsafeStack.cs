using System;
using System.Collections.Generic;
using System.IO;

namespace SilentOrbit.ProtocolBuffers
{
    public class ThreadUnsafeStack : IMemoryStreamStack, IDisposable
    {
        private readonly Stack<MemoryStream> stack = new Stack<MemoryStream>();

        public MemoryStream Pop()
        {
            return this.stack.Count == 0 ? new MemoryStream() : this.stack.Pop();
        }

        public void Push(MemoryStream stream)
        {
            this.stack.Push(stream);
        }

        public void Dispose()
        {
            this.stack.Clear();
        }
    }
}
