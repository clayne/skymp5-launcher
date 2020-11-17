using Sentry;
using System;
using System.IO;
using UpdatesClient.Modules.Configs;
using Yandex.Metrica;

namespace UpdatesClient.Core
{
    public static class Logger
    {
        public static void Init(Version version)
        {
            SentrySdk.Init(options =>
            {
                options.Dsn = new Dsn("https://13d9192e33aa4e86a8f9a55d89d5ffc5:2def384d727d457482cf641f234a2fc8@sentry.skyrez.su/4");
                options.Release = version.ToString();
            });

            string tmpPath = Settings.PathToLocalTmp;
            if (!Directory.Exists(tmpPath)) Directory.CreateDirectory(tmpPath);
            YandexMetricaFolder.SetCurrent(tmpPath);
            YandexMetrica.Config.CustomAppVersion = version;
        }

        public static void SetUser(int id, string userName)
        {
            SentrySdk.ConfigureScope(scope =>
            {
                scope.SetTag("Locale", Settings.Locale);
                scope.User = new Sentry.Protocol.User()
                {
                    Id = id.ToString(),
                    Username = userName
                };
            });
        }

        public static void Error(string message, Exception exception)
        {
            SentryEvent sentryEvent = new SentryEvent(exception)
            {
                Message = message,
                Level = Sentry.Protocol.SentryLevel.Error,
            };

            SentrySdk.CaptureEvent(sentryEvent);
        }
        public static void FatalError(string message, Exception exception)
        {
            SentryEvent sentryEvent = new SentryEvent(exception)
            {
                Message = message,
                Level = Sentry.Protocol.SentryLevel.Fatal
            };

            SentrySdk.CaptureEvent(sentryEvent);
        }


        public static void Event(string Message)
        {
            SentrySdk.CaptureMessage(Message);
        }
    }
}
