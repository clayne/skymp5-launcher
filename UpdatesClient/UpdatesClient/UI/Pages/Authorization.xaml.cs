using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UpdatesClient.Core;
using UpdatesClient.Core.Network;
using UpdatesClient.Core.Network.Models.Request;
using UpdatesClient.Core.Network.Models.Response;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.Debugger;
using UpdatesClient.UI.Controllers;
using UpdatesClient.UI.Pages.Models.AuthModels;
using Res = UpdatesClient.Properties.Resources;

namespace UpdatesClient.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : UserControl
    {
        public delegate void AuthResult();
        public event AuthResult SignIn;

        //TODO: уведомления об ошибках

        public FormModel FormModel { get; set; }

        public Authorization()
        {
            InitializeComponent();

            FormModel = new FormModel
            {
                AuthModel = new AuthModel(),
                RegModel = new RegModel(),
                RecPswrdModel = new RecPswrdModel(),
                CurrentView = FormModel.View.SignIn
            };
            DataContext = FormModel;
        }

        private void Open_AuthPanel(object sender, RoutedEventArgs e)
        {
            FormModel.CurrentView = FormModel.View.SignIn;
        }
        private void Open_RegisterPanel(object sender, RoutedEventArgs e)
        {
            FormModel.CurrentView = FormModel.View.SignUp;
        }
        private void Open_ForgotPassPanel(object sender, RoutedEventArgs e)
        {
            FormModel.CurrentView = FormModel.View.Recov;
        }

        private async void Signin_Click(object sender, RoutedEventArgs e)
        {
            authPanel.IsEnabled = false;
            //Settings.RememberMe = (bool)rmAuth.IsChecked;

            try
            {
                ReqLoginModel model = new ReqLoginModel()
                {
                    Password = ""
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
                if (s != null)
                {
                    using (var reader = new StreamReader(s.GetResponseStream()))
                    {
                        string raw = reader.ReadToEnd();
                        try
                        {
                            JArray jObject = JArray.Parse(raw);
                            foreach (JToken par in jObject.Children())
                            {
                                NotifyController.Show(PopupNotify.Error, par.Value<string>("property"),
                                    ((JProperty)par.Value<JToken>("constraints").First()).Value.ToString(), 4000);
                            }
                        }
                        catch
                        {
                            NotifyController.Show(PopupNotify.Error, Res.Error, raw, 5000);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Logger.Error("Auth_Login", err);
            }

            authPanel.IsEnabled = true;
        }
        private async void Signup_Click(object sender, RoutedEventArgs e)
        {
            registerPanel.IsEnabled = false;

            try
            {
                ReqRegisterModel model = new ReqRegisterModel()
                {

                };
                ResRegisterModel ds = await Account.Register(model);
                NotifyController.Show(PopupNotify.Normal, Res.Successfully, Res.VerifyAccount);
            }
            catch (WebException we)
            {
                WebResponse s = we.Response;
                if (s != null)
                {
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
                            NotifyController.Show(PopupNotify.Error, Res.Error, raw, 5000);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Logger.Error("Auth_Register", err);
            }

            registerPanel.IsEnabled = true;
        }
        private async void Forgot_Click(object sender, RoutedEventArgs e)
        {
            forgotPassPanel.IsEnabled = false;

            try
            {
                ReqResetPassword model = new ReqResetPassword()
                {

                };
                await Account.ResetPassword(model);
                await Task.Delay(200);
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
                        NotifyController.Show(PopupNotify.Error, Res.Error, raw, 5000);
                    }
                }
            }
            catch (Exception err)
            {
                Logger.Error("Auth_ResetPassword", err);
            }

            forgotPassPanel.IsEnabled = true;
        }
    }
}
