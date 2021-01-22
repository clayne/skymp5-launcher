using Newtonsoft.Json;
using System;
using UpdatesClient.Core;
using UpdatesClient.Modules.Configs.Models;

namespace UpdatesClient.Modules.Configs
{
    internal static class NetworkSettings
    {
        private static NetworkSettingsModel model = new NetworkSettingsModel();
        public static bool Loaded { get; private set; } = false;

        public static bool ReportDmp { get => model.ReportDmp; }
        public static bool ShowingServerStatus { get => model.ShowingServerStatus; }
        public static string ServerStatus { get => model.ServerStatus; }
        public static string OfficialServerAdress { get => model.OfficialServerAdress; }
        public static bool EnableAntiCheat { get => model.EnableAntiCheat; }
        public static bool EnableModLoader { get => model.EnableModLoader; }

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
