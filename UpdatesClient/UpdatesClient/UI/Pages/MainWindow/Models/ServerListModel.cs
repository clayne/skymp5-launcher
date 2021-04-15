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
    public class ServerListModel : INotifyPropertyChanged
    {
        private Visibility visibleServerBlock;

        public event PropertyChangedEventHandler PropertyChanged;
        

        public Visibility VisibleServerBlock
        {
            get { return visibleServerBlock; }
            set { visibleServerBlock = value; OnPropertyChanged(); }
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
