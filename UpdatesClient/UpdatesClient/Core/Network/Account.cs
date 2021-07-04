using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading.Tasks;
using UpdatesClient.Core.Network.Models.Request;
using UpdatesClient.Core.Network.Models.Response;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.Notifications;

namespace UpdatesClient.Core.Network
{
    public class Account
    {
        public const string URL_Api = "https://skymp.io/api/";

        public static async Task<ResRegisterModel> Register(ReqRegisterModel model)
        {
            string raw = await Net.PostAsync($"{URL_Api}users", false, JsonConvert.SerializeObject(model));
            return JsonConvert.DeserializeObject<ResRegisterModel>(raw);
        }

        public static async Task<ResLoginModel> Login(ReqLoginModel model)
        {
            string raw = await Net.PostAsync($"{URL_Api}users/login", false, JsonConvert.SerializeObject(model));
            return JsonConvert.DeserializeObject<ResLoginModel>(raw);
        }

        public static async Task<ResVerifyRegisterModel> Verify(ReqVerifyRegisterModel model)
        {
            string raw = await RequestPost($"{URL_Api}{model.Id}/users", true, JsonConvert.SerializeObject(model));
            if (raw != null) return JsonConvert.DeserializeObject<ResVerifyRegisterModel>(raw);
            else return default;
        }

        public static Task ResetPassword(ReqResetPassword model)
        {
            return RequestPost($"{URL_Api}users/reset-password", false, JsonConvert.SerializeObject(model));
        }

        public static Task VerifyToken()
        {
            return Net.GetAsync($"{URL_Api}secure", true);
        }

        public static async Task<string> GetLogin()
        {
            string raw = await Net.GetAsync($"{URL_Api}users/{Settings.UserId}", true);
            JObject jObject = JObject.Parse(raw);
            return jObject["name"].ToString();
        }

        public static async Task<object> GetSession(string address)
        {
            string raw = await RequestPost($"{URL_Api}users/{Settings.UserId}/play/{address}", true, null);
            if (raw != null) return JsonConvert.DeserializeObject(raw);
            else return null;

        }

        private static async Task<string> RequestPost(string url, bool auth, string data)
        {
            try
            {
                return await Net.PostAsync(url, auth, data);
            }
            catch (HttpRequestException hre)
            {
                NotifyController.Show(hre);
            }
            catch (WebSocketException wse)
            {
                NotifyController.Show(wse);
            }
            catch (WebException we)
            {
                NotifyController.Show(we);
            }
            return null;
        }
    }
}
