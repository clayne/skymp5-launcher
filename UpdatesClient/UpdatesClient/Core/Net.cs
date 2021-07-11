using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.Debugger;

namespace UpdatesClient.Core
{
    public class Net
    {
        public const string URL_Version = "https://skymp.skyrez.su/api/latest_version.txt";
        public const string URL_SKSELink = "https://skymp.skyrez.su/api/skse_link.php";
        public const string URL_ModLink = "https://skymp.skyrez.su/api/skymp_link.php";

        public const string URL_CrashDmp = "https://skymp.skyrez.su/api/crashes.php";
        public const string URL_SERVERS = "https://skymp.io/api/servers";

        public const string URL_Mod_RuFix = "https://skymp.skyrez.su/mods/SSERuFixConsole.zip";

        public const string URL_ApiLauncher = "https://skymplauncher.skyrez.su/api/v1/";

        private static readonly HttpClient http = new HttpClient();

        static Net()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        public static Task<string> GetLastestVersion()
        {
            return GetAsync($"{URL_Version}", false);
        }

        public static async Task<(string, string)> GetUrlToClient()
        {
            string ver = await GetLastestVersion();
            string link = await GetAsync(URL_ModLink, false);
            return (link, ver);
        }

        public static async Task<string> GetUrlToSKSE()
        {
            string link = await GetAsync(URL_SKSELink, false);
            return link;
        }

        public static async Task<bool> ReportDmp(string pathToFile)
        {
            if (NetworkSettings.ReportDmp)
            {
                string req = await UploadRequest(URL_CrashDmp, false, pathToFile, "crashdmp", null);
                if (req != "OK") Logger.Error("ReportDmp_Net", new Exception(req));
                return req == "OK";
            }
            return true;
        }

        public static async Task<string> PostAsync(string url, bool auth, string data)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            if (auth) request.Headers.Add(nameof(HttpRequestHeader.Authorization), Settings.UserToken);
            request.Content = new StringContent(data ?? "", Encoding.UTF8, "application/json");

            HttpResponseMessage response = await http.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> GetAsync(string url, bool auth)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            if (auth) request.Headers.Add(nameof(HttpRequestHeader.Authorization), Settings.UserToken);

            HttpResponseMessage response = await http.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> UploadRequest(string url, bool auth, string file, string namePar, string data)
        {
            MultipartFormDataContent dataContent = new MultipartFormDataContent(DateTime.Now.Ticks.ToString("x"));
            dataContent.Add(new ByteArrayContent(File.ReadAllBytes(file)), namePar);
            if (data != null) dataContent.Add(new StringContent(data, Encoding.UTF8, "application/json"), "data");
            if (auth) dataContent.Headers.Add(nameof(HttpRequestHeader.Authorization), Settings.UserToken);
            HttpResponseMessage response = await http.PostAsync(url, dataContent);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
