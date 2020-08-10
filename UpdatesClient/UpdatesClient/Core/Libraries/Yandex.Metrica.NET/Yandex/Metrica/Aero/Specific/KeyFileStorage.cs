using System.IO;

namespace Yandex.Metrica.Aero.Specific
{
    internal class KeyFileStorage : IStorage
    {
        public Stream GetReadStream(string key)
        {
            return (Stream)File.OpenRead(key);
        }

        public Stream GetWriteStream(string key)
        {
            return (Stream)File.Open(key, FileMode.Create);
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
