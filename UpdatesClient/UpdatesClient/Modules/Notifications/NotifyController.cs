using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using UpdatesClient.Core;
using UpdatesClient.Core.Helpers;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.Notifications.Enums;
using UpdatesClient.Modules.Notifications.Models;
using UpdatesClient.Modules.Notifications.UI;
using UpdatesClient.UI.Controllers;

namespace UpdatesClient.Modules.Notifications
{
    public static class NotifyController
    {
        private static readonly object sync = new object();
        private static NotificationsModel Notifications = new NotificationsModel();

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
            Load();
            GetNotify();
            while (true)
            {
                while (popupNotifies.Count != 0)
                {
                    PopupNotify popup;
                    lock (sync)
                    {
                        NotifyModel popupModel = popupNotifies.Dequeue();
                        popup = new PopupNotify(popupModel);
                        Notifications.Notifications.Add(popupModel);
                    }

                    NotifyList.NotifyPanel?.panelList.Children.Insert(0, popup);
                    popup.Margin = new Thickness(0, 0, 0, 20);
                    popup.ClickClose += Popup_ClickClose;
                }
                await Task.Delay(100);
            }
        }

        public static async void GetNotify()
        {
            await Task.Yield();

            while (true)
            {
                try
                {
                    string jsn = await Net.Request(Net.URL_ApiLauncher + "GetNotifications/" + Notifications.LastID, "POST", false, null);
                    WNotifyModel[] models = JsonConvert.DeserializeObject<WNotifyModel[]>(jsn);

                    foreach (WNotifyModel model in models)
                    {
                        if (Notifications.LastID < model.Id) Notifications.LastID = model.Id;
                        lock (sync)
                            popupNotifies.Enqueue(new NotifyModel(model.Text, model.Color, model.Type));
                    }
                }
                catch { }
                await Task.Delay(45000);
            }
        }

        public static void Show(string text)
        {
            lock (sync)
                popupNotifies.Enqueue(new NotifyModel(text, null, NotificationType.Text));
        }
        public static void Show(Exception exception)
        {
            lock (sync)
                popupNotifies.Enqueue(new NotifyModel(exception));
        }

        private static void Load()
        {
            Notifications = Notifications.Load<NotificationsModel>(DefaultPaths.PathToLocal + "Notifications.json");
            foreach (NotifyModel notify in Notifications.Notifications)
            {
                PopupNotify popup = new PopupNotify(notify);
                NotifyList.NotifyPanel?.panelList.Children.Insert(0, popup);
                popup.Margin = new Thickness(0, 0, 0, 20);
                popup.ClickClose += Popup_ClickClose;
            }
        }

        public static void Save()
        {
            Notifications.Save(DefaultPaths.PathToLocal + "Notifications.json");
        }

        private static void Popup_ClickClose(object sender, EventArgs e)
        {
            Close((PopupNotify)sender);
        }

        private static async void Close(PopupNotify popup)
        {
            popup.ClickClose -= Popup_ClickClose;
            popup.BeginAnimation(UserControl.OpacityProperty, Hide);
            Notifications.Notifications.Remove(popup.notifyModel);
            await Task.Delay(Hide.Duration.TimeSpan);

            UIElementCollection collection = NotifyList.NotifyPanel?.panelList.Children;
            NotifyList.NotifyPanel?.panelList.Children.Remove(popup);
        }
    }
}
