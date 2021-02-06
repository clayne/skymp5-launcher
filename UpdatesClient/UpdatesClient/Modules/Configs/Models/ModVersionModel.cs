﻿using Security.Extensions;
using System;

namespace UpdatesClient.Modules.Configs.Models
{
    public class ModVersionModel
    {
        public DateTime? LastDmpReported { get; set; }

        public ModVersionModel()
        {
            LastDmpReported = new DateTime();
        }
    }
}
