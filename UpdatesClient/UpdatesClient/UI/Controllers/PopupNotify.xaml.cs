using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UpdatesClient.Core.Models;

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

        public int DelayMs { get; }

        public enum Type
        {
            Normal,
            Error
        }

        public PopupNotify(NotifyModel model)
        {
            InitializeComponent();
            switch (model.Type)
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
            this.status.Text = model.Status;
            description.Text = model.Text;
            closeBtn.Click += (s, e) => ClickClose?.Invoke(this, e);
            DelayMs = model.DelayMs;
        }
    }
}
