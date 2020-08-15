using System;

namespace UpdatesClient.Modules.Configs.Models
{
    public class ModVersionModel
    {
        public string Version { get; set; } = "0.0.0.0";
        public bool? HasRuFixConsole { get; set; } = null;
        public DateTime LastDmpReported { get; set; }
    }
}
