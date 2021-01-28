using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdatesClient.Core;
using UpdatesClient.Core.Helpers;

namespace UpdatesClient.Modules.ModsManager.Models
{
    public class ModModel : IJsonSaver
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public bool HasMainFile { get; set; }
        public string MainFile { get; set; }

        public Dictionary<string, uint> Files { get; set; } = new Dictionary<string, uint>();
    }
}
