using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace UpdatesClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Check file "SkyrimSE.exe"
        //Base color: #FF04D9FF
        //War color: #FFFF7604
        //Er color: #FFFF0404

        public MainWindow()
        {
            InitializeComponent();
            TitleWindow.MouseLeftButtonDown += (s, e) => DragMove();

            wind.Loaded += delegate {
                if (string.IsNullOrEmpty(Properties.Settings.Default.PathToSkyrim))
                {
                    using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                    {
                        System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                        if (result == System.Windows.Forms.DialogResult.OK && File.Exists(dialog.SelectedPath + "\\SkyrimSE.exe"))
                        {
                            Properties.Settings.Default.PathToSkyrim = dialog.SelectedPath;
                            Properties.Settings.Default.Version = "0.0.0.0";
                            Properties.Settings.Default.Save();
                        }
                        else { Application.Current.Shutdown(); }
                    }
                }

                BrushConverter converter = new BrushConverter();
                MainBtn.ColorBar = (Brush)converter.ConvertFrom("#FF04D9FF");
                CheckClient();
            };

        }

        private async void CheckClient()
        {
            MainBtn.IsIndeterminate = true;

            await Task.Delay(1000);

            //CheckUpdate

            MainBtn.StatusText = "ИГРАТЬ";
            MainBtn.IsIndeterminate = false;
            MainBtn.IsDisabled = false;
        }


        private void ImageButton_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MainBtn_Click(object sender, EventArgs e)
        {
            //Play
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.FileName = $"{Properties.Settings.Default.PathToSkyrim}\\SkyrimSE.exe";
            startInfo.Arguments = $"--UUID Launcher --Session TEST";
            startInfo.WorkingDirectory = $"{Properties.Settings.Default.PathToSkyrim}\\";

            process.StartInfo = startInfo;
            process.Start();
            Close();
        }
    }
}
