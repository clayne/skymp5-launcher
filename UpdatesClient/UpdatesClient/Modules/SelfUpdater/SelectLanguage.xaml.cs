using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace UpdatesClient.Modules.SelfUpdater
{
    /// <summary>
    /// Логика взаимодействия для SelectLanguage.xaml
    /// </summary>
    public partial class SelectLanguage : Window
    {
        public string LanguageBase = "en-US";
        private bool Selected = false;
        private readonly DropShadowEffect dse = new DropShadowEffect()
        {
            BlurRadius = 30,
            Color = Colors.White,
            Direction = 0,
            ShadowDepth = 0
        };

        public SelectLanguage()
        {
            InitializeComponent();
        }

        private void Clear()
        {
            ru.Effect = null;
            us.Effect = null;
            ru.Opacity = 0.5;
            us.Opacity = 0.5;
        }


        private void Ru_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Clear();
            ru.Effect = dse;
            ru.Opacity = 1;
            LanguageBase = "ru-RU";
            Selected = true;
        }

        private void Us_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Clear();
            us.Effect = dse;
            us.Opacity = 1;
            LanguageBase = "en-US";
            Selected = true;
        }

        private void ImageButton_Click(object sender, EventArgs e)
        {
            if (Selected) Close();
        }
    }
}
