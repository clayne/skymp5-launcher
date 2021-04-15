using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UpdatesClient.UI.Pages.MainWindow.Models
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        private ServerListModel serverList;
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public ServerListModel ServerList
        {
            get { return serverList; }
            set { serverList = value; OnPropertyChanged(); }
        }

        public MainWindowModel(string userName)
        {
            UserName = userName;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
