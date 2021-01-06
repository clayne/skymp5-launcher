using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdatesClient.Core;
using UpdatesClient.Modules.Configs.Models;

namespace UpdatesClient.Modules.Configs
{
    internal static class NetworkSettings
    {
        private static NetworkSettingsModel model = new NetworkSettingsModel();
        public static bool Loaded { get; private set; } = false;

        public static bool ReportDmp { get => model.ReportDmp; }


        public static async void Init()
        {
            try
            {
                if (!Loaded)
                {
                    string jsn = await Net.Request(Net.URL_ApiLauncher + "GetParameters", "POST", false, null);
                    model = JsonConvert.DeserializeObject<NetworkSettingsModel>(jsn);
                    Loaded = true;
                }
            }
            catch (Exception e) 
            { 
                Logger.Error("NetSettigsInit", e);
            }
        }
    }
}
