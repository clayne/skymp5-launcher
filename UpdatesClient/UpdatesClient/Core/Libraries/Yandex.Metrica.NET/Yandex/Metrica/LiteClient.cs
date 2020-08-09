// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.LiteClient
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using Yandex.Metrica.Aides;
using Yandex.Metrica.Models;

namespace Yandex.Metrica
{
    internal static class LiteClient
    {
        public static async Task<HttpResponseMessage> PostAsync(
          this ReportPackage package)
        {
            return await LiteClient.PostAsync(new Uri(Config.Global.ReportUrl + "/report?".GlueGetList(new Dictionary<string, object>()
      {
        {
          "deviceid",
          (object) Critical.GetDeviceId()
        },
        {
          "uuid",
          (object) Critical.GetUuid()
        }
      }, false) + package.UrlParameters), (Stream)new MemoryStream(package.GetRawStream().ToArray()));
        }

        public static async Task<bool> RefreshStartupAsync()
        {
            StartupResponse startupAsync = await LiteClient.GetStartupAsync();
            if (startupAsync == null)
                return Config.Global.ReportUrl != null;
            if (startupAsync.DeviceId != null)
                Critical.SetDeviceId(startupAsync.DeviceId);
            if (startupAsync.Uuid != null)
                Critical.SetUuid(startupAsync.Uuid);
            Config.Global.ReportUrl = startupAsync.ReportUrl;
            return true;
        }

        public static async Task<HttpResponseMessage> PostAsync(
          Uri uri,
          Stream stream)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("user-agent", ServiceData.UserAgent);
                StreamContent streamContent = new StreamContent(stream);
                streamContent.Headers.ContentEncoding.Add("gzip");
                using (HttpResponseMessage response = await httpClient.PostAsync(uri, (HttpContent)streamContent, CancellationToken.None))
                {
                    if (response.IsSuccessStatusCode)
                        return response;
                    if (await response.Content.ReadAsStringAsync() == "Incorrect uuid")
                        Critical.SetUuid((string)null);
                    return response;
                }
            }
            catch (Exception)
            {
                return (HttpResponseMessage)null;
            }
        }

        private static async Task<StartupResponse> GetStartupAsync()
        {
            try
            {
                Uri baseUri = new Uri(Config.Global.CustomStartupUrl ?? "https://startup.mobile.yandex.net/");
                string str1 = "analytics/startup?query_hosts=1&".GlueGetList(await ServiceData.GetStartupParameters(), true);
                if (Critical.GetUuid() != null)
                    str1 = str1 + "&uuid=" + Critical.GetUuid();
                Uri requestUri = new Uri(baseUri, str1 + "&deviceid=" + Critical.GetDeviceId());
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("user-agent", ServiceData.UserAgent);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                using (HttpResponseMessage response = await httpClient.GetAsync(requestUri, CancellationToken.None))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        string str2 = await response.Content.ReadAsStringAsync();
                        return (StartupResponse)null;
                    }
                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    {
                        StartupResponse startupResponse1 = stream == null ? (StartupResponse)null : new DataContractJsonSerializer(typeof(StartupResponse)).ReadObject(stream) as StartupResponse;
                        if (startupResponse1 == null)
                            return (StartupResponse)null;
                        StartupResponse startupResponse2 = startupResponse1;
                        DateTimeOffset? date = response.Headers.Date;
                        DateTimeOffset? local = date;
                        DateTime dateTime = local.HasValue ? local.GetValueOrDefault().DateTime : DateTime.UtcNow;
                        startupResponse2.ServerDateTime = dateTime;
                        startupResponse1.ServerTimeOffset = startupResponse1.ServerDateTime.ToUniversalTime() - DateTime.UtcNow;
                        return startupResponse1;
                    }
                }
            }
            catch (Exception)
            {
                return (StartupResponse)null;
            }
        }

        internal class HttpRequestHeaders
        {
            public const string Accept = "accept";
            public const string ServerDate = "date";
            public const string UserAgent = "user-agent";
        }

        internal class HttpRequestMethods
        {
            public const string Get = "GET";
            public const string Post = "POST";
        }
    }
}
