using Newtonsoft.Json;
using System;
using System.IO;
using UpdatesClient.Core;
using UpdatesClient.Modules.Configs.Models;
using Yandex.Metrica;

namespace UpdatesClient.Modules.Configs
{
    internal class ModVersion
    {
        private static ModVersionModel model;

        public static string Version
        {
            get { return Security.FromAes256Base64(model.Version); }
            set { model.Version = Security.ToAes256Base64(value); }
        }
        public static bool? HasRuFixConsole
        {
            get { return model.HasRuFixConsole; }
            set { model.HasRuFixConsole = value; }
        }
        public static DateTime LastDmpReported
        {
            get { return model.LastDmpReported; }
            set { model.LastDmpReported = value; }
        }

        internal static bool Load()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.PathToSkyrim)) return false;
                string path = $"{Settings.PathToSkyrim}\\version.json";
                if (File.Exists(path))
                {
                    model = JsonConvert.DeserializeObject<ModVersionModel>(File.ReadAllText(path));
                    return true;
                }
                else
                {
                    model = new ModVersionModel();
                }
            }
            catch (Exception e)
            {
                YandexMetrica.ReportError("Version_Load", e);
                Logger.Error(e);
            }
            return false;
        }
        internal static void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.PathToSkyrim)) return;
                string path = $"{Settings.PathToSkyrim}\\version.json";
                File.WriteAllText(path, JsonConvert.SerializeObject(model));
            }
            catch (Exception e)
            {
                YandexMetrica.ReportError("Version_Save", e);
                Logger.Error(e);
            }
        }
        internal static void Reset()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.PathToSkyrim)) return;
                string path = $"{Settings.PathToSkyrim}\\version.json";
                if (File.Exists(path)) File.Delete(path);
                model = new ModVersionModel();
            }
            catch (Exception e)
            {
                YandexMetrica.ReportError("Version_Reset", e);
                Logger.Error(e);
            }
        }
    }
}
