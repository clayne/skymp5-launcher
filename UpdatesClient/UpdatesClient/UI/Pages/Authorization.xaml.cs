using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UpdatesClient.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : UserControl
    {
        public Authorization()
        {
            InitializeComponent();
            authPanel.Visibility = Visibility.Visible;
        }

        private void open_RegisterPanel(object sender, RoutedEventArgs e)
        {
            authPanel.Visibility = Visibility.Collapsed;
            registerPanel.Visibility = Visibility.Visible;
        }

        private void open_ForgotPassPanel(object sender, RoutedEventArgs e)
        {
            authPanel.Visibility = Visibility.Collapsed;
            forgotPassPanel.Visibility = Visibility.Visible;
        }
    }
}
