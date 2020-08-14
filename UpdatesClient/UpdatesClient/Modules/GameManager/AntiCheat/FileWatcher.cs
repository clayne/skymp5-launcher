using System;
using System.IO;
using System.Windows;
using UpdatesClient.Modules.Configs;
using Yandex.Metrica;

namespace UpdatesClient.Modules.GameManager.AntiCheat
{
    internal class FileWatcher
    {
        private const bool EnableAntiCheat = false;

        //FileSystemWatcher
        internal static void Init()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.PathToSkyrim)) return;
                FileSystemWatcher watcher = new FileSystemWatcher(Settings.PathToSkyrim + "\\Data\\Platform\\Plugins");
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
                watcher.Created += Watcher_Created;
                watcher.Changed += Watcher_Changed;
                watcher.Deleted += Watcher_Deleted;
                watcher.Renamed += Watcher_Renamed;
                watcher.Error += Watcher_Error;
            }
            catch (Exception e)
            {
                YandexMetrica.ReportError("AntiCheat_Init", e);
            }
        }

        private static void Watcher_Error(object sender, ErrorEventArgs e)
        {
            YandexMetrica.ReportError("WatcherError", e?.GetException());
        }

        private static void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            YandexMetrica.ReportEvent($"WatcherRenamed_{e?.Name}");
        }

        private static void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            YandexMetrica.ReportEvent($"WatcherDeleted_{e?.Name}");
        }

        private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            YandexMetrica.ReportEvent($"WatcherChanged_{e?.Name}");
            AntiCheatAlert(e?.FullPath);
        }

        private static void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            YandexMetrica.ReportEvent($"WatcherCreated_{e?.Name}");
        }

        private static void AntiCheatAlert(string file)
        {
            if (EnableAntiCheat)
            {
                //Провести валидацию изменений

                MessageBox.Show($"Файл {file} был изменен", "Внимание");
                GameLauncher.StopGame();
            }
        }
    }
}
