﻿using Sentry;
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
                options.BeforeSend = SentryEvent;
            });

            string tmpPath = Settings.PathToLocalTmp;
            if (!Directory.Exists(tmpPath)) Directory.CreateDirectory(tmpPath);
            YandexMetricaFolder.SetCurrent(tmpPath);
            YandexMetrica.Config.CustomAppVersion = version;
        }


        private static SentryEvent SentryEvent(SentryEvent e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Settings.UserName))
                {
                    e.User = new Sentry.Protocol.User()
                    {
                        Id = Settings.UserId.ToString(),
                        Username = Settings.UserName
                    };
                }
                e.SetTag("Locale", Settings.Locale);
            } catch { }
            
            return e;
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
