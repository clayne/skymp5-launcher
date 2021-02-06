using Sentry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.Configs.Helpers;
using UpdatesClient.Modules.SelfUpdater;
using Yandex.Metrica;

namespace UpdatesClient.Core
{
    public static class Logger
    {
        private static string uri;
        private static string AssemblyShortName
        {
            get
            {
                if (_assemblyShortName == null)
                {
                    Assembly a = typeof(Logger).Assembly;
                    _assemblyShortName = a.ToString().Split(',')[0];
                }
                return _assemblyShortName;
            }
        }
        private static string _assemblyShortName;

        private static string md5Luancher;

        public static void Init(Version version)
        {
            try
            {
                StringBuilder uriString = new StringBuilder();
                uriString.Append("pack://application:,,,");
                uriString.Append("/" + AssemblyShortName + ";component/{path}");
                uri = uriString.ToString();
                md5Luancher = Hashing.GetMD5FromFile(File.OpenRead(Assembly.GetExecutingAssembly().Location));
            }
            catch { }
            
            SentrySdk.Init(options =>
            {
                options.Dsn = new Dsn("https://13d9192e33aa4e86a8f9a55d89d5ffc5:2def384d727d457482cf641f234a2fc8@sentry.skyrez.su/4");
                options.Release = version.ToString();
                options.BeforeSend = SentryEvent;
            });

            string tmpPath = Settings.PathToLocalTmp;
            if (!Directory.Exists(tmpPath)) Directory.CreateDirectory(tmpPath);
            YandexMetricaFolder.SetCurrent(tmpPath);
            ExperimentalFunctions.IfUse("SetVers", () =>
            {
                Version nv = new Version(version.Major, version.Minor, version.Build, version.Revision+100);
                YandexMetrica.Config.CustomAppVersion = nv;
            }, () =>
            {
                YandexMetrica.Config.CustomAppVersion = version;
            });
        }


        private static SentryEvent SentryEvent(SentryEvent e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Settings.UserName) && Settings.UserId != 0)
                {
                    e.User = new Sentry.Protocol.User()
                    {
                        Id = Settings.UserId.ToString(),
                        Username = Settings.UserName
                    };
                }
                e.SetTag("Locale", Settings.Locale);
                e.SetTag("URI", uri);
                e.SetTag("MD5", md5Luancher);
                e.SetTag("ExperimentalFunctions", Settings.ExperimentalFunctions?.ToString());
            }
            catch { }

            return e;
        }

        public static void Error(string message, Exception exception, IEnumerable<KeyValuePair<string, string>> extraTags = null)
        {
            SentryEvent sentryEvent = new SentryEvent(exception)
            {
                Message = message,
                Level = Sentry.Protocol.SentryLevel.Error,
            };
            if (extraTags != null) sentryEvent.SetTags(extraTags);
            sentryEvent.SetTag("HResult", exception.HResult.ToString());

            SentrySdk.CaptureEvent(sentryEvent);
        }

        public static void FatalError(string message, Exception exception)
        {
            SentryEvent sentryEvent = new SentryEvent(exception)
            {
                Message = message,
                Level = Sentry.Protocol.SentryLevel.Fatal
            };
            sentryEvent.SetTag("HResult", exception.HResult.ToString());

            SentrySdk.CaptureEvent(sentryEvent);
        }


        public static void Event(string Message)
        {
            SentrySdk.CaptureMessage(Message);
        }
    }
}
