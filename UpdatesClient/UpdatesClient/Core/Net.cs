using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UpdatesClient.Modules.Configs;
using Yandex.Metrica;

namespace UpdatesClient.Core
{
    public class Net
    {
        public const string URL_Version = "https://skymp.io/api/latest_version";
        public const string URL_SKSELink = "https://skymp.io/api/skse_link/{VERSION}";
        public const string URL_ModLink = "https://skymp.io/api/skymp_link/{VERSION}";

        public const string URL_CrashDmp = "https://skymp.io/api/crashes";

        public const string URL_Lib = "https://skymp.skyrez.su/libs/7z.dll";
        public const string URL_Mod_RuFix = "https://skymp.skyrez.su/mods/SSERuFixConsole.zip";

        public static async Task<bool> UpdateAvailable()
        {
            string result = await Request($"{URL_Version}", null);

            return ModVersion.Version != result;
        }

        public static async Task<(string, string)> GetUrlToClient()
        {
            string ver = await Request($"{URL_Version}", null);
            string link = await Request(URL_ModLink.Replace("{VERSION}", ver), null);
            return (link, ver);
        }

        public static async Task<string> GetUrlToSKSE()
        {
            string ver = await Request($"{URL_Version}", null);
            string link = await Request(URL_SKSELink.Replace("{VERSION}", ver), null);
            return link;
        }

        public static async Task<bool> ReportDmp(string pathToFile)
        {
            using(FileStream fs = new FileStream(pathToFile, FileMode.Open, FileAccess.Read))
            {
                string req = await UploadRequest(URL_CrashDmp, null, new FileInfo(pathToFile).Name, fs, "crashdmp", "application/x-www-form-urlencoded");
                if (req == "OK")
                {
                    return true;
                }
                else
                {
                    YandexMetrica.ReportError("ReportDmp_Net", new Exception(req));
                    return false;
                }
            }
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

        private static async Task<string> UploadRequest(string url, string data, string fileName, Stream stream, string paramName, string contentType)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;

            using (Stream requestStream = request.GetRequestStream())
            {
                if (data != null)
                {
                    foreach (string d in data.Split('&'))
                    {
                        (string, string) pair = (d.Split('=')[0], d.Split('=')[1]);
                        requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                        string header1 = $"Content-Disposition: form-data; name=\"{pair.Item1}\" \r\n\r\n";
                        byte[] headerbytes1 = Encoding.UTF8.GetBytes(header1);
                        requestStream.Write(headerbytes1, 0, headerbytes1.Length);

                        byte[] bdata = Encoding.UTF8.GetBytes(pair.Item2);
                        requestStream.Write(bdata, 0, bdata.Length);
                    }
                }

                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                string header = $"Content-Disposition: form-data; name=\"{paramName}\"; filename=\"{fileName}.dmp\"\r\nContent-Type: {contentType}\r\n\r\n";
                byte[] headerbytes = Encoding.UTF8.GetBytes(header);
                requestStream.Write(headerbytes, 0, headerbytes.Length);

                stream.CopyTo(requestStream);

                byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                requestStream.Write(trailer, 0, trailer.Length);
            }

            using (var sr = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                return await sr.ReadToEndAsync();
            }
        }
    }
}
