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
        public string ip { get; set; }
        public int port { get; set; }
        public string name { get; set; }        
        public int maxPlayers { get; set; }
        public int online { get; set; }
        public int ID => (ip + port.ToString()).GetHashCode();

        public override string ToString()
        {
            return name + " (" + online + " / " + maxPlayers + ")";
        }

        public bool isSameServer(SkympClientSettings settings)
        {
            return (settings.ip + settings.port.ToString()).GetHashCode() == ID;
        }

        public SkympClientSettings ToSkympClientSettings(SkympClientSettings oldServer)
        {
            return new SkympClientSettings
            {
                ip = this.ip,
                isEnableConsole = oldServer.isEnableConsole,
                isShowMe = oldServer.isShowMe,
                port = this.port
            };
        }

        public static async Task<List<ServerModel>> GetServerList()
        {
            return JArray.Parse(await Net.Request(Net.URL_SERVERS, null)).ToObject<List<ServerModel>>();
        }
    }
}
