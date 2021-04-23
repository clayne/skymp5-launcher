using System.Windows;
using System.Windows.Controls;
using UpdatesClient.Core.Enums;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.GameManager;
using UpdatesClient.UI.Pages.MainWindow.Models;

namespace UpdatesClient.UI.Pages.MainWindow
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class SettingsPanel : UserControl
    {
        private SettingsPanelModel Model;

        public SettingsPanel()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            Model = new SettingsPanelModel()
            {
                Locales = new string[] { "Русский", "Английский" },
                SelectedLocale = GetIdByLocale(Settings.Locale),
                SkyrimPath = Settings.PathToSkyrim,
                ExpFunctions = Settings.ExperimentalFunctions ?? false
            };
            grid.DataContext = Model;
        }

        public void Save()
        {
            Settings.PathToSkyrim = Model.SkyrimPath;
            Settings.Locale = GetLocaleById(Model.SelectedLocale);
            Settings.ExperimentalFunctions = Model.ExpFunctions;
            Settings.Save();
        }

        private int GetIdByLocale(Locales locale)
        {
            switch (locale)
            {
                case Locales.ru_RU:
                    return 0;
                case Locales.en_US:
                    return 1;
                default: return -1;
            }
        }

        private Locales GetLocaleById(int locale)
        {
            return (Locales)(locale + 1);
        }

        private void SelectSkyrimPath(object sender, RoutedEventArgs e)
        {
            Model.SkyrimPath = GameVerification.GetGameFolder() ?? Settings.PathToSkyrim;
        }
    }
}
