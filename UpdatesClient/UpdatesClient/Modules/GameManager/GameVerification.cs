using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UpdatesClient.Core;
using UpdatesClient.Modules.GameManager.Enums;
using UpdatesClient.Modules.GameManager.Model;
using Res = UpdatesClient.Properties.Resources;

namespace UpdatesClient.Modules.GameManager
{
    internal class GameVerification
    {
        public static string GetGameFolder()
        {
            try
            {
                using (System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    dialog.Description = Res.SelectGameFolder;
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        return dialog.SelectedPath;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("GetGameFolder", e);
            }
            return null;
        }

        internal static ResultGameVerification VerifyGame(string pathToGameFolder, string pathToVerificationFile)
        {
            ResultGameVerification result = new ResultGameVerification();
            if (File.Exists($"{pathToGameFolder}\\SkyrimSE.exe"))
            {
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo($"{pathToGameFolder}\\SkyrimSE.exe");
                if (fileVersionInfo != null)
                {
                    result.IsGameFound = true;
                    string ver = "null";

                    try
                    {
                        ver = fileVersionInfo.FileVersion;
                        result.GameVersion = new Version(ver);
                    }
                    catch { Logger.Error("VersionRead", new Exception($"Raw vesrion {ver}")); }
                    result.UnSafeGameFilesDictionary = VerifyGameFiles();
                    result.IsGameSafe = result.UnSafeGameFilesDictionary.Count == 0;
                }
            }
            else return result;

            if (File.Exists($"{pathToGameFolder}\\skse64_loader.exe"))
            {
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo($"{pathToGameFolder}\\skse64_loader.exe");
                if (fileVersionInfo != null)
                {
                    result.IsSKSEFound = true;
                    string ver = "null";
                    try
                    {
                        ver = fileVersionInfo.FileVersion.Replace(", ", ".");
                        result.SKSEVersion = new Version(ver);
                    }
                    catch { Logger.Error("VersionReadSKSE", new Exception($"Raw vesrion {ver}")); }
                    result.UnSafeSKSEFilesDictionary = VerifySKSEFiles();
                    result.IsSKSESafe = result.UnSafeSKSEFilesDictionary.Count == 0;
                }
            }

            if (Directory.Exists($"{pathToGameFolder}\\Data\\Interface"))
            {
                result.IsRuFixConsoleFound = true;
            }

            result.IsModFound = FindMod();

            return result;
        }

        private static Dictionary<string, FileState> VerifyGameFiles()
        {
            return new Dictionary<string, FileState>();
        }
        private static Dictionary<string, FileState> VerifySKSEFiles()
        {
            return new Dictionary<string, FileState>();
        }

        private static bool FindMod()
        {
            return false;
        }
    }
}
