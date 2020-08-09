// Decompiled with JetBrains decompiler
// Type: SilentOrbit.ProtocolBuffers.Key
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

namespace SilentOrbit.ProtocolBuffers
{
  public class Key
  {
    public uint Field { get; set; }

    public Wire WireType { get; set; }

    public Key(uint field, Wire wireType)
    {
      this.Field = field;
      this.WireType = wireType;
    }

    public override string ToString()
    {
      return string.Format("[Key: {0}, {1}]", (object) this.Field, (object) this.WireType);
    }
  }
}
