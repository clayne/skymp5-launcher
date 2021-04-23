using System.Windows;
using UpdatesClient.Core;
using UpdatesClient.Modules;
using UpdatesClient.Modules.Configs;
using UpdatesClient.UI.Pages.MainWindow;
using UpdatesClient.UI.Pages.MainWindow.Models;
using Res = UpdatesClient.Properties.Resources;

namespace UpdatesClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowModel WindowModel;
        private ServerList ServerList;

        public MainWindow()
        {
            InitializeComponent();
            userButton.LogoutBtn.Click += LogOut_Click;

            WindowModel = new MainWindowModel(Settings.UserName);
            wind.DataContext = WindowModel;

            ServerList = new ServerList();
            content.Content = ServerList;

            wind.Loaded += Wind_Loaded;
        }

        private void Wind_Loaded(object sender, RoutedEventArgs e)
        {
            NotifyController.Init();
            ServerList.PostInit();
            ModulesManager.PostInitModules();
        }

        //TODO: переход на окно авторизации
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            //TODO: аннулирование токена

            Settings.UserId = 0;
            Settings.UserName = "";
            Settings.UserToken = "";
            Settings.Save();
        }

        private void openSettings(object sender, RoutedEventArgs e)
        {
            if (WindowModel.IsOpenSettings)
            {
                settingsPanel.Init();
            }
            else
            {
                settingsPanel.Save();
                ServerList.PostInit();
            }
        }
    }
}
