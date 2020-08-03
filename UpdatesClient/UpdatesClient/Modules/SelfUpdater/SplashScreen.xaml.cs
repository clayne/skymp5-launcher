using System;
using System.Windows;
using System.Windows.Threading;

namespace UpdatesClient.Modules.SelfUpdater
{
    /// <summary>
    /// Логика взаимодействия для SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Splash_Loaded);
        }

        void Splash_Loaded(object sender, RoutedEventArgs e)
        {
            IAsyncResult result = null;

            AsyncCallback initCompleted = delegate (IAsyncResult ar)
            {
                App.Current.ApplicationInitialize.EndInvoke(result);

                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate { Close(); });
            };
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
