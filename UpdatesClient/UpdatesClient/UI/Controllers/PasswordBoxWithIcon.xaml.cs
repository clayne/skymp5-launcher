using System.Windows.Controls;
using System.Windows.Media;

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
