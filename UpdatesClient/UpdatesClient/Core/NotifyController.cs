using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdatesClient.UI.Controllers;

namespace UpdatesClient.Core
{
    public static class NotifyController
    {
        private async static void Show(PopupNotify popup, int delayMs)
        {
            popup.Margin = new System.Windows.Thickness(0, 0, 0, 10);
            popup.ClickClose += Popup_ClickClose;
            NotifyList.NotifyPanel?.panelList.Children.Add(popup);
            await Task.Delay(delayMs);
            NotifyList.NotifyPanel?.panelList.Children.Remove(popup);
        }

        public static void Show(PopupNotify.Type type, string status, string text, int delayMs = 5000)
        {
            Show(new PopupNotify(type, status, text), delayMs);
        }
        public static void Show(Exception exception, int delayMs = 3000)
        {
            Show(new PopupNotify(exception), delayMs);
        }

        private static void Popup_ClickClose(object sender, EventArgs e)
        {
            NotifyList.NotifyPanel?.panelList.Children.Remove((PopupNotify)sender);
        }
    }
}
