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
    /// Логика взаимодействия для ProgressBarBtn.xaml
    /// </summary>
    public partial class ProgressBarBtn : UserControl
    {
        public event EventHandler Click;

        public Thickness ProgressMargin { get => progressBar.Margin; set { progressBar.Margin = value; mask.Margin = value; } }
        public double StatusFontSize { get => statusText.FontSize; set => statusText.FontSize = value; }
        public string StatusText { get => statusText.Text; set => statusText.Text = value; }

        public Brush NormalColor { get; set; }
        public Brush SelectColor { get; set; }
        public Brush PressColor { get; set; }

        public bool IsDisabled { get; set; }

        public Brush ColorBar { get => progressBar.Foreground; set => progressBar.Foreground = value; }

        public bool IsIndeterminate { get => progressBar.IsIndeterminate; set => progressBar.IsIndeterminate = value; }
        public double Value { get => progressBar.Value; set => progressBar.Value = value; }

        public ProgressBarBtn()
        {
            InitializeComponent();

            mask.MouseEnter += Btn_MouseEnter;
            mask.MouseLeave += Btn_MouseLeave;
            mask.MouseLeftButtonDown += Btn_MouseLeftButtonDown;
            mask.MouseLeftButtonUp += Btn_MouseLeftButtonUp;
        }

        private void Btn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(!IsDisabled)
            {
                Click?.Invoke(sender, e);
                mask.Fill = SelectColor;
            }
        }

        private void Btn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsDisabled)
                mask.Fill = PressColor;
        }

        private void Btn_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsDisabled)
                mask.Fill = NormalColor;
        }

        private void Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!IsDisabled)
                mask.Fill = SelectColor;
        }
    }
}
