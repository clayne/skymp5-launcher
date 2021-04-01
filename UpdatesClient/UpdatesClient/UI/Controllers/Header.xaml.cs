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

namespace UpdatesClient.UI.Controllers
{
    /// <summary>
    /// Логика взаимодействия для Header.xaml
    /// </summary>
    public partial class Header : UserControl
    {
        public bool MaximizeIsEnabled
        {
            get
            {
                return maximize.Visibility == Visibility.Collapsed;
            }

            set
            {
                if (value)
                    maximize.Visibility = Visibility.Visible;
                else
                    maximize.Visibility = Visibility.Collapsed;
            }
        }

        public bool MoveIsEnabled { get; set; } = true;

        private Window window;

        public Header()
        {
            InitializeComponent();
            grid.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
            Loaded += Header_Loaded;
        }

        private void Header_Loaded(object sender, RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MoveIsEnabled) window?.DragMove();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            window?.Close();
        }

        private void Maximize(object sender, RoutedEventArgs e)
        {
            if (window != null)
            {
                if (window.WindowState == WindowState.Normal) window.WindowState = WindowState.Maximized;
                else window.WindowState = WindowState.Normal;
            }
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            if (window != null) window.WindowState = WindowState.Minimized;
        }
    }
}
