// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Legacy.IsolatedStreamStorage`1
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;

namespace Yandex.Metrica.Legacy
{
  internal class IsolatedStreamStorage<T>
  {
    private readonly IStreamSerializer<T> _serializer;

    public IsolatedStreamStorage(IStreamSerializer<T> sessionsProtoSerializer)
    {
      this._serializer = sessionsProtoSerializer;
    }

    public Task SaveAsync(string resourceLocation, T obj)
    {
      using (IsolatedStorageFileStream storageFileStream = IsolatedStreamStorage<T>.GetStore().OpenFile(resourceLocation, FileMode.Create))
        this._serializer.Serialize((Stream) storageFileStream, obj);
      return (Task) Task.FromResult<object>(new object());
    }

    public Task<T> ReadAsync(string resourceLocation)
    {
      try
      {
        IsolatedStorageFile store = IsolatedStreamStorage<T>.GetStore();
        if (store.FileExists(resourceLocation))
        {
          using (IsolatedStorageFileStream storageFileStream = store.OpenFile(resourceLocation, FileMode.Open))
            return Task.FromResult<T>(this._serializer.Deserialize((Stream) storageFileStream));
        }
      }
      catch
      {
      }
      return Task.FromResult<T>(default (T));
    }

    public Task DeleteAsync(string resourceLocation)
    {
      try
      {
        IsolatedStorageFile store = IsolatedStreamStorage<T>.GetStore();
        if (store.FileExists(resourceLocation))
          store.DeleteFile(resourceLocation);
      }
      catch
      {
      }
      return (Task) Task.FromResult<object>(new object());
    }

    private static IsolatedStorageFile GetStore()
    {
      return IsolatedStorageFile.GetUserStoreForDomain();
    }
  }
}
