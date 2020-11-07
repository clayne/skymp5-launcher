using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using UpdatesClient.UI.Controllers;

namespace UpdatesClient.Core
{
    public static class NotifyController
    {
        private static readonly Queue<PopupNotify> popupNotifies = new Queue<PopupNotify>();
        private static readonly DoubleAnimation Hide = new DoubleAnimation
        {
            From = 1,
            To = 0,
            AccelerationRatio = 0.9,
            Duration = TimeSpan.FromMilliseconds(500),
        };

        private static bool run = false;

        private async static void Show()
        {
            while (NotifyList.NotifyPanel?.panelList.Children.Count < 2 && popupNotifies.Count != 0)
            {
                PopupNotify popup = popupNotifies.Dequeue();
                NotifyList.NotifyPanel?.panelList.Children.Add(popup);
                popup.Margin = new System.Windows.Thickness(0, 0, 0, 10);
                popup.ClickClose += Popup_ClickClose;
            }
            if (run) return;
            run = true;
            if (NotifyList.NotifyPanel?.panelList.Children.Count != 0)
            {
                PopupNotify popup = (PopupNotify)NotifyList.NotifyPanel?.panelList.Children[0];
                await Task.Delay(popup.DelayMs);
                Close(popup);
            }
        }

        public static void Show(PopupNotify.Type type, string status, string text, int delayMs = 6000)
        {
            popupNotifies.Enqueue(new PopupNotify(type, status, text, delayMs));
            Show();
        }
        public static void Show(Exception exception, int delayMs = 8000)
        {
            popupNotifies.Enqueue(new PopupNotify(exception, delayMs));
            Show();
        }

        private static void Popup_ClickClose(object sender, EventArgs e)
        {
            Close((PopupNotify)sender);
        }

        private async static void Close(PopupNotify popup)
        {
            popup.ClickClose -= Popup_ClickClose;
            popup.BeginAnimation(UserControl.OpacityProperty, Hide);
            await Task.Delay(Hide.Duration.TimeSpan);

            UIElementCollection collection = NotifyList.NotifyPanel?.panelList.Children;
            if (collection.Count > 0 && (PopupNotify)collection[0] == popup) run = false;

            NotifyList.NotifyPanel?.panelList.Children.Remove(popup);
            Show();
        }
    }
}
