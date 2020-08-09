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
