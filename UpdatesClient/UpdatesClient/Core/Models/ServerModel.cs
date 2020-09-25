﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatesClient.Core.Models
{
    public struct ServerModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ServerModel(int iD, string name)
        {
            ID = iD;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
