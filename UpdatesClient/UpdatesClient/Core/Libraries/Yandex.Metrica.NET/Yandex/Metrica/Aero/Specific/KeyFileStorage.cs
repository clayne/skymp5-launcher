// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Aero.Specific.KeyFileStorage
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System.IO;

namespace Yandex.Metrica.Aero.Specific
{
  internal class KeyFileStorage : IStorage
  {
    public Stream GetReadStream(string key)
    {
      return (Stream) File.OpenRead(key);
    }

    public Stream GetWriteStream(string key)
    {
      return (Stream) File.Open(key, FileMode.Create);
    }

    public void DeleteKey(string key)
    {
      if (!File.Exists(key))
        return;
      File.Delete(key);
    }

    public bool HasKey(string key)
    {
      return File.Exists(key);
    }

    public long Length(string key)
    {
      return !File.Exists(key) ? 0L : File.OpenRead(key).Length;
    }

    public bool DirectoryExists(string path)
    {
      return string.IsNullOrWhiteSpace(path) || Directory.Exists(path);
    }

    public void CreateDirectory(string path)
    {
      if (string.IsNullOrWhiteSpace(path))
        return;
      Directory.CreateDirectory(path);
    }
  }
}
