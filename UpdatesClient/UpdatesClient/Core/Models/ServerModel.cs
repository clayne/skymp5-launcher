using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
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

        [JsonIgnore]
        public string ViewName
        {
            get
            {
                if (Name.Length > 18) return Name.Substring(0, 18) + "...";
                else return Name;
            }
        }

        [JsonProperty("maxPlayers")]
        public int MaxPlayers { get; set; }

        [JsonProperty("online")]
        public int Online { get; set; }

        public int ID => (IP + Port.ToString()).GetHashCode();
        public string Address => $"{IP}:{Port}";

        public int DataPort { get => Port == 7777 ? 3000 : Port + 1; }
        public string AddressData => $"{IP}:{DataPort}";

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
            return await Net.Request(Net.URL_SERVERS, "GET", false, null);
        }

        public static string Load()
        {
            if (File.Exists(DefaultPaths.PathToSavedServerList))
            {
                return File.ReadAllText(DefaultPaths.PathToSavedServerList);
            }
            else
            {
                File.WriteAllText(DefaultPaths.PathToSavedServerList, "[{}]");
                return "[{}]";
            }
        }

        public static void Save(string serverList)
        {
            File.WriteAllText(DefaultPaths.PathToSavedServerList, serverList);
        }
    }
}
