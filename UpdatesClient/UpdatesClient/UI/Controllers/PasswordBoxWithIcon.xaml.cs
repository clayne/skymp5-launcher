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
    /// Логика взаимодействия для PasswordBoxWithIcon.xaml
    /// </summary>
    public partial class PasswordBoxWithIcon : UserControl
    {
        public ImageSource Icon { get => icon.Source; set => icon.Source = value; }
        public string Password { get => passwordBox.Password; set => passwordBox.Password = value; }
        public string PlaceHolder { get => (string)passwordBox.Tag; set => passwordBox.Tag = value; }

        public PasswordBoxWithIcon()
        {
            InitializeComponent();
        }
    }
}
