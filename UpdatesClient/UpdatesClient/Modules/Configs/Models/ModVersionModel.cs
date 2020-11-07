using Security.Extensions;
using System;

namespace UpdatesClient.Modules.Configs.Models
{
    public class ModVersionModel
    {
        public SecureString Version { get; set; }
        public bool? HasRuFixConsole { get; set; } = null;
        public DateTime LastDmpReported { get; set; }
    }
}
