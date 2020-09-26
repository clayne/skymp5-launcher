using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatesClient.Core.Models
{
    public struct ServerModel
    {
        public string IP { get; private set; }
        public int Port { get; set; }
        public string Name { get; private set; }        
        public int MaxPlayers { get; private set; }
        public int Online { get; set; }
        public int ID => IP.GetHashCode();
        public List<string> Info => new List<string> { "Online: " + Online, "Max Players: " + MaxPlayers };

        public ServerModel(string iP, int port, string name, int maxPlayers, int online) : this()
        {
            IP = iP;
            Port = port;
            Name = name;
            MaxPlayers = maxPlayers;
            Online = online;
        }

        public override string ToString()
        {
            return Name + " (" + Online + " / " + MaxPlayers + ")";
        }
    }
}
