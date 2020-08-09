using System;
using System.IO;

namespace SilentOrbit.ProtocolBuffers
{
    public interface IMemoryStreamStack : IDisposable
    {
        MemoryStream Pop();

        void Push(MemoryStream stream);
    }
}
