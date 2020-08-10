using System.IO;

namespace Yandex.Metrica.Legacy
{
    internal interface IStreamSerializer<T>
    {
        void Serialize(Stream stream, T obj);

        T Deserialize(Stream stream);
    }
}
