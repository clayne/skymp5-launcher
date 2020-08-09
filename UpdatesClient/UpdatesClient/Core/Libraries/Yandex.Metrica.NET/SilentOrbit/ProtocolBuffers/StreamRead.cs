using System;
using System.IO;

namespace SilentOrbit.ProtocolBuffers
{
    [Obsolete("Renamed to PositionStream")]
    public class StreamRead : PositionStream
    {
        public StreamRead(Stream baseStream)
          : base(baseStream)
        {
        }
    }
}
