using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Aero.Specific;
using Yandex.Metrica.Aides;
using Yandex.Metrica.Legacy;
using Yandex.Metrica.Models;

namespace Yandex.Metrica
{
    public static class YandexMetrica
    {
        private static readonly object ActivationLock = new object();
        private static readonly object CacheLock = new object();
        private static readonly List<ReportMessage.Session.Event> Cache = new List<ReportMessage.Session.Event>();
        public static readonly YandexMetrica.YandexMetricaConfig Config = new YandexMetrica.YandexMetricaConfig();
        private static LiteMetricaService _liteMetricaService;

        static YandexMetrica()
        {
            string current = YandexMetricaFolder.Current;
            if (current == null)
                throw new Exception("You should specify valid 'YandexMetricaFolder.Current' before.");
            Memory.ActiveBox = new Memory((IStorage)new KeyFileStorage(), Path.Combine(current, "Yandex.Metrica.{0}.json"), true, "  ");
            Lifecycler lifecycler = Store.Get<Lifecycler>();
            lifecycler.UnhandledException += (EventHandler)((sender, args) =>
           {
               if (!Adapter.IsInternalException((object)args))
                   return;
               Adapter.TryHandleException((object)args);
               Memory.ActiveBox.Destroy<LiteMetricaService>((string)null);
               Memory.ActiveBox.Destroy<Yandex.Metrica.Models.Config>((string)null);
               Store.Container.Remove(typeof(LiteMetricaService));
               Store.Container.Remove(typeof(Yandex.Metrica.Models.Config));
               YandexMetrica._liteMetricaService = Store.Get<LiteMetricaService>();
           });
            if (!lifecycler.IsBackgroundTask)
                return;
            Memory.ActiveBox = new Memory((IStorage)new KeyFileStorage(), Path.Combine(current, "Yandex.Metrica.{0}.b.json"), true, "  ");
        }

        private static void Report(ReportMessage.Session.Event item)
        {
            if (YandexMetrica.InternalConfig.ApiKey == Guid.Empty)
                throw new ArgumentException("ApiKey is empty");
            if (YandexMetrica._liteMetricaService == null)
            {
                lock (YandexMetrica.CacheLock)
                {
                    if (YandexMetrica._liteMetricaService != null)
                        return;
                    YandexMetrica.Cache.Add(item);
                }
            }
            else
                YandexMetrica._liteMetricaService.Report(item);
        }

        private static void ActivateInternal(Guid apiKey)
        {
            try
            {
                LegacyManager.CompleteMigration().RunSynchronously();
            }
            catch (Exception)
            {
            }
            YandexMetrica.MigrateApiKeys();
            LiteMetricaService liteMetricaService = Store.Get<LiteMetricaService>();
            if (((IEnumerable<Guid>)Critical.GetApiKeys()).Contains<Guid>(apiKey))
            {
                liteMetricaService.Wake(false, true);
            }
            else
            {
                Critical.AddApiKey(apiKey);
                liteMetricaService.Wake(true, true);
                Critical.Submit();
            }
            lock (YandexMetrica.CacheLock)
            {
                liteMetricaService.Report(YandexMetrica.Cache.ToArray());
                YandexMetrica.Cache.Clear();
                YandexMetrica._liteMetricaService = liteMetricaService;
            }
            liteMetricaService.ForceSend = true;
        }

        private static void MigrateApiKeys()
        {
            Guid[] apiKeys = Critical.GetApiKeys();
            Guid[] array = YandexMetrica.InternalConfig.KnownKeys.Where<Guid>((Func<Guid, bool>)(k => !((IEnumerable<Guid>)apiKeys).Contains<Guid>(k))).ToArray<Guid>();
            ((IEnumerable<Guid>)array).ForEach<Guid>(new Action<Guid>(Critical.AddApiKey));
            if (!((IEnumerable<Guid>)array).Any<Guid>())
                return;
            Critical.Submit();
        }

        internal static void Reset()
        {
            string customStartupUrl = Yandex.Metrica.Models.Config.Global.CustomStartupUrl;
            Guid apiKey = Yandex.Metrica.Models.Config.Global.ApiKey;
            Memory.ActiveBox.Destroy<Yandex.Metrica.Models.Config>((string)null);
            Memory.ActiveBox.Destroy<Critical.CriticalConfig>((string)null);
            Memory.ActiveBox.Destroy<LiteMetricaService>((string)null);
            Critical.SetUuid((string)null);
            Store.Container.Remove(typeof(LiteMetricaService));
            Store.Container.Remove(typeof(Yandex.Metrica.Models.Config));
            Yandex.Metrica.Models.Config config = Store.Get<Yandex.Metrica.Models.Config>();
            config.ApiKey = apiKey;
            config.CustomStartupUrl = customStartupUrl;
            config.Snapshot();
            Yandex.Metrica.Models.Config.Global = config;
            YandexMetrica._liteMetricaService = (LiteMetricaService)null;
        }

        internal static Yandex.Metrica.Models.Config InternalConfig
        {
            get
            {
                return Yandex.Metrica.Models.Config.Global;
            }
        }

        public static void ReportEvent(string eventName)
        {
            YandexMetrica.Report(EventFactory.Create(eventName, (string)null));
        }

        public static void ReportEvent(string eventName, string jsonData)
        {
            YandexMetrica.Report(EventFactory.Create(eventName, jsonData));
        }

        public static void ReportEvent<TItem>(string eventName, TItem serializableItem)
        {
            YandexMetrica.Report(EventFactory.Create<TItem>(eventName, serializableItem));
        }

        [Obsolete("ReportUnhandledException is deprecated, it is no longer supported.")]
        public static void ReportUnhandledException(Exception exсeption)
        {
            YandexMetrica.Report(EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_CRASH, exсeption.ToString(), (string)null));
        }

        [Obsolete("ReportError is deprecated, it is no longer supported.")]
        public static void ReportError(string message, Exception exсeption)
        {
            YandexMetrica.Report(EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_ERROR, exсeption.ToString(), message));
        }

        public static void ReportLaunchUri(Uri uri)
        {
            if (uri == (Uri)null || string.IsNullOrEmpty(uri.AbsoluteUri))
                return;
            YandexMetrica.Report(EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_OPEN, "{\"link\":" + uri.ToJson((System.Type)null) + ",\"type\":\"open\"}", (string)null));
        }

        internal static void ReportInternalEvent(
          int type,
          string name,
          string value,
          Dictionary<string, object> environment)
        {
            if (type >= 1 && type <= 99 && (type != 14 && type != 15))
                return;
            byte[] numArray = value == null ? (byte[])null : Encoding.UTF8.GetBytes(value);
            string environment1 = environment != null ? environment.ToJson(JsonProfile.GetCompact(), (System.Type)null, 1) : (string)null;
            YandexMetrica.Report(EventFactory.Create((ReportMessage.Session.Event.EventType)type, numArray, name, environment1));
        }

        public static void Snapshot()
        {
            if (YandexMetrica._liteMetricaService == null)
            {
                for (int index = 0; YandexMetrica._liteMetricaService == null && index < 7; ++index)
                    TaskEx.Delay(TimeSpan.FromMilliseconds(250.0)).Wait();
            }
            if (YandexMetrica._liteMetricaService == null)
                return;
            YandexMetrica._liteMetricaService.Lull();
            YandexMetrica._liteMetricaService.ForceSend = true;
            YandexMetrica._liteMetricaService.Flush();
        }

        public static void Activate(string apiKey)
        {
            YandexMetrica.Activate(new Guid(apiKey));
        }

        public static void Activate(Guid apiKey)
        {
            YandexMetrica.InternalConfig.ApiKey = apiKey;
            Task.Factory.StartNew((Action)(() =>
           {
               lock (YandexMetrica.ActivationLock)
                   YandexMetrica.ActivateInternal(apiKey);
           }));
        }

        [DataContract]
        public class Location
        {
            [DataMember(Name = "lat")]
            public double Lat { get; set; }

            [DataMember(Name = "lon")]
            public double Lon { get; set; }

            [DataMember(Name = "timestamp")]
            public ulong Timestamp { get; set; }

            [DataMember(Name = "precision")]
            public uint Precision { get; set; }

            [DataMember(Name = "direction")]
            public uint Direction { get; set; }

            [DataMember(Name = "speed")]
            public uint Speed { get; set; }

            [DataMember(Name = "altitude")]
            public int Altitude { get; set; }
        }

        public class YandexMetricaConfig
        {
            public Guid ApiKey
            {
                get
                {
                    return YandexMetrica.InternalConfig.ApiKey;
                }
            }

            public Version LibraryVersion
            {
                get
                {
                    return YandexMetrica.InternalConfig.LibraryVersion;
                }
            }

            public bool OfflineMode
            {
                get
                {
                    return YandexMetrica.InternalConfig.OfflineMode;
                }
                set
                {
                    YandexMetrica.InternalConfig.OfflineMode = value;
                }
            }

            public bool CrashTracking
            {
                get
                {
                    return YandexMetrica.InternalConfig.CrashTracking;
                }
                set
                {
                    YandexMetrica.InternalConfig.CrashTracking = value;
                }
            }

            public bool LocationTracking
            {
                get
                {
                    return YandexMetrica.InternalConfig.LocationTracking;
                }
                set
                {
                    YandexMetrica.InternalConfig.LocationTracking = value;
                }
            }

            public string CustomAppId
            {
                get
                {
                    return YandexMetrica.InternalConfig.CustomAppId;
                }
                set
                {
                    YandexMetrica.InternalConfig.CustomAppId = value;
                }
            }

            public Version CustomAppVersion
            {
                get
                {
                    return YandexMetrica.InternalConfig.CustomAppVersion;
                }
                set
                {
                    YandexMetrica.InternalConfig.CustomAppVersion = value;
                }
            }

            public TimeSpan SessionTimeout
            {
                get
                {
                    return YandexMetrica.InternalConfig.SessionTimeout;
                }
                set
                {
                    YandexMetrica.InternalConfig.SetSessionTimeout(value);
                }
            }

            public bool HandleFirstActivationAsUpdate
            {
                get
                {
                    return YandexMetrica.InternalConfig.HandleFirstActivationAsUpdate;
                }
                set
                {
                    YandexMetrica.InternalConfig.HandleFirstActivationAsUpdate = value;
                }
            }

            public void SetCustomLocation(YandexMetrica.Location location)
            {
                YandexMetrica.InternalConfig.SetCustomLocation(location);
            }
        }
    }
}
