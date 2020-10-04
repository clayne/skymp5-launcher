using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using UpdatesClient.Core;
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

            try
            {
                Account.VerifyToken();
                authorization.Visibility = Visibility.Collapsed;
            }
            catch
            {
                authorization.Visibility = Visibility.Visible;
            }
            authorization.Visibility = Visibility.Visible;
            authorization.SignIn += Authorization_SignIn;

            Settings.Load();
            wind.Loaded += Wind_Loaded; ;
        }

        private void Authorization_SignIn()
        {
            authorization.Visibility = Visibility.Collapsed;
            MessageBox.Show("Worked!");
        }

        private async void Wind_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(Settings.PathToLocalTmp + "7z.dll")) await Download7zLib();

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

            CheckClientUpdates();
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
            if (File.Exists(Settings.PathToLocalTmp + "7z.dll") || await Download7zLib())
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
                        YandexMetrica.ReportError("ExtractSKSE", e);
                        NotifyController.Show(e);
                        mainButton.ButtonStatus = MainButtonStatus.Retry;
                    }
                    progressBar.Hide();
                }
            }
        }
        private Task<bool> Download7zLib()
        {
            string url = Net.URL_Lib;
            string destinationPath = $"{Settings.PathToLocalTmp}{url.Substring(url.LastIndexOf('/'), url.Length - url.LastIndexOf('/'))}";

            return DownloadFile(destinationPath, url, "Загрузка библиотеки");
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
                    YandexMetrica.ReportError("ExtractRuFix", e);
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
                YandexMetrica.ReportError("CheckClient", e);
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
        private async Task ReportDmp()
        {
            string pathToDmps = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\My Games\Skyrim Special Edition\SKSE\Crashdumps\";

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
                YandexMetrica.ReportError("ReportDmp", e);
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
                    YandexMetrica.ReportError("Extract", e);
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
    }
}
