using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using UpdatesClient.Core;
using UpdatesClient.Core.Enums;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.GameManager;
using UpdatesClient.Modules.GameManager.AntiCheat;
using UpdatesClient.Modules.GameManager.Model;
using Yandex.Metrica;
using static UpdatesClient.UI.Controllers.ProgressBarBtn;

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

        private readonly BrushConverter converter = new BrushConverter();
        private BtnAction BtnAction = BtnAction.Check;

        public MainWindow()
        {
            InitializeComponent();
            TitleWindow.MouseLeftButtonDown += (s, e) => DragMove();

            Settings.Load();
            wind.Loaded += delegate
            {
                if (string.IsNullOrEmpty(Settings.PathToSkyrim))
                {
                    SetGameFolder();
                }

                ModVersion.Load();
                FileWatcher.Init();

                if (ModVersion.HasRuFixConsole == null) CheckRuFixConsole();
                else if (BtnAction != BtnAction.InstallSKSE) CheckClient();
            };
        }

        private void CheckRuFixConsole()
        {
            ResultGameVerification result = GameVerification.VerifyGame(Settings.PathToSkyrim, null);
            if (!result.IsRuFixConsoleFound)
            {
                if (MessageBox.Show("SSE Rusian Fix Console не обнаружен, установить?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    InstallRuFixConsole();
                    return;
                }
                else
                {
                    ModVersion.HasRuFixConsole = false;
                }
            }
            else
            {
                ModVersion.HasRuFixConsole = true;
            }
            ModVersion.Save();
            CheckClient();
        }

        private async void InstallRuFixConsole()
        {
            MainBtn.SetAsProgressBar(ColorType.Warning, false);

            string req = Net.URL_Mod_RuFix;
            Downloader downloader = new Downloader($@"{Settings.PathToSkyrim}\tmp\{req.Substring(req.LastIndexOf('/'), req.Length - req.LastIndexOf('/'))}", req, "0.0.0.0");
            downloader.DownloadChanged += Downloader_DownloadChanged;
            if (await downloader.StartSync())
            {
                try
                {
                    MainBtn.SetAsProgressBar(ColorType.Normal, true, "Распаковка");

                    ModVersion.HasRuFixConsole = await Task.Run(() => Unpacker.UnpackZip($@"{Settings.PathToSkyrim}\tmp\{req.Substring(req.LastIndexOf('/'), req.Length - req.LastIndexOf('/'))}", Settings.PathToSkyrim + "\\Data"));
                    ModVersion.Save();
                }
                catch (Exception e)
                {
                    YandexMetrica.ReportError("ExtractRuFix", e);
                    await MainBtn.ShowMessage(ColorType.Error, "Ошибка");
                }
                CheckClient();
            }
            else
            {
                await MainBtn.ShowMessage(ColorType.Error, "Ошибка");
                InstallRuFixConsole();
            }
        }

        private void SetGameFolder()
        {
            using (System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Выберите папку с TES: Skyrim SE";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ResultGameVerification result = GameVerification.VerifyGame(dialog.SelectedPath, null);

                    if (!result.IsGameFound)
                    {
                        MessageBox.Show("Skyrim SE не обнаружен", "Ошибка");
                        SetGameFolder();
                    }
                    else if (!result.IsSKSEFound)
                    {
                        if (MessageBox.Show("SKSE не обнаружен, установить?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            Settings.PathToSkyrim = dialog.SelectedPath;
                            Settings.Save();
                            BtnAction = BtnAction.InstallSKSE;
                            DownloadLib();
                        }
                        else
                        {
                            SetGameFolder();
                        }
                    }
                    else
                    {
                        Settings.PathToSkyrim = dialog.SelectedPath;
                        Settings.Save();
                    }
                }
                else { Application.Current.Shutdown(); }
            }
        }

        private async void CheckClient()
        {
            MainBtn.SetAsProgressBar(ColorType.Normal, true, "Проверка");
            try
            {
                if (await Net.UpdateAvailable())
                {
                    MainBtn.SetAsButton(ColorType.Normal, "Обновить");
                    BtnAction = BtnAction.Update;
                }
                else
                {
                    MainBtn.SetAsButton(ColorType.Normal, "Играть");
                    BtnAction = BtnAction.Play;
                }
            }
            catch (Exception e)
            {
                YandexMetrica.ReportError("CheckClient", e);
                await MainBtn.ShowMessage(ColorType.Error, "Ошибка");
                MainBtn.SetAsButton(ColorType.Normal, "Повторить");
            }
        }

        private void DownloadLib()
        {
            if (!File.Exists(Settings.PathToSkyrim + "\\tmp\\7z.dll"))
            {
                MainBtn.SetAsProgressBar(ColorType.Warning, false);

                string req = Net.URL_Lib;
                Downloader downloader = new Downloader($@"{Settings.PathToSkyrim}\tmp\{req.Substring(req.LastIndexOf('/'), req.Length - req.LastIndexOf('/'))}", req, "0.0.0.0");
                downloader.DownloadChanged += Downloader_DownloadChanged;
                downloader.DownloadComplete += Downloader_DownloadLibComplete;
                downloader.StartAsync();
            }
            else
            {
                InstallSKSE();
            }
        }
        private async void Downloader_DownloadLibComplete(string DestinationFile, string Vers)
        {
            if (DestinationFile == null)
            {
                await MainBtn.ShowMessage(ColorType.Error, "Ошибка");
                DownloadLib();
            }
            else
            {
                InstallSKSE();
            }
        }
        private async void InstallSKSE()
        {
            MainBtn.SetAsProgressBar(ColorType.Warning, false);

            string req = await Net.GetUrlToSKSE();
            Downloader downloader = new Downloader($@"{Settings.PathToSkyrim}\tmp\{req.Substring(req.LastIndexOf('/'), req.Length - req.LastIndexOf('/'))}", req, "0.0.0.0");
            downloader.DownloadChanged += Downloader_DownloadChanged;
            downloader.DownloadComplete += Downloader_DownloadSKSEComplete;
            downloader.StartAsync();
        }

        private async void Downloader_DownloadSKSEComplete(string DestinationFile, string Vers)
        {
            if (DestinationFile == null)
            {
                await MainBtn.ShowMessage(ColorType.Error, "Ошибка");
                MainBtn.SetAsButton(ColorType.Normal, "Повторить");
            }
            else
            {
                ExtractSKSE(DestinationFile);
            }
        }
        private async void ExtractSKSE(string file)
        {
            try
            {
                MainBtn.SetAsProgressBar(ColorType.Normal, true, "Распаковка");

                if (await Task.Run(() => Unpacker.SevenZUnpack(file, Settings.PathToSkyrim)))
                {
                    MainBtn.ColorBar = (Brush)converter.ConvertFrom("#FF04D9FF");
                    CheckClient();
                }
                else
                {
                    MainBtn.SetAsButton(ColorType.Normal, "Повторить");
                    BtnAction = BtnAction.InstallSKSE;
                }
            }
            catch (Exception e)
            {
                YandexMetrica.ReportError("ExtractSKSE", e);
                await MainBtn.ShowMessage(ColorType.Error, "Ошибка");
                MainBtn.SetAsButton(ColorType.Normal, "Повторить");
            }
        }

        private void ImageButton_Click(object sender, EventArgs e)
        {
            YandexMetrica.Config.CrashTracking = false;
            Application.Current.Shutdown();
        }

        private void MainBtn_Click(object sender, EventArgs e)
        {
            switch (BtnAction)
            {
                case BtnAction.Play:
                    Play();
                    break;
                case BtnAction.Update:
                    Update();
                    break;
                case BtnAction.Check:
                    CheckClient();
                    break;
                case BtnAction.InstallSKSE:
                    InstallSKSE();
                    break;
            }
        }

        private async void Update()
        {
            MainBtn.SetAsProgressBar(ColorType.Normal, false);

            (string, string) req = await Net.GetUrlToClient();
            Downloader downloader = new Downloader($@"{Settings.PathToSkyrim}\tmp\client.zip", req.Item1, req.Item2);
            downloader.DownloadChanged += Downloader_DownloadChanged;
            downloader.DownloadComplete += Downloader_DownloadComplete;
            downloader.StartAsync();
        }

        private async void Downloader_DownloadComplete(string DestinationFile, string Vers)
        {
            if (DestinationFile == null)
            {
                await MainBtn.ShowMessage(ColorType.Error, "Ошибка");
                MainBtn.SetAsButton(ColorType.Normal, "Повторить");
            }
            else
            {
                Extract(DestinationFile, Vers);
            }
        }

        private async void Extract(string file, string vers)
        {
            try
            {
                MainBtn.SetAsProgressBar(ColorType.Normal, true, "Распаковка");

                if (await Task.Run(() => Unpacker.Unpack(file, Settings.PathToSkyrim)))
                {
                    ModVersion.Version = vers;
                    ModVersion.Save();
                }
                CheckClient();
            }
            catch (Exception e)
            {
                YandexMetrica.ReportError("Extract", e);
                await MainBtn.ShowMessage(ColorType.Error, "Ошибка");
                MainBtn.SetAsButton(ColorType.Normal, "Повторить");
            }
        }

        private void Downloader_DownloadChanged(double Value, double LenFile, double prDown)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate
            {
                try
                {
                    MainBtn.Value = prDown;
                    MainBtn.StatusText = $"{prDown:0}%";
                }
                catch { }
            });
        }

        private async void Play()
        {
            if (!File.Exists($"{Settings.PathToSkyrim}\\skse64_loader.exe"))
            {
                if (MessageBox.Show("SKSE не обнаружен, установить?", "Ошибка", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    InstallSKSE();
                return;
            }

            try
            {
                Hide();
                await GameLauncher.StartGame();
                Show();
            }
            catch
            {
                YandexMetrica.ReportEvent("HasNotAccess");
                Close();
            }
        }
    }
}
