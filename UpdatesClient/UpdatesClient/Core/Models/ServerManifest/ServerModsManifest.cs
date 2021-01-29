using Newtonsoft.Json;
using System.Collections.Generic;

namespace UpdatesClient.Core.Models.ServerManifest
{
    public class ServerModsManifest
    {
        [JsonProperty("mods")]
        public List<ServerModModel> Mods { get; set; } = new List<ServerModModel>();

        [JsonProperty("versionMajor")]
        public int VersionMajor { get; set; } = -1;

        [JsonProperty("loadOrder")]
        public List<string> LoadOrder { get; set; } = new List<string>();
    }
}
