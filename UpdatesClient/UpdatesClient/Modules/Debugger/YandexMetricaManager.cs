using System;
using System.IO;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.Configs.Helpers;
using Yandex.Metrica;

namespace UpdatesClient.Modules.Debugger
{
    public static class YandexMetricaManager
    {
        private const string key = "3cb6204a-2b9c-4a7c-9ea5-f177e78a4657";

        public static void Init(Version version)
        {
            string tmpPath = DefaultPaths.PathToLocalTmp;
            if (!Directory.Exists(tmpPath)) Directory.CreateDirectory(tmpPath);
            YandexMetricaFolder.SetCurrent(tmpPath);
            ExperimentalFunctions.IfUse("SetVers", () =>
            {
                Version nv = new Version(version.Major, version.Minor, version.Build, version.Revision+100);
                YandexMetrica.Config.CustomAppVersion = nv;
            }, () =>
            {
                YandexMetrica.Config.CustomAppVersion = version;
            });
        }

        public static void Activate()
        {
            YandexMetrica.Activate(key);
        }

        public static void SetCrashTracking(bool status)
        {
            YandexMetrica.Config.CrashTracking = status;
        }

        public static void ReportEvent(string text)
        {
            YandexMetrica.ReportEvent(text);
        }
    }
}