using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UpdatesClient.Core;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.Debugger;
using UpdatesClient.Modules.Downloader;
using UpdatesClient.Modules.Downloader.UI;

namespace UpdatesClient.Modules
{
    public static class ModulesManager
    {
        public static void PreInitModules()
        {
            Settings.Load();
            Logger.Init(new Version(EnvParams.VersionFile));
            NetworkSettings.Init();
        }

        public static void InitModules()
        {

        }

        public static void PostInitModules(ProgressBar progressBar)
        {
            DownloadManager.PostInit(progressBar);
        }
    }
}
