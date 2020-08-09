// Decompiled with JetBrains decompiler
// Type: SilentOrbit.ProtocolBuffers.KeyValue
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

namespace SilentOrbit.ProtocolBuffers
{
  public class KeyValue
  {
    public Key Key { get; set; }

    public byte[] Value { get; set; }

    public KeyValue(Key key, byte[] value)
    {
      this.Key = key;
      this.Value = value;
    }

    public override string ToString()
    {
      return string.Format("[KeyValue: {0}, {1}, {2} bytes]", (object) this.Key.Field, (object) this.Key.WireType, (object) this.Value.Length);
    }
  }
}
