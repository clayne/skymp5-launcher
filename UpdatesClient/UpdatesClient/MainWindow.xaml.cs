using BlendModeEffectLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using UpdatesClient.Core;
using UpdatesClient.Core.Models;
using UpdatesClient.Core.Models.ServerManifest;
using UpdatesClient.Core.Network;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.Configs.Helpers;
using UpdatesClient.Modules.GameManager;
using UpdatesClient.Modules.GameManager.AntiCheat;
using UpdatesClient.Modules.GameManager.Helpers;
using UpdatesClient.Modules.GameManager.Model;
using UpdatesClient.Modules.ModsManager;
using UpdatesClient.UI.Controllers;
using Yandex.Metrica;
using Res = UpdatesClient.Properties.Resources;

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

        private bool blockMainBtn = false;

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
            await CheckClientUpdates();
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

            Mods.Init();

            if(ExperimentalFunctions.HasExperimentalFunctions())
            {
                if (!Mods.ExistMod("SkyMPCore"))
                {
                    MessageBox.Show("All files will be reinstalled", "Attention",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
                    GameCleaner.Clear();
                }

                await CheckGameNew();
            }
            else
            {
                await CheckGameOld();
            }

            SetBackgroundServerList();
            FillServerList();
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
        private async Task CheckGameOld()
        {
            ResultGameVerification result = CheckSkyrim();

            ModVersion.Load();
            FileWatcher.Init();

            if (!result.IsSKSEFound && MessageBox.Show(Res.SKSENotFound, Res.Warning, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                blockMainBtn = true;
                await InstallSKSE();
                blockMainBtn = false;
            }

            try
            {
                if (!result.IsRuFixConsoleFound && ModVersion.HasRuFixConsole == null
                    && MessageBox.Show(Res.SSERFix, Res.Warning, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    blockMainBtn = true;
                    await InstallRuFixConsole();
                    ModVersion.Save();
                    blockMainBtn = false;
                }
                else
                {
                    ModVersion.HasRuFixConsole = result.IsRuFixConsoleFound;
                    ModVersion.Save();
                }
            }
            catch (Exception er)
            {
                blockMainBtn = false;
                Logger.FatalError("CheckGame_SSERFix", er);
            }
        }

        private async Task CheckGameNew() 
        {
            ResultGameVerification result = CheckSkyrim();

            ModVersion.Load();
            FileWatcher.Init();

            if (!Mods.ExistMod("SKSE") && !result.IsSKSEFound && MessageBox.Show(Res.SKSENotFound, Res.Warning, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                blockMainBtn = true;
                await GetSKSE();
                Mods.EnableMod("SKSE");
                blockMainBtn = false;
            }
            else if(Mods.ExistMod("SKSE"))
            {
                Mods.EnableMod("SKSE");
            }

            try
            {
                if (!result.IsRuFixConsoleFound && ModVersion.HasRuFixConsole == null
                    && MessageBox.Show(Res.SSERFix, Res.Warning, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    blockMainBtn = true;
                    if (!Mods.ExistMod("RuFixConsole")) await GetRuFixConsole();
                    Mods.EnableMod("RuFixConsole");

                    ModVersion.HasRuFixConsole = true;
                    ModVersion.Save();

                    blockMainBtn = false;
                }
                else
                {
                    ModVersion.HasRuFixConsole = result.IsRuFixConsoleFound;
                    ModVersion.Save();
                }
            }
            catch (Exception er)
            {
                blockMainBtn = false;
                Logger.FatalError("CheckGameNew_SSERFix", er);
            }
        }

        private void SetBackgroundServerList()
        {
            try
            {
                serverListBg.Effect = new OverlayEffect()
                {
                    BInput = GetGridBackGround(serverList)
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
        private async Task InstallSKSE()
        {
            try
            {
                string url = await Net.GetUrlToSKSE();
                string destinationPath = $@"{Settings.PathToSkyrimTmp}{url.Substring(url.LastIndexOf('/'), url.Length - url.LastIndexOf('/'))}";

                bool ok = await DownloadFile(destinationPath, url, Res.DownloadingSKSE);

                if (ok)
                {
                    progressBar.Show(true, Res.ExtractingSKSE);
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
            catch (Exception e)
            {
                Logger.Error("InstallSKSE", e);
            }
        }
        private async Task InstallRuFixConsole()
        {
            try
            {
                string url = Net.URL_Mod_RuFix;
                string destinationPath = $@"{Settings.PathToSkyrimTmp}{url.Substring(url.LastIndexOf('/'), url.Length - url.LastIndexOf('/'))}";

                bool ok = await DownloadFile(destinationPath, url, Res.DownloadingSSERuFixConsole);
                if (ok)
                {
                    try
                    {
                        progressBar.Show(true, Res.Extracting);
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
            catch (Exception e)
            {
                Logger.Error("InstallRuFixConsole", e);
            }
        }
        private async Task CheckClientUpdates()
        {
            progressBar.Show(true, Res.CheckingUpdates);
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
                    if (ExperimentalFunctions.HasExperimentalFunctions()) await UpdateClientNew();
                    else await UpdateClientOld();
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
                await Task.Run(() => ExperimentalFunctions.IfUse("ModsInit", () => CheckGameNew().Wait(), () => CheckGameOld().Wait()));
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

        private async Task<ServerModsManifest> GetManifest(string adress)
        {
            string serverManifest = await Net.Request($"http://{adress}/manifest.json", "GET", false, null);
            return JsonConvert.DeserializeObject<ServerModsManifest>(serverManifest);
        }
        private Dictionary<string, List<(string, uint)>> GetMods(ServerModsManifest modsManifest)
        {
            List<string> WhiteList = new List<string>();
            WhiteList.Add("Skyrim");
            WhiteList.Add("Update");
            WhiteList.Add("Dawnguard");
            WhiteList.Add("HearthFires");
            WhiteList.Add("Dragonborn");

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
            string path = Settings.PathToLocalSkyrim + "Plugins.txt";
            string content = "";
#if (DEBUG)
            bool d = true;
#else
            bool d = false;
#endif

            if (d || (NetworkSettings.EnableModLoader && ExperimentalFunctions.HasExperimentalFunctions()))
            {
                try
                {
                    Mods.DisableAll();
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
                            Mods.AddMod(mod.Key, "", tmpPath, mainFile);
                        }
                        Mods.EnableMod(Path.GetFileNameWithoutExtension(mod.Key));
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
                        NotifyController.Show(PopupNotify.Normal, Res.Attempt, "Вероятно целевой сервер устер, используется режим совместимости");
                        if (Mods.ExistMod("Farm"))
                            Mods.OldModeEnable();
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
            }
            else
            {
                content = @"*FarmSystem.esp";
            }
            
            try
            {
                if (!Directory.Exists(Settings.PathToLocalSkyrim)) Directory.CreateDirectory(Settings.PathToLocalSkyrim);
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
            await DownloadFile(destinationPath, url, $"Загрузка {file}");
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
                oldServer = new SkympClientSettingsModel();
                oldServer.IsEnableConsole = false;
                oldServer.IsShowMe = false;
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
                if (!(e is WebException) && (e is SocketException))
                    Logger.Error("ReportDmp", e);
            }
        }

        private async Task GetSKSE()
        {
            try
            {
                string url = await Net.GetUrlToSKSE();
                string destinationPath = $@"{Settings.PathToSkyrimTmp}{url.Substring(url.LastIndexOf('/'), url.Length - url.LastIndexOf('/'))}";

                bool ok = await DownloadFile(destinationPath, url, Res.DownloadingSKSE);

                if (ok)
                {
                    progressBar.Show(true, Res.ExtractingSKSE);
                    try
                    {
                        string path = Mods.GetTmpPath();
                        await Task.Run(() => Unpacker.UnpackArchive(destinationPath, path, Path.GetFileNameWithoutExtension(destinationPath)));
                        Mods.AddMod("SKSE", "SKSEHash", path);
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
            catch (Exception e)
            {
                Logger.Error("InstallSKSE", e);
            }
        }
        private async Task GetRuFixConsole()
        {
            try
            {
                string url = Net.URL_Mod_RuFix;
                string destinationPath = $@"{Settings.PathToSkyrimTmp}{url.Substring(url.LastIndexOf('/'), url.Length - url.LastIndexOf('/'))}";

                bool ok = await DownloadFile(destinationPath, url, Res.DownloadingSSERuFixConsole);
                if (ok)
                {
                    try
                    {
                        string path = Mods.GetTmpPath();
                        progressBar.Show(true, Res.Extracting);
                        await Task.Run(() => Unpacker.UnpackArchive(destinationPath, path + "\\Data"));
                        Mods.AddMod("RuFixConsole", "RuFixConsoleHash", path);
                        progressBar.Hide();
                    }
                    catch (Exception e)
                    {
                        Logger.Error("ExtractRuFix", e);
                        NotifyController.Show(e);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("InstallRuFixConsole", e);
            }
        }
        
        private async Task UpdateClientOld()
        {
            (string, string) url = await Net.GetUrlToClient();
            string destinationPath = $"{Settings.PathToSkyrimTmp}client.zip";

            try
            {
                if (File.Exists(destinationPath)) File.Delete(destinationPath);
            }
            catch (Exception e)
            {
                Logger.Error("DelClientZip", e);
            }
            
            bool ok = await DownloadFile(destinationPath, url.Item1, Res.DownloadingClient, url.Item2);

            if (ok)
            {
                progressBar.Show(true, Res.ExtractingClient);
                try
                {
                    if (await Task.Run(() => Unpacker.UnpackArchive(destinationPath, Settings.PathToSkyrim, "client")))
                    {
                        ModVersion.Version = url.Item2;
                        ModVersion.Save();
                        NotifyController.Show(PopupNotify.Normal, Res.InstallationCompleted, Res.HaveAGG);
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
            await CheckClientUpdates();
        }

        private async Task UpdateClientNew()
        {
            (string, string) url = await Net.GetUrlToClient();
            string destinationPath = $"{Settings.PathToSkyrimTmp}client.zip";

            try
            {
                if (File.Exists(destinationPath)) File.Delete(destinationPath);
            }
            catch (Exception e)
            {
                Logger.Error("DelClientZip", e);
            }

            bool ok = await DownloadFile(destinationPath, url.Item1, Res.DownloadingClient, url.Item2);

            if (ok)
            {
                progressBar.Show(true, Res.ExtractingClient);
                try
                {
                    string path = Mods.GetTmpPath();
                    if (await Task.Run(() => Unpacker.UnpackArchive(destinationPath, path, "client")))
                    {
                        Mods.AddMod("SkyMPCore", url.Item2, path);
                        Mods.EnableMod("SkyMPCore");

                        ModVersion.Version = url.Item2;
                        ModVersion.Save();
                        NotifyController.Show(PopupNotify.Normal, Res.InstallationCompleted, Res.HaveAGG);
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
            await CheckClientUpdates();
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
            progressBar.Show(false, $"{status}{(c != 0 ? $" ({Res.Attempt} №{c + 1})" : "")}", vers);

            Downloader downloader = new Downloader(destinationPath, url);
            downloader.DownloadChanged += Downloader_DownloadChanged;
            progressBar.Start();
            bool ok = await downloader.StartSync();
            downloader.DownloadChanged -= Downloader_DownloadChanged;
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
            Settings.UserName = "";
            Settings.UserToken = "";
            Settings.Save();

            authorization.Visibility = Visibility.Visible;
        }
    }
}
