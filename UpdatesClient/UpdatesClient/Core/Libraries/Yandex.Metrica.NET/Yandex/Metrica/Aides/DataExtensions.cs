// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Aides.DataExtensions
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Json;
using System.Text;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Models;

namespace Yandex.Metrica.Aides
{
  internal static class DataExtensions
  {
    public static string GlueGetList(
      this string url,
      Dictionary<string, object> args,
      bool removeLastAmp = true)
    {
      StringBuilder builder = new StringBuilder(url);
      bool flag = false;
      foreach (KeyValuePair<string, object> keyValuePair in args)
      {
        if (keyValuePair.Value != null)
        {
          builder.Append<string>(keyValuePair.Key, "=", keyValuePair.Value.ToString(), "&");
          flag = true;
        }
      }
      if (flag & removeLastAmp)
        builder.Remove(builder.Length - 1, 1);
      return builder.ToString();
    }

    public static string ToJsonString<T>(this T obj) where T : class
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new DataContractJsonSerializer(typeof (T)).WriteObject((Stream) memoryStream, (object) obj);
        byte[] array = memoryStream.ToArray();
        return Encoding.UTF8.GetString(array, 0, array.Length);
      }
    }

    public static string ToProtobufString<T>(this T obj) where T : ReportMessage
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        ReportMessage.Serialize((Stream) memoryStream, (ReportMessage) obj);
        byte[] array = memoryStream.ToArray();
        return Encoding.UTF8.GetString(array, 0, array.Length);
      }
    }

    public static void Write(this ReportMessage message, Stream requestStream)
    {
      using (GZipStream gzipStream = new GZipStream(requestStream, CompressionMode.Compress, true))
        ReportMessage.Serialize((Stream) gzipStream, message);
    }
  }
}
