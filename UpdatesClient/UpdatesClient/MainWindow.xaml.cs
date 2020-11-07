using BlendModeEffectLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using UpdatesClient.Core;
using UpdatesClient.Core.Models;
using UpdatesClient.Core.Network;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.GameManager;
using UpdatesClient.Modules.GameManager.AntiCheat;
using UpdatesClient.Modules.GameManager.Model;
using UpdatesClient.UI.Controllers;
using Yandex.Metrica;

namespace UpdatesClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Check file "SkyrimSE.exe"
        //Check file "skse64_loader.exe"
        //Base color: #FF04D9FF
        //War color: #FFFF7604
        //Er color: #FFFF0404

        public MainWindow()
        {
            InitializeComponent();
            TitleWindow.MouseLeftButtonDown += (s, e) => DragMove();
            authorization.TitleWindow.MouseLeftButtonDown += (s, e) => DragMove();
            CloseBtn.Click += (s, e) =>
            {
                YandexMetrica.Config.CrashTracking = false;
                Application.Current.Shutdown();
            };
            authorization.CloseBtn.Click += (s, e) =>
            {
                YandexMetrica.Config.CrashTracking = false;
                Application.Current.Shutdown();
            };
            MinBtn.Click += (s, e) =>
            {
                WindowState = WindowState.Minimized;
            };
            authorization.MinBtn.Click += (s, e) =>
            {
                WindowState = WindowState.Minimized;
            };
            progressBar.Hide();

            Settings.Load();

            userButton.LogoutBtn.Click += LogOut_Click;
            authorization.SignIn += Authorization_SignIn;

            wind.Loaded += Wind_Loaded;
        }

        private ImageBrush GetGridBackGround(FrameworkElement element)
        {
            Point relativePoint = element.TranslatePoint(new Point(0, 0), mainGrid);
            var image = (BitmapSource)((ImageBrush)wind.Background).ImageSource;
            double w = wind.Width / image.Width;
            double h = wind.Height / image.Height;
            var im = new CroppedBitmap(image, new Int32Rect((int)(relativePoint.X * w), (int)(relativePoint.Y * h), (int)(element.Width * w), (int)(element.Height * h)));
            return new ImageBrush(im);
        }

        private async void Authorization_SignIn()
        {
            authorization.Visibility = Visibility.Collapsed;
            try
            {
                await GetLogin();
                Logger.SetUser(Settings.UserId, Settings.UserName);
                authorization.Visibility = Visibility.Collapsed;
            }
            catch
            {
                authorization.Visibility = Visibility.Visible;
                return;
            }
            CheckClientUpdates();
        }

        private async Task GetLogin()
        {
            var username = await Account.GetLogin();
            JObject jObject = JObject.Parse(username);
            string name = jObject["name"].ToString();
            Settings.UserName = name;
            userButton.Text = name;
        }

        private async void Wind_Loaded(object sender, RoutedEventArgs e)
        {
            string pathToSkyrim = Settings.PathToSkyrim;
            ResultGameVerification result;
            do
            {
                while (string.IsNullOrEmpty(pathToSkyrim)
                || !Directory.Exists(pathToSkyrim)
                || !File.Exists($"{pathToSkyrim}\\SkyrimSE.exe")) pathToSkyrim = GetGameFolder();

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

                MessageBox.Show("Skyrim SE не обнаружен", "Ошибка");
            } while (true);

            ModVersion.Load();
            FileWatcher.Init();

            if (!result.IsSKSEFound && MessageBox.Show("SKSE не обнаружен, установить?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await InstallSKSE();
            }
            if (!result.IsRuFixConsoleFound
                    && ModVersion.HasRuFixConsole == null
                    && MessageBox.Show("SSE Rusian Fix Console не обнаружен, установить?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await InstallRuFixConsole();
                ModVersion.Save();
            }
            else
            {
                ModVersion.HasRuFixConsole = result.IsRuFixConsoleFound;
                ModVersion.Save();
            }

            FillServerList();

            serverListBg.Effect = new OverlayEffect()
            {
                BInput = GetGridBackGround(serverList)
            };

            try
            {
                await GetLogin();
                Logger.SetUser(Settings.UserId, Settings.UserName);
                authorization.Visibility = Visibility.Collapsed;
            }
            catch
            {
                authorization.Visibility = Visibility.Visible;
                return;
            }

            CheckClientUpdates();
        }

        private async void FillServerList()
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
        }

        private string GetGameFolder()
        {
            using (System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Выберите папку с TES: Skyrim SE";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
                else { Application.Current.Shutdown(); Close(); }
            }
            return null;
        }
        private async Task InstallSKSE()
        {
            string url = await Net.GetUrlToSKSE();
            string destinationPath = $@"{Settings.PathToSkyrimTmp}{url.Substring(url.LastIndexOf('/'), url.Length - url.LastIndexOf('/'))}";

            bool ok = await DownloadFile(destinationPath, url, "Загрузка SKSE");

            if (ok)
            {
                progressBar.Show(true, "Распаковка SKSE");
                try
                {
                    await Task.Run(() => Unpacker.UnpackArchive(destinationPath,
                        Settings.PathToSkyrim, Path.GetFileNameWithoutExtension(destinationPath)));
                }
                catch (Exception e)
                {
                    Logger.Error("ExtractSKSE", e);
                    NotifyController.Show(e);
                    mainButton.ButtonStatus = MainButtonStatus.Retry;
                }
                progressBar.Hide();
            }
        }

        private async Task InstallRuFixConsole()
        {
            string url = Net.URL_Mod_RuFix;
            string destinationPath = $@"{Settings.PathToSkyrimTmp}{url.Substring(url.LastIndexOf('/'), url.Length - url.LastIndexOf('/'))}";

            bool ok = await DownloadFile(destinationPath, url, "Загрузка фикса консоли");
            if (ok)
            {
                try
                {
                    progressBar.Show(true, "Распаковка");
                    ModVersion.HasRuFixConsole = await Task.Run(() => Unpacker.UnpackArchive(destinationPath, Settings.PathToSkyrim + "\\Data"));
                    ModVersion.Save();
                    progressBar.Hide();
                }
                catch (Exception e)
                {
                    Logger.Error("ExtractRuFix", e);
                    NotifyController.Show(e);
                }
            }
        }
        private async void CheckClientUpdates()
        {
            progressBar.Show(true, "Проверка обновлений");
            try
            {
                if (await Net.UpdateAvailable()) mainButton.ButtonStatus = MainButtonStatus.Update;
                else mainButton.ButtonStatus = MainButtonStatus.Play;
            }
            catch (Exception e)
            {
                Logger.Error("CheckClient", e);
                NotifyController.Show(e);
                mainButton.ButtonStatus = MainButtonStatus.Retry;
            }
            progressBar.Hide();
        }
        private void MainBtn_Click(object sender, EventArgs e)
        {
            if (progressBar.Started) return;
            switch (mainButton.ButtonStatus)
            {
                case MainButtonStatus.Play:
                    Play();
                    break;
                case MainButtonStatus.Update:
                    UpdateClient();
                    break;
                case MainButtonStatus.Retry:
                    CheckClientUpdates();
                    break;
            }
        }
        private async void Play()
        {
            if (!File.Exists($"{Settings.PathToSkyrim}\\skse64_loader.exe"))
            {
                Wind_Loaded(null, null);
                return;
            }

            if (serverList.SelectedItem == null) return;
            SetServer();
            ServerModel server = (ServerModel)serverList.SelectedItem;
            SetSession(await Account.GetSession(server.Address));
            SetMods();

            try
            {
                Hide();
                bool crash = await GameLauncher.StartGame();
                Show();

                if (crash)
                {
                    YandexMetrica.ReportEvent("CrashDetected");
                    await Task.Delay(500);
                    await ReportDmp();
                }
            }
            catch
            {
                YandexMetrica.ReportEvent("HasNotAccess");
                Close();
            }
        }

        private void SetMods()
        {
            string path = Settings.PathToLocalSkyrim + "Plugins.txt";
            string content = @"# This file is used by Skyrim to keep track of your downloaded content.
# Please do not modify this file.
*FarmSystem.esp";

            if (!Directory.Exists(Settings.PathToLocalSkyrim)) Directory.CreateDirectory(Settings.PathToLocalSkyrim);
            if (File.Exists(path)) File.SetAttributes(path, FileAttributes.Normal);

            try
            {
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
        }

        private void SetServer()
        {
            if (serverList.SelectedItem == null) return;
            SkympClientSettingsModel oldServer = JsonConvert.DeserializeObject<SkympClientSettingsModel>(File.ReadAllText(Settings.PathToSkympClientSettings));
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

        private async Task ReportDmp()
        {
            string pathToDmps = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\My Games\Skyrim Special Edition\SKSE\Crashdumps\";
            if (!Directory.Exists(pathToDmps)) return;
            try
            {
                DateTime dt = ModVersion.LastDmpReported;
                string fileName = "";
                foreach (FileSystemInfo fileSI in new DirectoryInfo(pathToDmps).GetFileSystemInfos())
                {
                    if (fileSI.Extension == ".dmp")
                    {
                        if (dt < Convert.ToDateTime(fileSI.CreationTime))
                        {
                            dt = Convert.ToDateTime(fileSI.CreationTime);
                            fileName = fileSI.Name;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (await Net.ReportDmp(pathToDmps + fileName))
                        YandexMetrica.ReportEvent("CrashReported");
                    else YandexMetrica.ReportEvent("CantReport");
                    ModVersion.LastDmpReported = dt;
                    ModVersion.Save();

                    await Task.Delay(3000);
                    File.Delete(pathToDmps + fileName);
                }
            }
            catch (Exception e)
            {
                Logger.Error("ReportDmp", e);
            }
        }
        private async void UpdateClient()
        {
            (string, string) url = await Net.GetUrlToClient();
            string destinationPath = $"{Settings.PathToSkyrimTmp}client.zip";

            bool ok = await DownloadFile(destinationPath, url.Item1, "Загрузка клиента", url.Item2);

            if (ok)
            {
                progressBar.Show(true, "Распаковка клиента");
                try
                {
                    if (await Task.Run(() => Unpacker.UnpackArchive(destinationPath, Settings.PathToSkyrim, "client")))
                    {
                        ModVersion.Version = url.Item2;
                        ModVersion.Save();
                        NotifyController.Show(PopupNotify.Normal, "Установка завершена", "Приятной игры!");
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("Extract", e);
                    NotifyController.Show(e);
                    mainButton.ButtonStatus = MainButtonStatus.Retry;
                    return;
                }
                progressBar.Hide();
            }
            CheckClientUpdates();
        }
        private void Downloader_DownloadChanged(long downloaded, long size, double prDown)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate
            {
                try
                {
                    progressBar.Size = size;
                    progressBar.Update(downloaded);
                }
                catch { }
            });
        }
        private async Task<bool> DownloadFile(string destinationPath, string url, string status, string vers = null, int c = 0)
        {
            progressBar.Show(false, $"{status}{(c != 0 ? $" (Попытка №{c})" : "")}", vers);

            Downloader downloader = new Downloader(destinationPath, url);
            downloader.DownloadChanged += Downloader_DownloadChanged;
            progressBar.Start();
            bool ok = await downloader.StartSync();
            progressBar.Stop();
            progressBar.Hide();

            if (!ok && c < 3) return await DownloadFile(destinationPath, url, status, vers, ++c);
            return ok;
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
            Settings.UserToken = "";
            Settings.Save();

            authorization.Visibility = Visibility.Visible;
        }
    }
}
