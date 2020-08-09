// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.ServiceData
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Aides;
using Yandex.Metrica.Models;
using Yandex.Metrica.Patterns;
using Yandex.Metrica.Properties;
using Yandex.Metrica.Providers;

namespace Yandex.Metrica
{
  internal static class ServiceData
  {
    public const string StartupUrl = "https://startup.mobile.yandex.net/";
    public const string SdkName = "com.yandex.mobile.metrica.sdk";
    public const string UnknownValue = "unknown";

    public static ProductInfo Product { get; private set; }

    public static DeviceProperties Device { get; private set; }

    public static NetworkDataProvider NetworkTracker { get; private set; }

    public static LocationDataProvider LocationTracker { get; internal set; }

    public static ILifecycler Lifecycler { get; private set; }

    public static string DeviceFingerprint { get; private set; }

    public static string UserAgent { get; private set; }

    public static bool IsExposed { get; private set; }

    static ServiceData()
    {
      ServiceData.Expose();
    }

    private static async void Expose()
    {
      ServiceData.Lifecycler = (ILifecycler) Store.Get<Yandex.Metrica.Lifecycler>();
      ServiceData.Product = Identification.GetProductInfo();
      ServiceData.Device = await Store.Get<DeviceDataProvider>().Provide();
      ServiceData.DeviceFingerprint = await Store.Get<FingerprintDataProvider>().Provide();
      ServiceData.NetworkTracker = Store.Get<NetworkDataProvider>();
      ServiceData.LocationTracker = Store.Get<LocationDataProvider>();
      ServiceData.UserAgent = string.Format("{0}/{1}.{2} ", (object) "com.yandex.mobile.metrica.sdk", (object) "3.5.1", (object) 246) + string.Format("({0} {1}; {2} {3})", (object) ServiceData.Device.Manufacturer, (object) ServiceData.Device.ModelName, (object) ServiceData.Device.OSPlatform, (object) ServiceData.Device.OSVersion);
      ServiceData.IsExposed = true;
    }

    public static async Task WaitExposeAsync()
    {
      if (ServiceData.IsExposed)
        return;
      await TaskEx.Run((Action) (() =>
      {
        while (!ServiceData.IsExposed)
          TaskEx.Delay(TimeSpan.FromMilliseconds(250.0)).Wait();
      }));
    }

    private static async Task<Dictionary<string, object>> GetCommonRequestParameters()
    {
      await ServiceData.WaitExposeAsync();
      return new Dictionary<string, object>()
      {
        {
          "app_id",
          (object) (string.IsNullOrWhiteSpace(Config.Global.CustomAppId) ? ServiceData.Product.Id : Config.Global.CustomAppId)
        },
        {
          "model",
          (object) ServiceData.Device.ModelName
        },
        {
          "locale",
          (object) Config.GetLocale()
        },
        {
          "os_version",
          (object) ServiceData.Device.OSVersion.ToString()
        },
        {
          "manufacturer",
          (object) ServiceData.Device.Manufacturer
        },
        {
          "app_platform",
          (object) ServiceData.Device.StartupPlatform
        },
        {
          "screen_dpi",
          (object) ServiceData.Device.Dpi
        },
        {
          "scalefactor",
          (object) ServiceData.Device.ScaleFactor.ToString((IFormatProvider) CultureInfo.InvariantCulture)
        },
        {
          "screen_width",
          (object) ServiceData.Device.ScreenWidth.ToString((IFormatProvider) CultureInfo.InvariantCulture)
        },
        {
          "screen_height",
          (object) ServiceData.Device.ScreenHeight.ToString((IFormatProvider) CultureInfo.InvariantCulture)
        },
        {
          "analytics_sdk_version",
          (object) 351
        }
      };
    }

    public static async Task<Dictionary<string, object>> GetStartupParameters()
    {
      Dictionary<string, object> parameters;
      parameters = await ServiceData.GetCommonRequestParameters();
      new Dictionary<string, object>()
      {
        {
          "protocol_version",
          (object) 2
        },
        {
          "analytics_sdk_version_name",
          (object) "3.5.1"
        }
      }.ForEach<KeyValuePair<string, object>>((Action<KeyValuePair<string, object>>) (p => parameters[p.Key] = p.Value));
      return parameters;
    }

    public static async Task<Dictionary<string, object>> GetReportParameters()
    {
      Dictionary<string, object> parameters;
      parameters = await ServiceData.GetCommonRequestParameters();
      Version version1 = Config.Global.CustomAppVersion;
      if (version1 is null)
        version1 = ServiceData.Product.Version;
      Version version2 = version1;
      new Dictionary<string, object>()
      {
        {
          "api_key_128",
          (object) Config.Global.ApiKey
        },
        {
          "app_framework",
          (object) "native"
        },
        {
          "windows_aid",
          (object) Identification.GetAdvertisingId()
        },
        {
          "device_type",
          (object) ServiceData.Device.GetDeviceType().ToLower()
        },
        {
          "analytics_sdk_build_number",
          (object) 246
        },
        {
          "analytics_sdk_build_type",
          (object) AssemblyProperties.Edition
        },
        {
          "app_build_number",
          (object) version2.Build
        },
        {
          "app_version_name",
          (object) version2.ToString()
        },
        {
          "app_platform",
          (object) "WindowsPhone"
        }
      }.ForEach<KeyValuePair<string, object>>((Action<KeyValuePair<string, object>>) (p => parameters[p.Key] = p.Value));
      return parameters;
    }
  }
}
