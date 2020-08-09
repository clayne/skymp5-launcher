// Decompiled with JetBrains decompiler
// Type: SilentOrbit.ProtocolBuffers.ThreadSafeStack
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

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
