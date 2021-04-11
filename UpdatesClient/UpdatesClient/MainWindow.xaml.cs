using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UpdatesClient.Core;
using UpdatesClient.Core.Models;
using UpdatesClient.Core.Network;
using UpdatesClient.Modules;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.Debugger;
using UpdatesClient.Modules.GameManager;
using UpdatesClient.Modules.GameManager.AntiCheat;
using UpdatesClient.Modules.GameManager.Helpers;
using UpdatesClient.Modules.GameManager.Model;
using UpdatesClient.Modules.ModsManager;
using UpdatesClient.UI.Controllers;
using Res = UpdatesClient.Properties.Resources;

namespace UpdatesClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool blockMainBtn = false;

        public MainWindow()
        {
            InitializeComponent();

            userButton.LogoutBtn.Click += LogOut_Click;

            //ModulesManager.PostInitModules(progressBar);

            wind.Loaded += Wind_Loaded;
        }
        private void Wind_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Settings.ExperimentalFunctions == null)
                {
                    MessageBoxResult result = MessageBox.Show(Res.ExperimentalFeaturesText.Replace(@"\n", "\n"),
                        Res.ExperimentalFeatures, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                    if (result == MessageBoxResult.Yes) Settings.ExperimentalFunctions = true;
                    else Settings.ExperimentalFunctions = false;
                    Settings.Save();
                }
            }
            catch (Exception er) { Logger.Error("ExpFunc", er); }

            NotifyController.Init();

            Authorization_SignIn();
        }
        private async void Authorization_SignIn()
        {
            try
            {
                await GetLogin();
                //authorization.Visibility = Visibility.Collapsed;
            }
            catch
            {
                //authorization.Visibility = Visibility.Visible;
                return;
            }

            await Task.Delay(100);

            await CheckGame();
            ModVersion.Load();
            FileWatcher.Init();

            if (!Mods.ExistMod("SkyMPCore"))
            {
                GameCleaner.Clear();
            }

            await CheckClientUpdates();
            FillServerList();
        }
        private async Task GetLogin()
        {
            var username = await Account.GetLogin();
            Settings.UserName = username;
            userButton.Text = username;
        }
        
        //TODO: часть убрать в установщик
        private async Task CheckGame() 
        {
            ResultGameVerification result = GameVerification.CheckSkyrim();
            Mods.Init();

            if (!Mods.ExistMod("SKSE") && !result.IsSKSEFound && MessageBox.Show(Res.SKSENotFound, Res.Warning, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                blockMainBtn = true;
                if (await ModUtilities.GetSKSE())
                {
                    await Mods.EnableMod("SKSE");
                }
                blockMainBtn = false;
            }
            else if(Mods.ExistMod("SKSE"))
            {
                await Mods.EnableMod("SKSE");
            }

            try
            {
                if (!result.IsRuFixConsoleFound && !Mods.ExistMod("RuFixConsole"))
                {
                    blockMainBtn = true;
                    if (await ModUtilities.GetRuFixConsole())
                    {
                        await Mods.EnableMod("RuFixConsole");
                    }
                    blockMainBtn = false;
                }
            }
            catch (Exception er)
            {
                blockMainBtn = false;
                Logger.FatalError("CheckGame_SSERFix", er);
            }
        }

        private async void FillServerList()
        {
            try
            {
                string servers;
                try
                {
                    servers = await ServerModel.GetServers();
                    ServerModel.Save(servers);
                }
                catch
                {
                    servers = ServerModel.Load();
                }
                List<ServerModel> list = ServerModel.ParseServersToList(servers);
                list.RemoveAll(x => x.IsEmpty());
                //serverList.ItemsSource = null;
                //serverList.ItemsSource = list;
                //serverList.SelectedItem = list.Find(x => x.ID == Settings.LastServerID);
                if (NetworkSettings.ShowingServerStatus)
                {
                    int hashCode = NetworkSettings.OfficialServerAdress.GetHashCode();
                    if (!list.Exists(x => x.ID == hashCode))
                    {
                        bottomInfoPanel.Text = NetworkSettings.ServerStatus;
                        bottomInfoPanel.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        bottomInfoPanel.Visibility = Visibility.Hidden;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("FillServerList", e);
            }
        }
        private async Task CheckClientUpdates()
        {
            //progressBar.Show(true, Res.CheckingUpdates);
            try
            {
                string lastVersion = await Net.GetLastestVersion();
                string version = Mods.GetModHash("SkyMPCore");
                //if (String.IsNullOrEmpty(version) || lastVersion != version) mainButton.ButtonStatus = MainButtonStatus.Update;
                //else mainButton.ButtonStatus = MainButtonStatus.Play;
            }
            catch (WebException we)
            {
                NotifyController.Show(we);
                //mainButton.ButtonStatus = MainButtonStatus.Retry;
            }
            catch (Exception e)
            {
                Logger.Error("CheckClient", e);
                NotifyController.Show(e);
                //mainButton.ButtonStatus = MainButtonStatus.Retry;
            }
            //progressBar.Hide();
        }
        private async void MainBtn_Click(object sender, EventArgs e)
        {
            if (!blockMainBtn)
            {
                blockMainBtn = true;
                //switch (mainButton.ButtonStatus)
                //{
                //    case MainButtonStatus.Play:
                //        await Play((ServerModel)serverList.SelectedItem);
                //        break;
                //    case MainButtonStatus.Update:
                //        if (await ModUtilities.GetClient()) goto case MainButtonStatus.Retry;
                //        else mainButton.ButtonStatus = MainButtonStatus.Retry;
                //        break;
                //    case MainButtonStatus.Retry:
                //        await CheckClientUpdates();
                //        break;
                //}
                blockMainBtn = false;
            }
        }

        private async Task Play(ServerModel server)
        {
            if (!File.Exists($"{Settings.PathToSkyrim}\\skse64_loader.exe"))
            {
                await CheckGame();
                return;
            }

            //if (serverList.SelectedItem == null)
            //{
            //    NotifyController.Show(PopupNotify.Error, Res.Warning, Res.SelectServer);
            //    return;
            //}

            await GameUtilities.Play(server, this);
        }

        private void RefreshServerList(object sender, RoutedEventArgs e)
        {
            FillServerList();
        }
        private void ServerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (serverList.SelectedIndex != -1)
            //{
            //    Settings.LastServerID = ((ServerModel)serverList.SelectedItem).ID;
            //}
        }
        private void ServerList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject source = (DependencyObject)e.OriginalSource;

            if (source is TextBlock block)
            {
                if (block.DataContext is ServerModel)
                {
                    MainBtn_Click(sender, e);
                }
            }
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            //TODO: аннулирование токена

            Settings.UserId = 0;
            Settings.UserName = "";
            Settings.UserToken = "";
            Settings.Save();

            //authorization.Visibility = Visibility.Visible;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
