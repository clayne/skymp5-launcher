using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UpdatesClient.UI.Pages.MainWindow.Models
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        private string userName;
        private bool isOpenSettings;

        public bool IsOpenSettings
        {
            get { return isOpenSettings; }
            set { isOpenSettings = value; OnPropertyChanged(); }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
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
