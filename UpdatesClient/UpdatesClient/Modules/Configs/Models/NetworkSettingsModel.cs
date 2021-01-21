using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatesClient.Modules.Configs.Models
{
    public class NetworkSettingsModel
    {
        public bool ReportDmp { get; set; } = false;
        public bool ShowingServerStatus { get; set; } = false;
        public string ServerStatus { get; set; } = "ОФИЦИАЛЬНЫЙ СЕРВЕР СЕЙЧАС НЕДОСТУПЕН.";
        public string OfficialServerAdress { get; set; } = "35.180.83.817777";
        public bool EnableAntiCheat { get; set; } = false;
    }
}
