using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatesClient.Modules.ModsManager.Models
{
    public class ModModel
    {
        public string Name { get; set; }
        public string Hash { get; set; }

        public Dictionary<string, string> Files { get; set; } = new Dictionary<string, string>();
    }
}
