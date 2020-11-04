using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UpdatesClient.Core;
using Yandex.Metrica;

namespace UpdatesClient.UI.Controllers
{
    /// <summary>
    /// Логика взаимодействия для ImageButton.xaml
    /// </summary>
    public partial class ImageButton : UserControl
    {
        public Thickness ImageMargin { get => image.Margin; set => image.Margin = value; }
        public Brush Image { get => image.OpacityMask; set => image.OpacityMask = value; }
        public Brush NormalColor { get; set; }
        public Brush SelectColor { get; set; }
        public Brush PressColor { get; set; }

        public event EventHandler Click;

        public ImageButton()
        {
            try
            {
                InitializeComponent();

                this.Loaded += (a, e) => image.Fill = NormalColor;
                btn.MouseEnter += Btn_MouseEnter;
                btn.MouseLeave += Btn_MouseLeave;
                btn.MouseLeftButtonDown += Btn_MouseLeftButtonDown;
                btn.MouseLeftButtonUp += Btn_MouseLeftButtonUp;
            }
            catch (Exception e) { YandexMetrica.ReportError("ImageButton", e); Logger.Error("ImageButton", e); }
            
        }

        private void Btn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Click?.Invoke(sender, e);
            image.Fill = SelectColor;
        }

        private void Btn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            image.Fill = PressColor;
        }

        private void Btn_MouseLeave(object sender, MouseEventArgs e)
        {
            image.Fill = NormalColor;
        }

        private void Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            image.Fill = SelectColor;
        }
    }
}
