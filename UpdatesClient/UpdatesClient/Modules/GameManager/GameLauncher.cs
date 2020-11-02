using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UpdatesClient.Core;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.GameManager.Helpers;
using Yandex.Metrica;

namespace UpdatesClient.Modules.GameManager
{
    public class GameLauncher
    {
        public static bool Runing { get; private set; }

        private static Process GameProcess = new Process();
        private static readonly ProcessStartInfo StartInfo = new ProcessStartInfo();

        public static void StopGame()
        {
            GameProcess.Kill();
        }

        public static void EnableDebug()
        {
            string path = $"{Settings.PathToSkyrim}\\Data\\SKSE\\SKSE.ini";
            if (!File.Exists(path)) File.Create(path).Close();

            IniFile iniFile = new IniFile(path);
            iniFile.WriteINI("DEBUG", "WriteMiniDumps", "1");
        }

        public static async Task<bool> StartGame()
        {
            EnableDebug();

            StartInfo.FileName = $"{Settings.PathToSkyrim}\\skse64_loader.exe";
            //StartInfo.Arguments = $"--UUID {UUID} --Session {session}";
            StartInfo.WorkingDirectory = $"{Settings.PathToSkyrim}\\";
            StartInfo.Verb = "runas";

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
            Microsoft.Win32.SafeHandles.SafeProcessHandle sh = GameProcess.SafeHandle;
            if (!GameProcess.HasExited) await Task.Run(() => GameProcess.WaitForExit());

            YandexMetrica.ReportEvent("ExitedGame");
            Runing = false;

            return GameProcess.ExitCode != 0;
        }

    }
}
