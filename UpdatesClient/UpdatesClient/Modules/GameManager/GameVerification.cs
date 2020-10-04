using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UpdatesClient.Modules.GameManager.Enums;
using UpdatesClient.Modules.GameManager.Model;

namespace UpdatesClient.Modules.GameManager
{
    internal class GameVerification
    {
        internal static ResultGameVerification VerifyGame(string pathToGameFolder, string pathToVerificationFile)
        {
            ResultGameVerification result = new ResultGameVerification();

            if (File.Exists($"{pathToGameFolder}\\SkyrimSE.exe"))
            {
                result.IsGameFound = true;
                result.GameVersion = new Version(FileVersionInfo.GetVersionInfo($"{pathToGameFolder}\\SkyrimSE.exe").FileVersion);
                result.UnSafeGameFilesDictionary = VerifyGameFiles();
                result.IsGameSafe = result.UnSafeGameFilesDictionary.Count == 0;
            }

            if (File.Exists($"{pathToGameFolder}\\skse64_loader.exe"))
            {
                result.IsSKSEFound = true;
                result.SKSEVersion = new Version(FileVersionInfo.GetVersionInfo($"{pathToGameFolder}\\skse64_loader.exe").FileVersion.Replace(", ", "."));
                result.UnSafeSKSEFilesDictionary = VerifySKSEFiles();
                result.IsSKSESafe = result.UnSafeSKSEFilesDictionary.Count == 0;
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
