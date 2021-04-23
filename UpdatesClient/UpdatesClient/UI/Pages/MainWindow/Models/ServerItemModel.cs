using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UpdatesClient.Core.Models;

namespace UpdatesClient.UI.Pages.MainWindow.Models
{
    public class ServerItemModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ServerModel Server;
        private bool selected;
        private bool favorite;

        public string ViewName
        {
            get => Server.Name.Length > 28 ? Server.Name.Substring(0, 28) + "..." : Server.Name;
        }

        public string Players { get => $"{Server.Online} / {Server.MaxPlayers}"; }

        public string Address { get => $"{Server.Address}"; }

        public string Description { get => "<Empty>"; }

        public Visibility HasMicrophone { get => Visibility.Hidden; }

        public string Locale { get => $"Ru"; }

        public string Ping { get => $"-"; }

        public bool Selected { get { return selected; } set { selected = value; OnPropertyChanged(); OnPropertyChanged("SelectedRect"); } }

        public Visibility SelectedRect { get => selected ? Visibility.Visible : Visibility.Hidden; }

        public bool Favorite
        {
            get { return favorite; }
            set { favorite = value; OnPropertyChanged(); }
        }

        public ServerItemModel(ServerModel server)
        {
            Server = server;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
