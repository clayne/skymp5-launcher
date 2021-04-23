using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using UpdatesClient.Core.Enums;
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
                if (Settings.Locale == 0)
                {
                    SelectLanguage language = new SelectLanguage();
                    language.ShowDialog();
                    if (string.IsNullOrEmpty(language.LanguageBase))
                    {
                        Close();
                        return;
                    }

                    Settings.Locale = GetLocaleByName(language.LanguageBase);
                    Settings.Save();
                }
                try
                {
                    Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(GetLocaleDescription(Settings.Locale));
                }
                catch (Exception e) { Logger.Error("SetLanguage", e); }
            }
            catch (Exception e)
            {
                Logger.Error("SelectLanguage", e);
            }

            InitializeComponent();
            Loaded += new RoutedEventHandler(Splash_Loaded);
        }

        private Locales GetLocaleByName(string name)
        {
            switch (name)
            {
                case "ru-RU": return Locales.ru_RU;
                case "en-US": return Locales.en_US;
                default: return Locales.nul;
            }
        }
        private string GetLocaleDescription(Locales locale)
        {
            MemberInfo[] memInfo = typeof(Locales).GetMember(locale.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return "en-US";
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
                auth.SignIn += Auth_SignIn;
                DispReady();
            });
        }

        private async void DispReady()
        {
            try
            {
                await Auth();
            }
            catch
            {
                Animation();
            }
        }

        private async Task Auth()
        {
            string username = await Account.GetLogin();
            Settings.UserName = username;
            Ok = true;
            WaitOk = true;
        }

        private async void Auth_SignIn()
        {
            try
            {
                await Auth();
            }
            catch { }
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
