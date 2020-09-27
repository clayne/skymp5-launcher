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
        public int ID => ip.GetHashCode();
        //public List<string> Info => new List<string> { "Online: " + online, "Max Players: " + maxPlayers };

        //public ServerModel(string iP, int port, string name, int maxPlayers, int online) : this()
        //{
        //    IP = iP;
        //    Port = port;
        //    Name = name;
        //    MaxPlayers = maxPlayers;
        //    Online = online;
        //}

        public override string ToString()
        {
            return name + " (" + online + " / " + maxPlayers + ")";
        }

        public static async Task<List<ServerModel>> GetServerList()
        {
            return JArray.Parse(await Net.Request(Net.URL_SERVERS, null)).ToObject<List<ServerModel>>();
        }

        //private static async Task<string> UploadServerList()
        //{
        //    string url = "https://skymp.io/api/servers";
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    using (var sr = new StreamReader(request.GetResponse().GetResponseStream()))
        //    {
        //        return await sr.ReadToEndAsync();
        //    }
        //}

    }
}
