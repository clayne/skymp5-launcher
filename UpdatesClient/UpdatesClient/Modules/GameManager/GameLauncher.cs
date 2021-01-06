using System;
using System.ComponentModel;
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
            
            await Task.Delay(500);

            Process[] SkyrimPlatformCEFs = Process.GetProcessesByName("SkyrimPlatformCEF");
            for (int i = 0; i < SkyrimPlatformCEFs.Length; i++)
            {
                try
                {
                    int tr = 0;
                    do
                    {
                        SkyrimPlatformCEFs[i].Kill();
                        await Task.Delay(200);
                    }
                    while (!SkyrimPlatformCEFs[i].HasExited && tr++ < 5);
                }
                catch (Win32Exception) 
                {
                    if(Settings.ExperimentalFunctions)
                    {
                        try
                        {
                            if (!SkyrimPlatformCEFs[i].HasExited)
                                ProcessKiller.KillProcess((IntPtr)SkyrimPlatformCEFs[i].Id);
                        }
                        catch (Exception e)
                        {
                            Logger.Error("StartGame_Killer_SkyrimPlatformCEF", e);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("StartGame_KillSkyrimPlatformCEF", e);
                }
            }

            Runing = false;

            return GameProcess.ExitCode != 0;
        }

    }
}
