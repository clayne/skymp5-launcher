﻿using System.Diagnostics;
using System.IO;
using UpdatesClient.Modules.Configs;

using Res = UpdatesClient.Properties.Resources;

namespace UpdatesClient.Core
{
    public static class LocalesManager
    {
        private const string LastLocaleVersion = "2.0.5.2";
        private static readonly string[] locales = { "ru-RU" };

        public static string GetPathToLocaleLib(string locale)
        {
            return $"{DefaultPaths.PathToLocal}{locale}\\UpdatesClient.resources.dll";
        }

        public static bool ExistLocale(string locale)
        {
            return Directory.Exists($"{DefaultPaths.PathToLocal}{locale}\\")
                && File.Exists(GetPathToLocaleLib(locale));
        }

        public static void CheckResxLocales()
        {
            foreach (string locale in locales)
            {
                string path = $"{DefaultPaths.PathToLocal}\\{locale}";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string pathToFile = $"{path}\\UpdatesClient.resources.dll";

                string vers = FileVersionInfo.GetVersionInfo(pathToFile)?.FileVersion;
                if (LastLocaleVersion != vers) UnpackResxLocale(locale, pathToFile);
            }
        }

        private static void UnpackResxLocale(string locale, string destPath)
        {
            try
            {
                byte[] bytes = (byte[])Res.ResourceManager.GetObject($"UpdatesClient_{locale}_resources");

                IO.FileSetNormalAttribute(destPath);

                File.WriteAllBytes(destPath, bytes);
            }
            catch { }
        }
    }
}
