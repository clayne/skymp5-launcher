using System;
using System.Windows;
using System.Windows.Controls;
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
        public string LanguageBase = null;
        
        public SelectLanguage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void SelectLang(object sender, RoutedEventArgs e)
        {
            switch(((Button)sender).Name)
            {
                case "ru":
                    LanguageBase = "ru-RU";
                    goto default;
                case "en":
                    LanguageBase = "en-US";
                    goto default;
                default:
                    Close();
                    break;
            }
        }
    }
}
