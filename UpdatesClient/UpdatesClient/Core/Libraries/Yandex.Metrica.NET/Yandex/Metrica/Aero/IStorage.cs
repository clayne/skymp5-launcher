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
