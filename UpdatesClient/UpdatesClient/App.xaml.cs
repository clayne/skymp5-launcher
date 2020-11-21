using SingleInstanceApp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using UpdatesClient.Core;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.SelfUpdater;
using Yandex.Metrica;
using Downloader = UpdatesClient.Modules.SelfUpdater.Downloader;
using SplashScreen = UpdatesClient.Modules.SelfUpdater.SplashScreen;
using Res = UpdatesClient.Properties.Resources;
using System.Globalization;

namespace UpdatesClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    internal delegate void Invoker();
    public partial class App : Application, ISingleInstance
    {
        private const string Unique = "{F627FE18-0573-4F39-A2BF-8A564F10FC30}";
        private const string BeginUpdate = "begin";
        private const string EndUpdate = "end";

        internal delegate void ApplicationInitializeDelegate(SplashScreen splashWindow);
        internal ApplicationInitializeDelegate ApplicationInitialize;

        private SplashScreen SplashWindow;

        private string MasterHash;

        private readonly string FullPathToSelfExe = Assembly.GetExecutingAssembly().Location;
        private readonly string NameExeFile = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);

        public static new App Current { get { return Application.Current as App; } }
        public static Application AppCurrent { get; private set; }

        private readonly bool mainInstance = false;

        public App()
        {
            try
            {
                AppCurrent = Current;
            } catch { }
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Version version = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
            Settings.Load();
            Logger.Init(version);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            UnpackResx();

            if (!Modules.SelfUpdater.Security.CheckEnvironment()) { ExitApp(); return; }
            if (!HandleCmdArgs()) { ExitApp(); return; }

            try
            {
                if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
                {
                    mainInstance = true;
                    InitApp();
                }
                else
                {
                    ExitApp();
                }
            }
            catch (Exception e)
            {
                Logger.FatalError("App", e);
            }
        }

        private void UnpackResx()
        {
            string[] locales = { "ru-RU" };
            foreach(string l in locales)
            {
                try
                {
                    string path = $"{Settings.PathToLocal}\\{l}";
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    if (!File.Exists($"{path}\\UpdatesClient.resources.dll"))
                    {
                        byte[] bytes = (byte[])Res.ResourceManager.GetObject($"UpdatesClient_{l}_resources");
                        File.WriteAllBytes($"{path}\\UpdatesClient.resources.dll", bytes);
                    }
                }
                catch (UnauthorizedAccessException uae)
                {
                    FileAttributes attributes = FileAttributes.Normal;
                    try
                    {
                        attributes = File.GetAttributes($"{Settings.PathToLocal}\\{l}\\UpdatesClient.resources.dll");
                    } catch { }
                    Logger.Error($"UnpackResx_{l}_{attributes}", uae);
                }
                catch (Exception e)
                {
                    Logger.Error($"UnpackResx_{l}", e);
                }
            }
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            try
            {
                if (Current?.MainWindow?.WindowState == WindowState.Minimized) Current.MainWindow.WindowState = WindowState.Normal;
                Current?.MainWindow?.Show();
                Current?.MainWindow?.Activate();
            }
            catch (Exception e)
            {
                Logger.Error("SignalExternal", e);
            }
            return true;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (mainInstance) SingleInstance<App>.Cleanup();
            base.OnExit(e);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.FatalError("UnhandledException", (Exception)e?.ExceptionObject);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName MissingAssembly = new AssemblyName(args.Name);
            CultureInfo ci = MissingAssembly.CultureInfo;

            string[] par = args.Name.Replace(" ", "").Split(',');
            string newName = par[0].Replace(".", "_");
            if (newName.EndsWith("_resources"))
            {
                if (Directory.Exists($"{Settings.PathToLocal}{ci.Name}\\") && File.Exists($"{Settings.PathToLocal}{ci.Name}\\UpdatesClient.resources.dll"))
                {
                    return Assembly.LoadFile($"{Settings.PathToLocal}{ci.Name}\\UpdatesClient.resources.dll");
                }
                else
                {
                    return null;
                }
            }
            else
            {
                try
                {
                    byte[] bytes = (byte[])Res.ResourceManager.GetObject(newName);
                    return Assembly.Load(bytes);
                }
                catch { }
            }
            return null;
        }

        private bool HandleCmdArgs()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                bool eUpdate = false;
                try
                {
                    switch (args[1])
                    {
                        case EndUpdate:
                            eUpdate = true;
                            Thread.Sleep(250);
                            File.Delete($"{args[2]}.update.exe");
                            break;
                        case BeginUpdate:
                            Thread.Sleep(250);
                            File.Copy(FullPathToSelfExe, $"{args[2]}.exe", true);
                            File.SetAttributes($"{args[2]}.exe", FileAttributes.Normal);
                            Process.Start($"{args[2]}.exe", $"{EndUpdate} {args[2]}");
                            goto default;
                        case "repair":
                            Settings.Reset();
                            break;
                        default:
                            ExitApp();
                            return false;
                    }
                }
                catch (UnauthorizedAccessException e)
                {
                    if (!eUpdate)
                    {
                        MessageBox.Show($"{Res.ErrorEndSelfUpdate}\n{e.Message}", $"{Res.Error}");
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("HandleCmdArgs", e);
                    return false;
                }
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
                SplashWindow.SetStatus($"{Res.CheckSelfUpdate}");

                MasterHash = await Updater.GetLauncherHash();

                if (MasterHash == null || MasterHash == "") throw new Exception("Hash is empty");
                if (NameExeFile == null || NameExeFile == "") throw new Exception("Path is empty");

                if (!CheckFile(FullPathToSelfExe))
                {
                    SplashWindow.SetStatus($"{Res.SelfUpdating}");
                    SplashWindow.SetProgressMode(false);
                    bool downloaded = Update();

                    if (downloaded && CheckFile($"{NameExeFile}.update.exe"))
                    {
                        Process p = new Process();
                        p.StartInfo.FileName = $"{NameExeFile}.update.exe";
                        p.StartInfo.Arguments = $"{BeginUpdate} {NameExeFile}";
                        p.Start();
                        ExitApp();
                    }
                    else
                    {
                        SplashWindow.SetStatus($"{Res.ErrorSelfUpdate}");
                        Thread.Sleep(1500);
                    }
                }
                else
                {
                    SplashWindow.SetStatus($"{Res.Done}");
                    SplashWindow.SetProgressMode(false);
                    StartLuancher();
                }
#endif
            }
            catch (WebException e)
            {
                MessageBox.Show($"{Res.Details}: {e.Message}", $"{Res.ConnectionError}");
            }
            catch (WebSocketException e)
            {
                MessageBox.Show($"{Res.Details}: {e.Message}", $"{Res.ConnectionError}");
            }
            catch (UnauthorizedAccessException uae)
            {
                FileAttributes attributes = FileAttributes.Normal;
                try
                {
                    string pathToUpdateFile = $"{Path.GetDirectoryName(FullPathToSelfExe)}\\{NameExeFile}.update.exe";
                    attributes = File.GetAttributes(pathToUpdateFile);
                }
                catch { }
                Logger.Error($"CriticalError_{Modules.SelfUpdater.Security.UID}_{attributes}", uae);
                MessageBox.Show($"{Res.Details}: {uae.Message}\n{Res.UrId}: {Modules.SelfUpdater.Security.UID}", $"{Res.CriticalError}");
            }
            catch (Exception e)
            {
                Logger.Error($"CriticalError_{Modules.SelfUpdater.Security.UID}", e);
                MessageBox.Show($"{Res.Details}: {e.Message}\n{Res.UrId}: {Modules.SelfUpdater.Security.UID}", $"{Res.CriticalError}");
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
            string pathToUpdateFile = $"{Path.GetDirectoryName(FullPathToSelfExe)}\\{NameExeFile}.update.exe";
            if (File.Exists(pathToUpdateFile)) File.Delete(pathToUpdateFile);

            Downloader downloader = new Downloader(Updater.AddressToLauncher + Updater.LauncherName, pathToUpdateFile)
            {
                IsHidden = true
            };
            downloader.DownloadChanged += SplashWindow.SetProgress;
            return downloader.Download();
        }
    }
}
