using Newtonsoft.Json;
using System.Collections.Generic;

namespace UpdatesClient.Core.Models.ServerManifest
{
    public class ModsManifest
    {
        [JsonProperty("mods")]
        public List<ModModel> Mods { get; set; } = new List<ModModel>();

        [JsonProperty("versionMajor")]
        public int VersionMajor { get; set; } = -1;

        [JsonProperty("loadOrder")]
        public List<string> LoadOrder { get; set; } = new List<string>();
    }
}
