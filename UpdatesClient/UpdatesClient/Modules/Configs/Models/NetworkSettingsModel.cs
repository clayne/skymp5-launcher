﻿namespace UpdatesClient.Modules.Configs.Models
{
    public class NetworkSettingsModel
    {
        public bool ReportDmp { get; set; } = false;
        public bool ShowingServerStatus { get; set; } = false;
        public string ServerStatus { get; set; } = "ОФИЦИАЛЬНЫЙ СЕРВЕР СЕЙЧАС НЕДОСТУПЕН.";
        public string OfficialServerAdress { get; set; } = "35.180.83.817777";
        public bool EnableAntiCheat { get; set; } = false;
        public bool EnableModLoader { get; set; } = true;
        public bool CompatibilityMode { get; set; } = false;
    }
}
