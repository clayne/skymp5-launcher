using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using UpdatesClient.Core;
using UpdatesClient.Modules.Configs;

namespace UpdatesClient.Modules.SelfUpdater
{
    /// <summary>
    /// Логика взаимодействия для SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.Locale))
                {
                    SelectLanguage language = new SelectLanguage();
                    language.ShowDialog();
                    Settings.Locale = language.Language;
                    Settings.Save();
                }
                Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(Settings.Locale);
            }
            catch (Exception e)
            {
                Logger.Error("SelectLanguage", e);
            }
            
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Splash_Loaded);
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
    }
}
