using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using UpdatesClient.Core.Enums;
using UpdatesClient.UI.Controllers.ServerBlock;
using Res = UpdatesClient.Properties.Resources;

namespace UpdatesClient.UI.Pages.MainWindow.Models
{
    public class ServerListModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<ServerItemModel> serversList;
        private ServerItemModel selectedServer;
        private TabEn tab = TabEn.Descrpt;
        private object content;
        private bool mainButtonProgressBar;
        private MainButtonStatus mainButtonStatus;

        public MainButtonStatus MainButtonStatus
        {
            get { return mainButtonStatus; }
            set { mainButtonStatus = value; OnPropertyChanged(); OnPropertyChanged("MainButtonText"); }
        }

        public bool MainButtonProgressBar
        {
            get { return mainButtonProgressBar; }
            set { mainButtonProgressBar = value; OnPropertyChanged(); OnPropertyChanged("ShowMainButton"); OnPropertyChanged("ShowMainProgressBar"); }
        }

        public object Content
        {
            get { return content; }
            set { content = value; OnPropertyChanged(); }
        }

        public enum TabEn
        {
            Descrpt,
            Mods,
            Settings
        }

        public TabEn Tab
        {
            get { return tab; }
            set
            {
                tab = value;
                OnPropertyChanged();
                OnPropertyChanged("IsDescrptTab");
                OnPropertyChanged("IsModsTab");
                OnPropertyChanged("IsSettingsTab");
                SetContent();
            }
        }

        public string MainButtonText
        {
            get
            {
                switch (mainButtonStatus)
                {
                    case MainButtonStatus.Install:
                        return "Install";
                    case MainButtonStatus.Update:
                        return Res.UPDATE;
                    case MainButtonStatus.Play:
                        return Res.PLAY;
                    case MainButtonStatus.Retry:
                        return Res.RETRY;
                    default:
                        return "ERROR";
                }
            }
        }

        public Visibility ShowMainButton { get => mainButtonProgressBar ? Visibility.Collapsed : Visibility.Visible; }
        public Visibility ShowMainProgressBar { get => mainButtonProgressBar ? Visibility.Visible : Visibility.Collapsed; }

        public bool IsDescrptTab
        {
            get { return tab == TabEn.Descrpt; }
            set { Tab = value ? TabEn.Descrpt : tab; }
        }

        public bool IsModsTab
        {
            get { return tab == TabEn.Mods; }
            set { Tab = value ? TabEn.Mods : tab; }
        }

        public bool IsSettingsTab
        {
            get { return tab == TabEn.Settings; }
            set { Tab = value ? TabEn.Settings : tab; }
        }

        public ServerItemModel SelectedServer
        {
            get { return selectedServer; }
            set
            {
                if (selectedServer != null) selectedServer.Selected = false;
                selectedServer = value;
                if (selectedServer != null) selectedServer.Selected = true;
                Tab = TabEn.Descrpt;
                OnPropertyChanged();
                OnPropertyChanged("VisibleServerBlock");
            }
        }

        public List<ServerItemModel> ServersList
        {
            get { return serversList; }
            set { serversList = value; OnPropertyChanged(); }
        }

        public Visibility VisibleServerBlock
        {
            get => selectedServer != null ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetContent()
        {
            switch (tab)
            {
                case TabEn.Descrpt:
                    Content = new ServerDescription();
                    break;
                case TabEn.Mods:
                    Content = new ServerMods();
                    break;
                case TabEn.Settings:
                    Content = new ServerPlayers();
                    break;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
