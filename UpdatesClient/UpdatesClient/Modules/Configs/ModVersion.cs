using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UpdatesClient.Modules.Configs.Models;
using Yandex.Metrica;

namespace UpdatesClient.Modules.Configs
{
    internal class ModVersion
    {
        public static string Version { get; set; } = "0.0.0.0";


        internal static bool Load()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.PathToSkyrim)) return false;

                string path = $"{Settings.PathToSkyrim}\\version.json";
                if (File.Exists(path))
                {
                    ModVersionModel model = JsonConvert.DeserializeObject<ModVersionModel>(File.ReadAllText(path));
                    Version = Security.FromAes256Base64(model.Version);
                    return true;
                }
            }
            catch (Exception e)
            {
                YandexMetrica.ReportError("Version_Load", e);
            }

            return false;

        }
        internal static void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.PathToSkyrim)) return;
                string path = $"{Settings.PathToSkyrim}\\version.json";

                ModVersionModel model = new ModVersionModel()
                {
                    Version = Security.ToAes256Base64(Version)
                };

                File.WriteAllText(path, JsonConvert.SerializeObject(model));

            }
            catch (Exception e)
            {
                YandexMetrica.ReportError("Version_Save", e);
            }
        }

        internal static void Reset()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.PathToSkyrim)) return;
                string path = $"{Settings.PathToSkyrim}\\version.json";
                if (File.Exists(path)) File.Delete(path);
            }
            catch (Exception e)
            {
                YandexMetrica.ReportError("Version_Reset", e);
            }
        }
    }
}
