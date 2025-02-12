﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatesClient.Modules.GameManager.Model
{
    public class ClientSettingsModel
    {
        [JsonProperty("server-ip")]
        public string IP { get; set; }

        [JsonProperty("server-port")]
        public int Port { get; set; }

        [JsonProperty("show-me")]
        public bool IsShowMe { get; set; }

        [JsonProperty("enable-console")]
        public bool IsEnableConsole { get; set; }

        [JsonProperty("gameData")]
        public object GameData { get; set; }
    }
}
