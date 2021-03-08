﻿using BlendModeEffectLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UpdatesClient.Core;
using UpdatesClient.Core.Models;
using UpdatesClient.Core.Models.ServerManifest;
using UpdatesClient.Core.Network;
using UpdatesClient.Modules;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.Debugger;
using UpdatesClient.Modules.Downloader;
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
            TitleWindow.MouseLeftButtonDown += (s, e) => DragMove();
            authorization.TitleWindow.MouseLeftButtonDown += (s, e) => DragMove();

            CloseBtn.Click += (s, e) => Application.Current.Shutdown();
            authorization.CloseBtn.Click += (s, e) => Application.Current.Shutdown();

            MinBtn.Click += (s, e) => WindowState = WindowState.Minimized;
            authorization.MinBtn.Click += (s, e) => WindowState = WindowState.Minimized;
            
            userButton.LogoutBtn.Click += LogOut_Click;
            authorization.SignIn += Authorization_SignIn;

            ModulesManager.PostInitModules(progressBar);

            wind.Loaded += Wind_Loaded;
        }
        
        private async void Authorization_SignIn()
        {
            try
            {
                await GetLogin();
                authorization.Visibility = Visibility.Collapsed;
            }
            catch
            {
                authorization.Visibility = Visibility.Visible;
                return;
            }

            await Task.Delay(100);

            await CheckGame();
            await CheckClientUpdates();
            FillServerList();
        }

        private async Task GetLogin()
        {
            var username = await Account.GetLogin();
            Settings.UserName = username;
            userButton.Text = username;
        }
        private async void Wind_Loaded(object sender, RoutedEventArgs e)
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

            Mods.Init();
            ModVersion.Load();
            FileWatcher.Init();

            if (!Mods.ExistMod("SkyMPCore"))
            {
                GameCleaner.Clear();
            }

            SetBackgroundServerList();
            Authorization_SignIn();
        }
        private ResultGameVerification CheckSkyrim()
        {
            string pathToSkyrim = Settings.PathToSkyrim;
            ResultGameVerification result = default;
            try
            {
                do
                {
                    while (string.IsNullOrEmpty(pathToSkyrim) || !Directory.Exists(pathToSkyrim))
                    {
                        string path = GameVerification.GetGameFolder();
                        if (string.IsNullOrEmpty(path))
                        {
                            App.AppCurrent.Shutdown();
                            Close();
                        }
                        pathToSkyrim = path;
                    }

                    result = GameVerification.VerifyGame(pathToSkyrim, null);
                    if (result.IsGameFound)
                    {
                        if (Settings.PathToSkyrim != pathToSkyrim)
                        {
                            Settings.PathToSkyrim = pathToSkyrim;
                            Settings.Save();
                        }
                        break;
                    }

                    pathToSkyrim = null;
                    MessageBox.Show(Res.SkyrimNotFound, Res.Error);
                } while (true);
            }
            catch (Exception er)
            {
                Logger.FatalError("CheckPathToSkyrim", er);
                MessageBox.Show(Res.InitError, Res.Error);
                Close();
            }

            return result;
        }
        
        private async Task CheckGame() 
        {
            ResultGameVerification result = CheckSkyrim();

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

        private void SetBackgroundServerList()
        {
            try
            {
                serverListBg.Effect = new OverlayEffect()
                {
                    BInput = Graphics.GetGridBackGround(serverListBg, mainGrid, (ImageBrush)wind.Background, wind.Width, wind.Height)
                };
            }
            catch (Exception oe)
            {
                Logger.Error("SetBackgroundServerList_OverlayEffect", oe);
            }
        }
        private async void FillServerList()
        {
            try
            {
                List<ServerModel> list = null;
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
                list = ServerModel.ParseServersToList(servers);
                list.RemoveAll(x => x.IsEmpty());
                serverList.ItemsSource = null;
                serverList.ItemsSource = list;
                serverList.SelectedItem = list.Find(x => x.ID == Settings.LastServerID);
                if (NetworkSettings.ShowingServerStatus)
                {
                    if (!list.Exists(x => x.ID == NetworkSettings.OfficialServerAdress.GetHashCode()))
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
            progressBar.Show(true, Res.CheckingUpdates);
            try
            {
                string lastVersion = await Net.GetLastestVersion();
                string version = Mods.GetModHash("SkyMPCore");
                if (String.IsNullOrEmpty(version) || lastVersion != version) mainButton.ButtonStatus = MainButtonStatus.Update;
                else mainButton.ButtonStatus = MainButtonStatus.Play;
            }
            catch (WebException we)
            {
                NotifyController.Show(we);
                mainButton.ButtonStatus = MainButtonStatus.Retry;
            }
            catch (Exception e)
            {
                Logger.Error("CheckClient", e);
                NotifyController.Show(e);
                mainButton.ButtonStatus = MainButtonStatus.Retry;
            }
            progressBar.Hide();
        }
        private async void MainBtn_Click(object sender, EventArgs e)
        {
            if (blockMainBtn) return;
            blockMainBtn = true;
            switch (mainButton.ButtonStatus)
            {
                case MainButtonStatus.Play:
                    await Play();
                    break;
                case MainButtonStatus.Update:
                    if (await ModUtilities.GetClient()) goto case MainButtonStatus.Retry;
                    else mainButton.ButtonStatus = MainButtonStatus.Retry;
                    break;
                case MainButtonStatus.Retry:
                    await CheckClientUpdates();
                    break;
            }
            blockMainBtn = false;
        }
        private async Task Play()
        {
            if (!File.Exists($"{Settings.PathToSkyrim}\\skse64_loader.exe"))
            {
                await CheckGame();
                return;
            }

            if (serverList.SelectedItem == null)
            {
                NotifyController.Show(PopupNotify.Error, Res.Warning, Res.SelectServer);
                return;
            }

            string adressData;
            try
            {
                if (Directory.Exists(Path.GetDirectoryName(Settings.PathToSkympClientSettings)) && File.Exists(Settings.PathToSkympClientSettings))
                {
                    File.SetAttributes(Settings.PathToSkympClientSettings, FileAttributes.Normal);
                }

                SetServer();
                string adress = ((ServerModel)serverList.SelectedItem).Address;
                adressData = ((ServerModel)serverList.SelectedItem).AddressData;

                object gameData = await Account.GetSession(adress);
                if (gameData == null) return;
                SetSession(gameData);
            }
            catch (JsonSerializationException)
            {
                NotifyController.Show(PopupNotify.Error, Res.Error, Res.ErrorReadSkyMPSettings);
                return;
            }
            catch (JsonReaderException)
            {
                NotifyController.Show(PopupNotify.Error, Res.Error, Res.ErrorReadSkyMPSettings);
                return;
            }
            catch (UnauthorizedAccessException)
            {
                FileAttributes attr = new FileInfo(Settings.PathToSkympClientSettings).Attributes;
                Logger.Error("Play_UAException", new UnauthorizedAccessException($"UnAuthorizedAccessException: Unable to access file. Attributes: {attr}"));
                NotifyController.Show(PopupNotify.Error, Res.Error, "UnAuthorizedAccessException: Unable to access file");
                return;
            }
            catch (Exception e)
            {
                Logger.Error("Play", e);
                NotifyController.Show(PopupNotify.Error, Res.Error, e.Message);
                return;
            }

            if (!await SetMods(adressData)) return;

            try
            {
                Hide();
                bool crash = await GameLauncher.StartGame();
                Show();

                if (crash)
                {
                    Logger.ReportMetricaEvent("CrashDetected");
                    await Task.Delay(500);
                    await DebuggerUtilities.ReportDmp();
                }
            }
            catch
            {
                Logger.ReportMetricaEvent("HasNotAccess");
                Close();
            }
        }

        private async Task<ServerModsManifest> GetManifest(string adress)
        {
            string serverManifest = await Net.Request($"http://{adress}/manifest.json", "GET", false, null);
            return JsonConvert.DeserializeObject<ServerModsManifest>(serverManifest);
        }
        private Dictionary<string, List<(string, uint)>> GetMods(ServerModsManifest modsManifest)
        {
            List<string> WhiteList = Mods.WhiteListMods;

            List<string> mods = new List<string>();
            foreach (string mod in modsManifest.LoadOrder)
            {
                string modName = Path.GetFileNameWithoutExtension(mod);
                if (!mods.Contains(modName) && !WhiteList.Contains(modName)) mods.Add(modName);
            }

            Dictionary<string, List<(string, uint)>> files = new Dictionary<string, List<(string, uint)>>();
            foreach (string mod in mods)
            {
                files.Add(mod, 
                    modsManifest.Mods.FindAll(m => Path.GetFileNameWithoutExtension(m.FileName) == mod).Select(s => (s.FileName, (uint)s.CRC32)).ToList());
            }

            return files;
        }
        private async Task<bool> SetMods(string adress)
        {
            string path = DefaultPaths.PathToLocalSkyrim + "Plugins.txt";
            string content = "";

            try
            {
                await Mods.DisableAll();
                ServerModsManifest mods = Mods.CheckCore(await GetManifest(adress));
                Dictionary<string, List<(string, uint)>> needMods = GetMods(mods);

                foreach (KeyValuePair<string, List<(string, uint)>> mod in needMods)
                {
                    if (!Mods.ExistMod(mod.Key) || !Mods.CheckMod(mod.Key, mod.Value))
                    {
                        string tmpPath = Mods.GetTmpPath();
                        string desPath = tmpPath + "\\Data\\";

                        IO.CreateDirectory(desPath);
                        string mainFile = null;
                        foreach (var file in mod.Value)
                        {
                            await DownloadMod(desPath + file.Item1, adress, file.Item1);
                            if (mods.LoadOrder.Contains(file.Item1)) mainFile = file.Item1;
                        }
                        await Mods.AddMod(mod.Key, "", tmpPath, true, mainFile);
                    }
                    await Mods.EnableMod(Path.GetFileNameWithoutExtension(mod.Key));
                }

                foreach (var item in mods.LoadOrder)
                {
                    content += $"*{item}\n";
                }
            }
            catch (WebException)
            {
                if (NetworkSettings.CompatibilityMode)
                {
                    NotifyController.Show(PopupNotify.Normal, Res.Attempt, "Вероятно целевой сервер устарел, используется режим совместимости");
                    if (Mods.ExistMod("Farm"))
                        await Mods.OldModeEnable();
                    await Task.Delay(3000);
                    content = @"*FarmSystem.esp";
                }
                else
                {
                    NotifyController.Show(PopupNotify.Error, Res.Attempt, "Возможно целевой сервер устарел, так как не ответил на запрос");
                    return false;
                }
            }
            catch (FileNotFoundException)
            {
                NotifyController.Show(PopupNotify.Error, Res.Error, "Один или несколько модов не удалось загрузить с сервера");
                return false;
            }
            catch (Exception e)
            {
                Logger.Error("EnablerMods", e);
                NotifyController.Show(e);
                return false;
            }
            
            try
            {
                if (!Directory.Exists(DefaultPaths.PathToLocalSkyrim)) Directory.CreateDirectory(DefaultPaths.PathToLocalSkyrim);
                if (File.Exists(path) && File.GetAttributes(path) != FileAttributes.Normal) File.SetAttributes(path, FileAttributes.Normal);
                File.WriteAllText(path, content);
            }
            catch (UnauthorizedAccessException)
            {
                FileAttributes attr = new FileInfo(path).Attributes;
                Logger.Error("Write_Plugin_UAException", new UnauthorizedAccessException($"UnAuthorizedAccessException: Unable to access file. Attributes: {attr}"));
            }
            catch (Exception e)
            {
                Logger.Error("Write_Plugin_txt", e);
            }
            return true;
        }
        private async Task DownloadMod(string destinationPath, string adress, string file) 
        {
            string url = $"http://{adress}/{file}";
            await DownloadManager.DownloadFile(destinationPath, url, $"Загрузка {file}", null, null);
        }
        private void SetServer()
        {
            if (!Directory.Exists(Path.GetDirectoryName(Settings.PathToSkympClientSettings))) 
                Directory.CreateDirectory(Path.GetDirectoryName(Settings.PathToSkympClientSettings));
            
            SkympClientSettingsModel oldServer;
            
            if (File.Exists(Settings.PathToSkympClientSettings))
            {
                oldServer = JsonConvert.DeserializeObject<SkympClientSettingsModel>(File.ReadAllText(Settings.PathToSkympClientSettings));
            }
            else
            {
                oldServer = new SkympClientSettingsModel
                {
                    IsEnableConsole = false,
                    IsShowMe = false
                };
            }

            ServerModel newServer = (ServerModel)serverList.SelectedItem;
            if (newServer.IsSameServer(oldServer)) return;
            File.WriteAllText(Settings.PathToSkympClientSettings, JsonConvert.SerializeObject(newServer.ToSkympClientSettings(oldServer), Formatting.Indented));
            Settings.Save();
        }
        private void SetSession(object gameData)
        {
            SkympClientSettingsModel settingsModel = JsonConvert.DeserializeObject<SkympClientSettingsModel>(File.ReadAllText(Settings.PathToSkympClientSettings));
            settingsModel.GameData = gameData;
            File.WriteAllText(Settings.PathToSkympClientSettings, JsonConvert.SerializeObject(settingsModel, Formatting.Indented));
        }
        
        
        
        private void RefreshServerList(object sender, RoutedEventArgs e)
        {
            FillServerList();
        }
        private void ServerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (serverList.SelectedIndex != -1)
            {
                Settings.LastServerID = ((ServerModel)serverList.SelectedItem).ID;
            }
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

            authorization.Visibility = Visibility.Visible;
        }
    }
}
