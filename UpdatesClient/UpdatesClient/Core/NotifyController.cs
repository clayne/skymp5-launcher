using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using UpdatesClient.Core.Models;
using UpdatesClient.UI.Controllers;

namespace UpdatesClient.Core
{
    public static class NotifyController
    {
        private static readonly object sync = new object();
        private static readonly Queue<NotifyModel> popupNotifies = new Queue<NotifyModel>();
        private static readonly DoubleAnimation Hide = new DoubleAnimation
        {
            From = 1,
            To = 0,
            AccelerationRatio = 0.9,
            Duration = TimeSpan.FromMilliseconds(500),
        };

        public static async void Init()
        {
            while (true)
            {
                while (NotifyList.NotifyPanel?.panelList.Children.Count < 2 && popupNotifies.Count != 0)
                {
                    PopupNotify popup;
                    lock (sync)
                    {
                        NotifyModel popupModel = popupNotifies.Dequeue();
                        popup = new PopupNotify(popupModel);
                    }

                    NotifyList.NotifyPanel?.panelList.Children.Add(popup);
                    popup.Margin = new Thickness(0, 0, 0, 10);
                    popup.ClickClose += Popup_ClickClose;
                }
                if (NotifyList.NotifyPanel?.panelList.Children.Count != 0)
                {
                    PopupNotify popup = (PopupNotify)NotifyList.NotifyPanel?.panelList.Children[0];
                    await Task.Delay(popup.DelayMs);
                    if (NotifyList.NotifyPanel?.panelList.Children.Contains(popup) == true) Close(popup);
                }
                else
                {
                    await Task.Delay(250);
                }
            }
        }

        public static void Show(PopupNotify.Type type, string status, string text, int delayMs = 6000)
        {
            lock (sync)
                popupNotifies.Enqueue(new NotifyModel(type, status, text, delayMs));
        }
        public static void Show(Exception exception, int delayMs = 8000)
        {
            lock (sync)
                popupNotifies.Enqueue(new NotifyModel(exception, delayMs));
        }

        private static void Popup_ClickClose(object sender, EventArgs e)
        {
            Close((PopupNotify)sender);
        }

        private static async void Close(PopupNotify popup)
        {
            popup.ClickClose -= Popup_ClickClose;
            popup.BeginAnimation(UserControl.OpacityProperty, Hide);
            await Task.Delay(Hide.Duration.TimeSpan);

            UIElementCollection collection = NotifyList.NotifyPanel?.panelList.Children;
            NotifyList.NotifyPanel?.panelList.Children.Remove(popup);
        }
    }
}
