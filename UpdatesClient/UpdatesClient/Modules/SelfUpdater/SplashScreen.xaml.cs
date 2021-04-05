using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using UpdatesClient.Core.Network;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.Debugger;

namespace UpdatesClient.Modules.SelfUpdater
{
    /// <summary>
    /// Логика взаимодействия для SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        private bool WaitOk = false;
        private bool Ok = false;

        public SplashScreen()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.Locale))
                {
                    SelectLanguage language = new SelectLanguage();
                    language.ShowDialog();
                    if (string.IsNullOrEmpty(language.LanguageBase))
                    {
                        Close();
                        return;
                    }
                    Settings.Locale = language.LanguageBase;
                    Settings.Save();
                }
                try
                {
                    Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(Settings.Locale);
                } catch (Exception e) { Logger.Error("SetLanguage", e); }
            }
            catch (Exception e)
            {
                Logger.Error("SelectLanguage", e);
            }
            
            InitializeComponent();
            Loaded += new RoutedEventHandler(Splash_Loaded);
        }

        void Splash_Loaded(object sender, RoutedEventArgs e)
        {
            IAsyncResult result = null;

            void initCompleted(IAsyncResult ar)
            {
                App.Current.ApplicationInitialize.EndInvoke(result);
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate { Close(); });
            }
            result = App.Current.ApplicationInitialize.BeginInvoke(this, initCompleted, null);
        }

        public void SetProgress(double Value, double LenFile, double prDown)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate
            {
                progBar.Value = prDown / 100;
                Status.Text = $"{(int)prDown}% ({Value}КБ/{LenFile}КБ)";
            });
        }
        public void SetProgressMode(bool IsIdenterminate)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate { progBar.IsIndeterminate = IsIdenterminate; });
        }
        public void SetStatus(string text)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate { Status.Text = text; });
        }

        public void Ready()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate
            {
                DispReady();
            });
        }

        private async void DispReady()
        {
            try
            {
                string username = await Account.GetLogin();
                Settings.UserName = username;
                Ok = true;
                WaitOk = true;
            }
            catch
            {
                Animation();
            }
        }

        public bool Wait()
        {
            //Условие выхода - авторизация
            while (!WaitOk) Thread.Sleep(100);
            return Ok;
        }

        public async void Animation()
        {
            progressBarGrid.Visibility = Visibility.Collapsed;
            header.MoveIsEnabled = false;
            const double width = 1054;
            TimeSpan span = new TimeSpan(0, 0, 0, 0, 750);

            DoubleAnimation animWidth = new DoubleAnimation(width, new Duration(span))
            {
                FillBehavior = FillBehavior.HoldEnd
            };
            BeginAnimation(WidthProperty, animWidth);
            BeginAnimation(MinWidthProperty, animWidth);
            DoubleAnimation animLeft = new DoubleAnimation(Left - (width - 469) / 2, new Duration(span))
            {
                FillBehavior = FillBehavior.Stop
            };
            BeginAnimation(LeftProperty, animLeft);

            await Task.Delay(span);
            header.MoveIsEnabled = true;
        }
    }
}
