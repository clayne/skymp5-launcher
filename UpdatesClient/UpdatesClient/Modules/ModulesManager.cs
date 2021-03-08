using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UpdatesClient.Core;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.Debugger;

namespace UpdatesClient.Modules
{
    public static class ModulesManager
    {
        public static void PreInitModules()
        {
            Logger.Init(new Version(EnvParams.VersionFile));
            Settings.Load();
            NetworkSettings.Init();
        }

        public static void InitModules()
        {

        }

        public static void PostInitModules()
        {

        }
    }
}
