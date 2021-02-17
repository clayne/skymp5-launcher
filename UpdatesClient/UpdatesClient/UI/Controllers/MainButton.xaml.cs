using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using UpdatesClient.Modules.Configs;
using Res = UpdatesClient.Properties.Resources;

namespace UpdatesClient.UI.Controllers
{
    public enum MainButtonStatus
    {
        Play,
        Update,
        Retry
    }

    /// <summary>
    /// Логика взаимодействия для MainButton.xaml
    /// </summary>
    public partial class MainButton : UserControl
    {
        private const string Normal = "#6BC1F2";
        private const string Warning = "#F2CD6B";

        public event EventHandler Click;

        private MainButtonStatus buttonStatus;
        public MainButtonStatus ButtonStatus
        {
            get
            {
                return buttonStatus;
            }
            set
            {
                buttonStatus = value;

                if (cycle.Effect is DropShadowEffect efC && textBlock.Effect is DropShadowEffect efT)
                {
                    switch (buttonStatus)
                    {
                        case MainButtonStatus.Play:
                            efC.Color = (Color)ColorConverter.ConvertFromString(Normal);
                            efT.Color = (Color)ColorConverter.ConvertFromString(Normal);
                            textBlock.Text = Res.PLAY;
                            textBlock.FontSize = 28;
                            break;
                        case MainButtonStatus.Update:
                            efC.Color = (Color)ColorConverter.ConvertFromString(Warning);
                            efT.Color = (Color)ColorConverter.ConvertFromString(Warning);
                            textBlock.Text = Res.UPDATE;
                            textBlock.FontSize = 26;
                            break;
                        case MainButtonStatus.Retry:
                            efC.Color = (Color)ColorConverter.ConvertFromString(Warning);
                            efT.Color = (Color)ColorConverter.ConvertFromString(Warning);
                            textBlock.Text = Res.RETRY;
                            if (Settings.Locale == "ru-RU")
                                textBlock.FontSize = 22;
                            else
                                textBlock.FontSize = 28;
                            break;
                    }
                }
            }
        }

        readonly DropShadowEffect efC;
        readonly DropShadowEffect efT;
        readonly DoubleAnimation buttonUpAnimation = new DoubleAnimation
        {
            To = 16,
            Duration = TimeSpan.FromMilliseconds(200)
        };
        readonly DoubleAnimation buttonDownAnimation = new DoubleAnimation
        {
            To = 0,
            Duration = TimeSpan.FromMilliseconds(200)
        };

        public MainButton()
        {
            InitializeComponent();
            efC = cycle.Effect as DropShadowEffect;
            efT = textBlock.Effect as DropShadowEffect;

            efC.BlurRadius = 0;
            efT.BlurRadius = 0;

            MouseEnter += MainButton_MouseEnter;
            MouseLeave += MainButton_MouseLeave;
            MouseLeftButtonDown += MainButton_MouseLeftButtonDown;
            MouseLeftButtonUp += MainButton_MouseLeftButtonUp;
        }

        private void MainButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Click?.Invoke(sender, e);
        }

        private void MainButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {


        }

        private void MainButton_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonDownAnimation.From = efT.BlurRadius;
            efC.BeginAnimation(DropShadowEffect.BlurRadiusProperty, buttonDownAnimation);
            efT.BeginAnimation(DropShadowEffect.BlurRadiusProperty, buttonDownAnimation);
        }

        private void MainButton_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonUpAnimation.From = efT.BlurRadius;
            efC.BeginAnimation(DropShadowEffect.BlurRadiusProperty, buttonUpAnimation);
            efT.BeginAnimation(DropShadowEffect.BlurRadiusProperty, buttonUpAnimation);
        }
    }
}
