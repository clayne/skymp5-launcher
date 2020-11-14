using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;
using UpdatesClient.Core.Network.Models.Request;
using UpdatesClient.Core.Network.Models.Response;
using UpdatesClient.Modules.Configs;

namespace UpdatesClient.Core.Network
{
    public class Account
    {
        public const string URL_Api = "https://skymp.io/api/";

        public static async Task<ResRegisterModel> Register(ReqRegisterModel model)
        {
            string raw = await Request($"{URL_Api}users", "POST", false, JsonConvert.SerializeObject(model));
            return JsonConvert.DeserializeObject<ResRegisterModel>(raw);
        }

        public static async Task<ResLoginModel> Login(ReqLoginModel model)
        {
            string raw = await Request($"{URL_Api}users/login", "POST", false, JsonConvert.SerializeObject(model));
            return JsonConvert.DeserializeObject<ResLoginModel>(raw);
        }

        public static async Task<ResVerifyRegisterModel> Verify(ReqVerifyRegisterModel model)
        {
            string raw = await Request($"{URL_Api}{model.Id}/users", "POST", true, JsonConvert.SerializeObject(model));
            return JsonConvert.DeserializeObject<ResVerifyRegisterModel>(raw);
        }

        public static Task ResetPassword(ReqResetPassword model)
        {
            return Request($"{URL_Api}users/reset-password", "POST", false, JsonConvert.SerializeObject(model));
        }

        public static Task VerifyToken()
        {
            return Request($"{URL_Api}secure", "GET", true, null);
        }

        public static Task<string> GetLogin()
        {
            return Request($"{URL_Api}users/{Settings.UserId}", "GET", true, null);
        }

        public static async Task<object> GetSession(string address)
        {
            string raw = await Request($"{URL_Api}users/{Settings.UserId}/play/{address}", "POST", true, null);
            return JsonConvert.DeserializeObject(raw);
        }

        private static async Task<string> Request(string url, string method, bool auth, string data) 
        {
            try
            {
                return await Net.Request(url, method, auth, data);
            }
            catch (WebSocketException wse)
            {
                NotifyController.Show(wse);
            }
            catch (WebException we)
            {
                NotifyController.Show(we);
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }
    }
}
