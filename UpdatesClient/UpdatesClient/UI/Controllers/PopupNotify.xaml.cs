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
    /// Логика взаимодействия для PopupNotify.xaml
    /// </summary>
    public partial class PopupNotify : UserControl
    {
        public event EventHandler ClickClose;

        public const Type Normal = Type.Normal;
        public const Type Error = Type.Error;

        public enum Type
        {
            Normal,
            Error
        }

        public PopupNotify(Type type, string status, string text)
        {
            InitializeComponent();
            switch (type)
            {
                case Type.Normal:
                    border.Background = new SolidColorBrush(Color.FromArgb(255, 9, 188, 0));
                    border.BorderBrush = new SolidColorBrush(Color.FromArgb(242, 0, 255, 102));
                    smile.Source = (ImageSource)Application.Current.Resources["HappySmile"];
                    break;
                case Type.Error:
                    border.Background = new SolidColorBrush(Color.FromArgb(255, 188, 0, 0));
                    border.BorderBrush = new SolidColorBrush(Color.FromArgb(242, 255, 0, 0));
                    smile.Source = (ImageSource)Application.Current.Resources["SadSmile"];
                    break;
            }
            this.status.Text = status;
            description.Text = text;
            closeBtn.Click += (s, e) => ClickClose?.Invoke(this, e);
        }

        public PopupNotify(Exception exception) : this(Error, "Ошибка", exception.Message)
        {

        }
    }
}
