using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.GameManager;
using UpdatesClient.Modules.GameManager.Model;
using UpdatesClient.UI.Windows.InstallerWindowModels;

namespace UpdatesClient.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для InstallerWindow.xaml
    /// </summary>
    public partial class InstallerWindow : Window
    {
        private InstallerWindowModel model;

        public bool InstallReady { get; set; } = false;
        public ResultGameVerification Result { get; set; }

        public InstallerWindow()
        {
            InitializeComponent();
            model = new InstallerWindowModel()
            {
                SkyrimVersion = "-",
                SKSEVersion = "-",
                CanInstall = false
            };
            DataContext = model;
        }

        private void SelectSkyrimPath(object sender, RoutedEventArgs e)
        {
            model.PathToSkyrim = GameVerification.GetGameFolder() ?? model.PathToSkyrim;
            VerifyGame();
        }

        private async void VerifyGame()
        {
            await Task.Yield();
            ResultGameVerification result = GameVerification.VerifyGame(model.PathToSkyrim, "");
            if (result.IsGameFound)
            {
                model.SkyrimVersion = result.GameVersion.ToString();
                if (result.IsSKSEFound)
                {
                    model.SKSEVersion = result.SKSEVersion.ToString();
                }
                else
                {
                    model.SKSEVersion = "не найден (будет установлен)";
                }
                Result = result;
                model.CanInstall = true;
            }
            else
            {
                model.SkyrimVersion = "(игра не найдена)";
                model.SKSEVersion = "-";
                model.CanInstall = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Settings.PathToSkyrim = model.PathToSkyrim;
            Settings.Save();
            InstallReady = true;
            Close();
        }
    }
}
