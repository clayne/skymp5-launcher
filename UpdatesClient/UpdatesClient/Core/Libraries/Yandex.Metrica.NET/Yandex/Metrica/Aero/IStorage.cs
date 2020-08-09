// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Aero.IStorage
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System.IO;

namespace Yandex.Metrica.Aero
{
  internal interface IStorage
  {
    Stream GetReadStream(string key);

    Stream GetWriteStream(string key);

    void DeleteKey(string key);

    bool HasKey(string key);

    long Length(string key);

    bool DirectoryExists(string path);

    void CreateDirectory(string path);
  }
}
