using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.GameManager.Helpers;
using Yandex.Metrica;

namespace UpdatesClient.Modules.GameManager
{
    public class GameLauncher
    {
        public static bool Runing { get; private set; }

        private static Process GameProcess = new Process();
        private static ProcessStartInfo StartInfo = new ProcessStartInfo();

        public static void StopGame()
        {
            GameProcess.Kill();
        }

        public static async Task StartGame()
        {
            StartInfo.FileName = $"{Settings.PathToSkyrim}\\skse64_loader.exe";
            //StartInfo.Arguments = $"--UUID {UUID} --Session {session}";
            StartInfo.WorkingDirectory = $"{Settings.PathToSkyrim}\\";
            StartInfo.Verb = "runas";
            //StartInfo.

            StartInfo.Domain = AppDomain.CurrentDomain.FriendlyName;

            GameProcess.StartInfo = StartInfo;

            Runing = true;
            GameProcess.Start();
            YandexMetrica.ReportEvent("StartedGame");

            int ParentPID = GameProcess.Id;

            await Task.Run(() => GameProcess.WaitForExit());

            foreach (var p in ProcessExtensions.FindChildrenProcesses(ParentPID))
            {
                GameProcess = p;
            }
            if (!GameProcess.HasExited) await Task.Run(() => GameProcess.WaitForExit());

            YandexMetrica.ReportEvent("ExitedGame");
            Runing = false;
        }
    }
}
