using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using UpdatesClient.Modules.SelfUpdater;
using Yandex.Metrica;
using SplashScreen = UpdatesClient.Modules.SelfUpdater.SplashScreen;

namespace UpdatesClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    internal delegate void Invoker();
    public partial class App : Application
    {
        private const string BeginUpdate = "begin";
        private const string EndUpdate = "end";

        internal delegate void ApplicationInitializeDelegate(SplashScreen splashWindow);
        internal ApplicationInitializeDelegate ApplicationInitialize;

        private SplashScreen SplashWindow;

        private string MasterHash;

        private readonly string FullPathToSelfExe = Assembly.GetExecutingAssembly().Location;
        private readonly string NameExeFile = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);

        public static new App Current { get { return Application.Current as App; } }

        public App()
        {
            if (!Security.CheckEnvironment()) { ExitApp(); return; }
            if (!HandleCmdArgs()) { ExitApp(); return; }

            string tmpPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\UpdatesClient\\tmp";
            if(!Directory.Exists(tmpPath)) Directory.CreateDirectory(tmpPath);
            YandexMetricaFolder.SetCurrent(tmpPath);

            YandexMetrica.Config.CustomAppVersion = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
            YandexMetrica.Activate("3cb6204a-2b9c-4a7c-9ea5-f177e78a4657");

            InitApp();
        }

        private bool HandleCmdArgs()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                try
                {
                    switch (args[1])
                    {
                        case EndUpdate:
                            File.Delete($"{args[2]}.update.exe");
                            break;
                        case BeginUpdate:
                            File.Copy(FullPathToSelfExe, $"{args[2]}.exe", true);
                            Process.Start($"{args[2]}.exe", $"{EndUpdate} {args[2]}");
                            ExitApp();
                            return false;
                        default:
                            ExitApp();
                            return false;
                    }
                }
                catch { }
            }
            return true;
        }

        private void InitApp()
        {
            ApplicationInitialize = ApplicationInit;
        }

        private void ExitApp()
        {
            Application.Current.Shutdown();
        }

        private async void ApplicationInit(SplashScreen splashWindow)
        {
            try
            {
#if (DEBUG || DeR)
                Thread.Sleep(5); //Без этого может не работать
                StartLuancher();
#else
                SplashWindow = splashWindow;
                SplashWindow.SetStatus("Проверка обновления лаунчера");

                MasterHash = await Updater.GetLauncherHash();

                if (MasterHash == null || MasterHash == "") throw new Exception("Hash is empty");
                if (NameExeFile == null || NameExeFile == "") throw new Exception("Path is empty");

                if (!CheckFile(FullPathToSelfExe))
                {
                    SplashWindow.SetStatus("Обновление лаунчера");
                    SplashWindow.SetProgressMode(false);
                    bool downloaded = Update();

                    if (downloaded && CheckFile($"{NameExeFile}.update.exe"))
                    {
                        Process p = new Process();
                        p.StartInfo.FileName = $"{NameExeFile}.update.exe";
                        p.StartInfo.Arguments = $"{BeginUpdate} {NameExeFile}";
                        p.Start();
                    }
                    else
                    {
                        SplashWindow.SetStatus("Не удалось выполнить обновление лаунчера");
                        Thread.Sleep(1500);
                    }
                }
                else
                {
                    SplashWindow.SetStatus("Готово");
                    SplashWindow.SetProgressMode(false);
                    StartLuancher();
                }
#endif
            }
            catch (Exception e) 
            {
                YandexMetrica.ReportError($"CriticalError_{Security.UID}", e);
                MessageBox.Show($"Сведения: {e.Message}\nВаш идентификатор: {Security.UID}", "Критическая ошибка"); 
            }
        }
        private void StartLuancher()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate
            {
                MainWindow = new MainWindow();
                MainWindow.Show();
            });
        }
        //****************************************************************//
        private bool CheckFile(string pathToFile)
        {
            if (File.Exists(pathToFile) && MasterHash.ToUpper() == Hashing.GetMD5FromFile(File.OpenRead(pathToFile)).ToUpper()) return true;
            else return false;
        }
        private bool Update() 
        {
            if (File.Exists($"{NameExeFile}.update.exe")) File.Delete($"{NameExeFile}.update.exe");

            Downloader downloader = new Downloader(Updater.AddressToLauncher + Updater.LauncherName, $"{NameExeFile}.update.exe");
            downloader.DownloadChanged += SplashWindow.SetProgress;
            return downloader.Download();
        }
    }
}
