using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatesClient.Core.Models
{
    public struct SkympClientSettings
    {
        [JsonProperty("server-ip")]
        public string ip { get; set; }

        [JsonProperty("server-port")]
        public int port { get; set; }
        
        [JsonProperty("show-me")]
        public bool isShowMe { get; set; }
        
        [JsonProperty("enable-console")]
        public bool isEnableConsole { get; set; }
    }
}
