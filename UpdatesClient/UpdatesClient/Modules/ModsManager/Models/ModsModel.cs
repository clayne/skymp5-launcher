using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatesClient.Modules.ModsManager.Models
{
    public class ModsModel
    {
        List<string> Mods { get; set; } = new List<string>();
        List<string> EnabledMods { get; set; } = new List<string>();
    }
}
