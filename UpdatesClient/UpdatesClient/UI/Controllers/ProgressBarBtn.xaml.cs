using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace UpdatesClient.UI.Controllers
{
    /// <summary>
    /// Логика взаимодействия для ProgressBarBtn.xaml
    /// </summary>
    public partial class ProgressBarBtn : UserControl
    {
        public event EventHandler Click;

        public enum ColorType
        {
            Normal,
            Warning,
            Error
        }

        public Thickness ProgressMargin { get => progressBar.Margin; set { progressBar.Margin = value; mask.Margin = value; } }
        public double StatusFontSize { get => statusText.FontSize; set => statusText.FontSize = value; }
        public string StatusText { get => statusText.Text; set => statusText.Text = value; }

        public Brush NormalColor { get; set; }
        public Brush BaseColor { get; set; }
        public Brush WarningColor { get; set; }
        public Brush ErrorColor { get; set; }
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
            if (!IsDisabled)
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

        public async Task ShowMessage(ColorType color, string text, int timeDelay = 1000)
        {
            bool _IsDisabled = IsDisabled;
            bool _IsIndeterminate = IsIndeterminate;
            double _Value = Value;
            string _StatusText = StatusText;

            IsIndeterminate = false;
            IsDisabled = true;
            Value = 100;
            switch (color)
            {
                case ColorType.Normal:
                    ColorBar = BaseColor;
                    break;
                case ColorType.Warning:
                    ColorBar = WarningColor;
                    break;
                case ColorType.Error:
                    ColorBar = ErrorColor;
                    break;
            }
            StatusText = text;
            await Task.Delay(timeDelay);

            ColorBar = NormalColor;

            IsDisabled = _IsDisabled;
            IsIndeterminate = _IsIndeterminate;
            Value = _Value;
            StatusText = _StatusText;
        }

        public void SetAsProgressBar(ColorType color, bool isIndeterminate, string text = null)
        {
            IsDisabled = true;
            IsIndeterminate = isIndeterminate;
            Value = 0;

            StatusText = text ?? "0%";

            switch (color)
            {
                case ColorType.Normal:
                    ColorBar = BaseColor;
                    break;
                case ColorType.Warning:
                    ColorBar = WarningColor;
                    break;
                case ColorType.Error:
                    ColorBar = ErrorColor;
                    break;
            }
        }

        public void SetAsButton(ColorType color, string text)
        {
            IsDisabled = false;
            IsIndeterminate = false;
            Value = 100;

            StatusText = text;

            switch (color)
            {
                case ColorType.Normal:
                    ColorBar = BaseColor;
                    break;
                case ColorType.Warning:
                    ColorBar = WarningColor;
                    break;
                case ColorType.Error:
                    ColorBar = ErrorColor;
                    break;
            }
        }
    }
}
