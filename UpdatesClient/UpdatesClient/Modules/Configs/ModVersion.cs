using Newtonsoft.Json;
using System;
using System.IO;
using UpdatesClient.Core;
using UpdatesClient.Modules.Configs.Models;

namespace UpdatesClient.Modules.Configs
{
    internal class ModVersion
    {
        private static readonly object sync = new object();
        private static ModVersionModel model;

        public static string Version
        {
            get { return model.Version; }
            set { model.Version = value; }
        }
        public static bool? HasRuFixConsole
        {
            get { return model.HasRuFixConsole; }
            set { model.HasRuFixConsole = value; }
        }
        public static DateTime LastDmpReported
        {
            get { return model.LastDmpReported ?? default; }
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
                    lock (sync)
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
                Logger.Error("Version_Load", e);
            }
            return false;
        }
        internal static void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.PathToSkyrim)) return;
                string path = $"{Settings.PathToSkyrim}\\version.json";
                lock (sync)
                    File.WriteAllText(path, JsonConvert.SerializeObject(model));
            }
            catch (Exception e)
            {
                NotifyController.Show(UI.Controllers.PopupNotify.Error, "Ошибка", "Не удалось сохранить сведения, файл занят");
                Logger.Error("Version_Save", e);
            }
        }
        internal static void Reset()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.PathToSkyrim)) return;
                string path = $"{Settings.PathToSkyrim}\\version.json";
                lock (sync)
                    if (File.Exists(path)) File.Delete(path);
                model = new ModVersionModel();
            }
            catch (Exception e)
            {
                Logger.Error("Version_Reset", e);
            }
        }
    }
}
