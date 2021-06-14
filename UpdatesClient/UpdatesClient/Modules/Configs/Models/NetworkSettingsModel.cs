namespace UpdatesClient.Modules.Configs.Models
{
    public class NetworkSettingsModel
    {
        public bool ReportDmp { get; set; } = false;
        public string OfficialServerAdress { get; set; } = "35.180.83.81:7777";
        public bool EnableAntiCheat { get; set; } = false;
        public bool CompatibilityMode { get; set; } = false;

        public bool ProblemShow { get; set; }
        public string ProblemText { get; set; }

        public string Banners { get; set; } = "";
    }
}
