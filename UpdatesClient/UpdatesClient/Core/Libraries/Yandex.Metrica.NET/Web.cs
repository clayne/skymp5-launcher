using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Yandex.Metrica.Aides;

public static class Web
{
    public static async Task<string> Request(
      string address,
      string method = "GET",
      object content = null,
      string contentType = "application/json")
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
        request.Method = method;
        if (content != null)
        {
            request.ContentType = contentType;
            using (Stream stream = await Task.Factory.FromAsync<Stream>(new Func<AsyncCallback, object, IAsyncResult>(((WebRequest)request).BeginGetRequestStream), new Func<IAsyncResult, Stream>(((WebRequest)request).EndGetRequestStream), (object)null))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(content.ToJson((Type)null));
                stream.Write(bytes, 0, bytes.Length);
            }
        }
        string end;
        using (HttpWebResponse httpWebResponse = (HttpWebResponse)await Task.Factory.FromAsync<WebResponse>(new Func<AsyncCallback, object, IAsyncResult>(((WebRequest)request).BeginGetResponse), new Func<IAsyncResult, WebResponse>(((WebRequest)request).EndGetResponse), (object)null))
        {
            using (Stream responseStream = httpWebResponse.GetResponseStream())
            {
                using (StreamReader streamReader = new StreamReader(responseStream ?? (Stream)new MemoryStream(), Encoding.UTF8))
                    end = streamReader.ReadToEnd();
            }
        }
        return end;
    }
}
