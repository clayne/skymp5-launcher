// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Legacy.ConfigProtoSerializer
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System.IO;

namespace Yandex.Metrica.Legacy
{
  internal class ConfigProtoSerializer : IStreamSerializer<Config>
  {
    public void Serialize(Stream stream, Config config)
    {
      Config.Serialize(stream, config);
    }

    public Config Deserialize(Stream stream)
    {
      return Config.Deserialize(stream);
    }
  }
}
