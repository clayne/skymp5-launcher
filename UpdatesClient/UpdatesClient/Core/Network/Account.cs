using Newtonsoft.Json;
using System.Threading.Tasks;
using UpdatesClient.Core.Network.Models.Request;
using UpdatesClient.Core.Network.Models.Response;

namespace UpdatesClient.Core.Network
{
    public class Account
    {
        public const string URL_Api = "https://skymp.io/api/";

        public static async Task<ResRegisterModel> Register(ReqRegisterModel model)
        {
            string raw = await Net.Request($"{URL_Api}users", "POST", false, JsonConvert.SerializeObject(model));
            return JsonConvert.DeserializeObject<ResRegisterModel>(raw);
        }

        public static async Task<ResLoginModel> Login(ReqLoginModel model)
        {
            string raw = await Net.Request($"{URL_Api}login", "POST", false, JsonConvert.SerializeObject(model));
            return JsonConvert.DeserializeObject<ResLoginModel>(raw);
        }

        public static async Task<ResVerifyRegisterModel> Verify(ReqVerifyRegisterModel model)
        {
            string raw = await Net.Request($"{URL_Api}{model.Id}/users", "POST", true, JsonConvert.SerializeObject(model));
            return JsonConvert.DeserializeObject<ResVerifyRegisterModel>(raw);
        }

        public static Task ResetPassword(ReqResetPassword model)
        {
            return Net.Request($"{URL_Api}/users/reset-password", "POST", false, JsonConvert.SerializeObject(model));
        }
    }
}
