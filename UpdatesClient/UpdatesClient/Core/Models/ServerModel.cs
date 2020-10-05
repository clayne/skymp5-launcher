using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UpdatesClient.Modules.Configs;

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

        public static int NullID => new ServerModel().ID;

        public bool IsEmpty()
        {
            return ID == NullID;
        }

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

        public static List<ServerModel> ParseServersToList(string jArrayServerList)
        {
            return JArray.Parse(jArrayServerList).ToObject<List<ServerModel>>();
        }

        public static async Task<string> GetServers()
        {
            return await Net.Request(Net.URL_SERVERS, null);
        }

        public static string Load()
        {
            if (File.Exists(Settings.PathToSavedServerList))
            {
                return File.ReadAllText(Settings.PathToSavedServerList);
            }
            else
            {
                File.WriteAllText(Settings.PathToSavedServerList, "[{}]");
                return "[{}]";
            }
        }

        public static void Save(string serverList)
        {
            File.WriteAllText(Settings.PathToSavedServerList, serverList);
        }
    }
}
