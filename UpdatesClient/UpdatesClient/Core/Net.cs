using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UpdatesClient.Core
{
    public class Net
    {
        public const string URL_Version = "https://skymp.io/api/latest_version";
        public const string URL_Client = "https://github.com/skyrim-multiplayer/skymp5-binaries/releases/download/{VERSION}/client.zip";



        public static async Task<bool> UpdateAvailable()
        {
            string result = await Request($"{URL_Version}", null);

            return Properties.Settings.Default.Version != result;
        }

        public static async Task<(string, string)> GetUrlToClient()
        {
            string ver = await Request($"{URL_Version}", null);
            return (URL_Client.Replace("{VERSION}", ver), ver);
        }

        private static async Task<string> Request(string url, string data)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:74.0) Gecko/20100101 Firefox/74.0";
            req.ContentType = "application/x-www-form-urlencoded;";
            if (data != null)
                using (var sw = new StreamWriter(req.GetRequestStream())) sw.Write($"{data}");

            using (var sr = new StreamReader(req.GetResponse().GetResponseStream())) return await sr.ReadToEndAsync();
        }
    }
}
