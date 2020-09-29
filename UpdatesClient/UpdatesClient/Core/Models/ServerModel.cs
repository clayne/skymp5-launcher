using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UpdatesClient.Core.Models
{
    public struct ServerModel
    {
        [JsonProperty("ip")]
        public string IP { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("maxPlayers")]
        public int MaxPlayers { get; set; }

        [JsonProperty("online")]
        public int Online { get; set; }

        public int ID => (IP + Port.ToString()).GetHashCode();

        public override string ToString()
        {
            return Name + " (" + Online + " / " + MaxPlayers + ")";
        }

        public bool IsSameServer(SkympClientSettingsModel settings)
        {
            return (settings.IP + settings.Port.ToString()).GetHashCode() == ID;
        }

        public SkympClientSettingsModel ToSkympClientSettings(SkympClientSettingsModel oldServer)
        {
            return new SkympClientSettingsModel
            {
                IP = this.IP,
                IsEnableConsole = oldServer.IsEnableConsole,
                IsShowMe = oldServer.IsShowMe,
                Port = this.Port
            };
        }

        public static async Task<List<ServerModel>> GetServerList()
        {
            return JArray.Parse(await Net.Request(Net.URL_SERVERS, null)).ToObject<List<ServerModel>>();
        }
    }
}
