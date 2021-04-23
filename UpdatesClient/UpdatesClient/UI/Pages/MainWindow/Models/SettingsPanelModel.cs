using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UpdatesClient.UI.Pages.MainWindow.Models
{
    public class SettingsPanelModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string skyrimPath;
        private string[] locales;
        private int selectedLocale;
        private bool expFunctions;

        public bool ExpFunctions
        {
            get { return expFunctions; }
            set { expFunctions = value; OnPropertyChanged(); }
        }


        public int SelectedLocale
        {
            get { return selectedLocale; }
            set { selectedLocale = value; OnPropertyChanged(); }
        }

        public string[] Locales
        {
            get { return locales; }
            set { locales = value; OnPropertyChanged(); }
        }

        public string SkyrimPath
        {
            get { return skyrimPath; }
            set { skyrimPath = value; OnPropertyChanged(); }
        }



        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
