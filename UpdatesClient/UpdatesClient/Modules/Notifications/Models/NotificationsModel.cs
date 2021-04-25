using System.Collections.Generic;
using UpdatesClient.Core.Helpers;

namespace UpdatesClient.Modules.Notifications.Models
{
    public class NotificationsModel : IJsonSaver
    {
        public int LastID { get; set; }
        public List<NotifyModel> Notifications { get; set; }

        public NotificationsModel()
        {
            Notifications = new List<NotifyModel>();
        }
    }
}
