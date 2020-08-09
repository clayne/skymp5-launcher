// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Legacy.IStreamSerializer`1
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System.IO;

namespace Yandex.Metrica.Legacy
{
  internal interface IStreamSerializer<T>
  {
    void Serialize(Stream stream, T obj);

    T Deserialize(Stream stream);
  }
}
