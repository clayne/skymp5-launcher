using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using UpdatesClient.Core;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.SelfUpdater;
using Yandex.Metrica;
using Downloader = UpdatesClient.Modules.SelfUpdater.Downloader;
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
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Version version = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
            Logger.Init(version);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (!Modules.SelfUpdater.Security.CheckEnvironment()) { ExitApp(); return; }
            if (!HandleCmdArgs()) { ExitApp(); return; }

            InitApp();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.FatalError("UnhandledException", (Exception)e?.ExceptionObject);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string[] par = args.Name.Replace(" ", "").Split(',');
            string newName = par[0].Replace(".", "_");
            if (newName.EndsWith("_resources")) return null;
            try
            {
                byte[] bytes = (byte[])UpdatesClient.Properties.Resources.ResourceManager.GetObject(newName);
                return Assembly.Load(bytes);
            }
            catch { }
            return null;
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
                            try
                            {
                                File.SetAttributes($"{args[2]}.exe", FileAttributes.Normal);
                            }
                            catch (Exception e)
                            {
                                YandexMetrica.ReportError("HandleCmdArgs_Normal", e);
                                Logger.Error("HandleCmdArgs_Normal", e);
                            }
                            Process.Start($"{args[2]}.exe", $"{EndUpdate} {args[2]}");
                            ExitApp();
                            return false;
                        case "repair":
                            Settings.Reset();
                            break;
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
                YandexMetrica.Activate("3cb6204a-2b9c-4a7c-9ea5-f177e78a4657");
                YandexMetrica.ReportError($"CriticalError_{Modules.SelfUpdater.Security.UID}", e);
                Logger.Error($"CriticalError_{Modules.SelfUpdater.Security.UID}", e);
                MessageBox.Show($"Сведения: {e.Message}\nВаш идентификатор: {Modules.SelfUpdater.Security.UID}", "Критическая ошибка"); 
            }
        }
        private void StartLuancher()
        {
            if (File.Exists($"{NameExeFile}.update.exe")) File.Delete($"{NameExeFile}.update.exe");

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate
            {
                YandexMetrica.Activate("3cb6204a-2b9c-4a7c-9ea5-f177e78a4657");
                MainWindow = new MainWindow();
                MainWindow.Show();
            });
        }
        //****************************************************************//
        private bool CheckFile(string pathToFile)
        {
            return File.Exists(pathToFile) 
                && MasterHash.ToUpper().Trim() == Hashing.GetMD5FromFile(File.OpenRead(pathToFile)).ToUpper().Trim();
        }
        private bool Update() 
        {
            if (File.Exists($"{NameExeFile}.update.exe")) File.Delete($"{NameExeFile}.update.exe");

            Downloader downloader = new Downloader(Updater.AddressToLauncher + Updater.LauncherName, $"{NameExeFile}.update.exe")
            {
                IsHidden = true
            };
            downloader.DownloadChanged += SplashWindow.SetProgress;
            return downloader.Download();
        }
    }
}
