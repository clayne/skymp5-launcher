using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdatesClient.UI.Controllers;
using Res = UpdatesClient.Properties.Resources;

namespace UpdatesClient.Core.Models
{
    public struct NotifyModel
    {
        public PopupNotify.Type Type { get; set; }
        public string Status { get; set; }
        public string Text { get; set; }
        public int DelayMs { get; set; }

        public NotifyModel(PopupNotify.Type type, string status, string text, int delayMs = 6000) : this()
        {
            Type = type;
            Status = status;
            Text = text;
            DelayMs = delayMs;
        }

        public NotifyModel(Exception exception, int delayMs = 8000) : this()
        {
            Type = PopupNotify.Error;
            Status = Res.Error;
            Text = exception.Message;
            DelayMs = delayMs;
        }
    }
}
