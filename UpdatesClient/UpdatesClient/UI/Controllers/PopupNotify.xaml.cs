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
            description.Text = model.Text;
            time.Text = DateTime.Now.ToString("HH:mm");
            closeBtn.Click += (s, e) => ClickClose?.Invoke(this, e);
            DelayMs = model.DelayMs;
        }
    }
}
