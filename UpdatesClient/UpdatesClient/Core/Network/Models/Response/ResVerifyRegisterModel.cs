using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatesClient.Core.Network.Models.Response
{
    public struct ResVerifyRegisterModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
