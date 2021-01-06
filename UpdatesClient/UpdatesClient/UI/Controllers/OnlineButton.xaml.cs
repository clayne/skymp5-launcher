using System;
using System.Windows.Controls;

namespace UpdatesClient.UI.Controllers
{
    /// <summary>
    /// Логика взаимодействия для OnlineButton.xaml
    /// </summary>
    public partial class OnlineButton : UserControl
    {
        public event EventHandler Click;

        public string Text { get => (string)Username.Header; set => Username.Header = value; }

        public OnlineButton()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Click?.Invoke(sender, e);
        }
    }
}
