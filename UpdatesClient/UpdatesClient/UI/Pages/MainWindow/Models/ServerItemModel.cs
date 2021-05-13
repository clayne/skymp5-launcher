using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UpdatesClient.Core.Models;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.GameManager;
using UpdatesClient.Modules.GameManager.Models.ServerManifest;

namespace UpdatesClient.UI.Pages.MainWindow.Models
{
    public class ServerItemModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly bool inited = false;
        private readonly Action resort;

        public ServerModel Server;
        private bool selected;
        private bool favorite;
        private string ping;

        private List<string> mods;

        public List<string> Mods
        {
            get { return mods; }
            set { mods = value; OnPropertyChanged(); }
        }


        public string ViewName
        {
            get => Server.Name.Length > 28 ? Server.Name.Substring(0, 28) + "..." : Server.Name;
        }

        public string Players { get => $"{Server.Online} / {Server.MaxPlayers}"; }

        public string Address { get => $"{Server.Address}"; }

        public string Description { get => "<Empty>"; }

        public Visibility HasMicrophone { get => Visibility.Hidden; }

        public string Locale { get => $"Ru"; }

        public string Ping { get { return ping; } set { ping = value; OnPropertyChanged(); } }
        public bool Selected { get { return selected; } set { selected = value; OnPropertyChanged(); OnPropertyChanged("SelectedRect"); } }

        public Visibility SelectedRect { get => selected ? Visibility.Visible : Visibility.Hidden; }

        public bool Favorite
        {
            get { return favorite; }
            set { favorite = value; OnPropertyChanged(); SetFavorite(); }
        }

        public ServerItemModel(ServerModel server, Action action = null)
        {
            Server = server;
            resort = action;
            Mods = new List<string>();
            if (Settings.FavoriteServers.Contains(Server.ID))
            {
                Favorite = true;
            }
            GetPing();
            inited = true;
        }

        private static CancellationTokenSource cancelTokenSource;
        public async void GetManifest()
        {
            if (cancelTokenSource != null)
            {
                cancelTokenSource.Cancel();
            }
            cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            ServerModsManifest mods = new ServerModsManifest();
            try
            {
                mods = await GameUtilities.GetManifest(Server.AddressData);
                //await Task.Run(() => 
                //{
                //    Task<ServerModsManifest> w = GameUtilities.GetManifest(Server.AddressData);
                //    w.Wait();
                //    mods = w.Result; 
                //}, token);
            }
            catch (WebException) { return; }
            catch (TaskCanceledException) { return; }

            if (cancelTokenSource == null || cancelTokenSource.IsCancellationRequested) return;

            GetMods(mods);

            cancelTokenSource = null;
        }

        private async void GetMods(ServerModsManifest mods)
        {
            await Task.Yield();
            List<string> WhiteListFiles = new List<string>(5)
            {
                "Skyrim.esm",
                "Update.esm",
                "Dawnguard.esm",
                "HearthFires.esm",
                "Dragonborn.esm"
            };
            
            mods.Mods.RemoveAll(r => WhiteListFiles.Contains(r.FileName));
            Mods.Clear();
            if (mods.Mods.Count != 0)
            {
                Mods.AddRange(mods.Mods.ConvertAll(c => c.FileName.Substring(0, c.FileName.LastIndexOf('.'))));
            }
            else
            {
                Mods.Add("<no mods>");
            }
        }

        private async void GetPing()
        {
            try
            {
                Ping ping = new Ping();
                PingOptions options = new PingOptions();
                byte[] buffer = new byte[16] { 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48 };
                const int timeout = 600;
                PingReply reply = await ping.SendPingAsync(Server.IP, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    Ping = reply.RoundtripTime.ToString();
                }
                else
                {
                    Ping = "-";
                }
            }
            catch { Ping = "-"; }
        }

        private void SetFavorite()
        {
            if (inited)
            {
                if (favorite)
                {
                    Settings.FavoriteServers.Add(Server.ID);
                    resort?.Invoke();
                }
                else if (Settings.FavoriteServers.Contains(Server.ID))
                {
                    Settings.FavoriteServers.Remove(Server.ID);
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
