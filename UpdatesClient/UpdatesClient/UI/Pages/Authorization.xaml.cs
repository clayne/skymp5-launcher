using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UpdatesClient.Core;
using UpdatesClient.Core.Network;
using UpdatesClient.Core.Network.Models.Request;
using UpdatesClient.Core.Network.Models.Response;
using UpdatesClient.Modules.Configs;
using UpdatesClient.UI.Controllers;
using Yandex.Metrica;

namespace UpdatesClient.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : UserControl
    {
        public delegate void AuthResult();
        public event AuthResult SignIn;

        public Authorization()
        {
            InitializeComponent();
            authPanel.Visibility = Visibility.Visible;
            forgotPassPanel.Visibility = Visibility.Collapsed;
            registerPanel.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Hidden;
        }

        private void Clear()
        {
            authPanel.IsEnabled = true;
            registerPanel.IsEnabled = true;
            forgotPassPanel.IsEnabled = true;
            backButton.IsEnabled = true;

            authPanel.Visibility = Visibility.Visible;
            forgotPassPanel.Visibility = Visibility.Collapsed;
            registerPanel.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Hidden;

            rmAuth.IsChecked = false;
            emailAuth.Text = "";
            passAuth.Password = "";

            nameReg.Text = "";
            emailReg.Text = "";
            passReg.Password = "";
            passCheckReg.Password = "";

            emailForgot.Text = "";
        }

        private void open_RegisterPanel(object sender, RoutedEventArgs e)
        {
            authPanel.Visibility = Visibility.Collapsed;
            registerPanel.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;
        }

        private void open_ForgotPassPanel(object sender, RoutedEventArgs e)
        {
            authPanel.Visibility = Visibility.Collapsed;
            forgotPassPanel.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private async void signin_Click(object sender, RoutedEventArgs e)
        {
            authPanel.IsEnabled = false;
            backButton.IsEnabled = false;
            Settings.RememberMe = (bool)rmAuth.IsChecked;

            try
            {
                ReqLoginModel model = new ReqLoginModel()
                {
                    Email = emailAuth.Text,
                    Password = passAuth.Password
                };
                ResLoginModel ds = await Account.Login(model);

                Settings.UserId = ds.Id;
                Settings.UserToken = ds.Token;
                Settings.Save();
                SignIn?.Invoke();
            }
            catch (WebException we)
            {
                var s = we.Response;
                using (var reader = new StreamReader(s.GetResponseStream()))
                {
                    string raw = reader.ReadToEnd();
                    try
                    {
                        JArray jObject = JArray.Parse(raw);
                        foreach (JToken par in jObject.Children())
                        {
                            NotifyController.Show(PopupNotify.Error, par.Value<string>("property"), ((JProperty)par.Value<JToken>("constraints").First()).Value.ToString(), 4000);
                        }
                    }
                    catch
                    {
                        NotifyController.Show(PopupNotify.Error, "Ошибка", raw, 5000);
                    }

                }
            }
            catch (Exception err)
            {
                YandexMetrica.ReportError("Auth_Login", err);
            }
            
            authPanel.IsEnabled = true;
            backButton.IsEnabled = true;
        }
        private async void signup_Click(object sender, RoutedEventArgs e)
        {
            if (passReg.Password != passCheckReg.Password)
            {
                passCheckReg.Password = "";
                return;
            }

            registerPanel.IsEnabled = false;
            backButton.IsEnabled = false;

            try
            {
                ReqRegisterModel model = new ReqRegisterModel()
                {
                    Email = emailReg.Text,
                    Name = nameReg.Text,
                    Password = passReg.Password
                };
                ResRegisterModel ds = await Account.Register(model);

                Settings.UserId = ds.Id;
                Settings.Save();
                SignIn?.Invoke();
                Clear();
            }
            catch (WebException we)
            {
                var s = we.Response;
                using (var reader = new StreamReader(s.GetResponseStream()))
                {
                    string raw = reader.ReadToEnd();
                    try
                    {
                        JArray jObject = JArray.Parse(raw);
                        foreach (JToken par in jObject.Children())
                        {
                            NotifyController.Show(PopupNotify.Error, par.Value<string>("property"), ((JProperty)par.Value<JToken>("constraints").First()).Value.ToString(), 4000);
                        }
                    }
                    catch
                    {
                        NotifyController.Show(PopupNotify.Error, "Ошибка", raw, 5000);
                    }
                    
                }
            }
            catch (Exception err)
            {
               YandexMetrica.ReportError("Auth_Register", err);
            }

            registerPanel.IsEnabled = true;
            backButton.IsEnabled = true;
        }
        private async void forgot_Click(object sender, RoutedEventArgs e)
        {
            forgotPassPanel.IsEnabled = false;
            backButton.IsEnabled = false;

            try
            {
                ReqResetPassword model = new ReqResetPassword()
                {
                    Email = emailForgot.Text
                };
                await Account.ResetPassword(model);
                await Task.Delay(200);
                Clear();
            }
            catch (WebException we)
            {
                var s = we.Response;
                using (var reader = new StreamReader(s.GetResponseStream()))
                {
                    string raw = reader.ReadToEnd();
                    try
                    {
                        JArray jObject = JArray.Parse(raw);
                        foreach (JToken par in jObject.Children())
                        {
                            NotifyController.Show(PopupNotify.Error, par.Value<string>("property"), ((JProperty)par.Value<JToken>("constraints").First()).Value.ToString(), 4000);
                        }
                    }
                    catch
                    {
                        NotifyController.Show(PopupNotify.Error, "Ошибка", raw, 5000);
                    }
                }
            }
            catch (Exception err)
            {
                YandexMetrica.ReportError("Auth_ResetPassword", err);
            }
            
            forgotPassPanel.IsEnabled = true;
            backButton.IsEnabled = true;
        }
    }
}
