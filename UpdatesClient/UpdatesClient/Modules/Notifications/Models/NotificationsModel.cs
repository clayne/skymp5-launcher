using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdatesClient.Core.Helpers;

namespace UpdatesClient.Modules.Notifications.Models
{
    public class NotificationsModel :  IJsonSaver
    {
        public int LastID { get; set; }
        public List<NotifyModel> Notifications { get; set; }

        public NotificationsModel()
        {
            Notifications = new List<NotifyModel>();
        }
    }
}
