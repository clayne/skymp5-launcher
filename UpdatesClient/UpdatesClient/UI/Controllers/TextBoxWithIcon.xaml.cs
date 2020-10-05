using System.Windows.Controls;
using System.Windows.Media;

namespace UpdatesClient.UI.Controllers
{
    /// <summary>
    /// Логика взаимодействия для TextBoxWithIcon.xaml
    /// </summary>
    public partial class TextBoxWithIcon : UserControl
    {
        public ImageSource Icon { get => icon.Source; set => icon.Source = value; }
        public string Text { get => textBox.Text; set => textBox.Text = value; }
        public string PlaceHolder { get => (string)textBox.Tag; set => textBox.Tag = value; }

        public TextBoxWithIcon()
        {
            InitializeComponent();
        }
    }
}
