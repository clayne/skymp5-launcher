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
                this._serializer.Serialize((Stream)storageFileStream, obj);
            return (Task)Task.FromResult<object>(new object());
        }

        public Task<T> ReadAsync(string resourceLocation)
        {
            try
            {
                IsolatedStorageFile store = IsolatedStreamStorage<T>.GetStore();
                if (store.FileExists(resourceLocation))
                {
                    using (IsolatedStorageFileStream storageFileStream = store.OpenFile(resourceLocation, FileMode.Open))
                        return Task.FromResult<T>(this._serializer.Deserialize((Stream)storageFileStream));
                }
            }
            catch
            {
            }
            return Task.FromResult<T>(default);
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
            return (Task)Task.FromResult<object>(new object());
        }

        private static IsolatedStorageFile GetStore()
        {
            return IsolatedStorageFile.GetUserStoreForDomain();
        }
    }
}
