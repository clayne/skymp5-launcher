using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatesClient.Core
{
    public static class Logger
    {
        public static void Init(Version version)
        {
            SentrySdk.Init(options =>
            {
                options.Dsn = new Dsn("https://13d9192e33aa4e86a8f9a55d89d5ffc5@sentry.skyrez.su/4");
                options.Release = version.ToString();
            });
        }

        public static void SetUser(string email, string userName)
        {
            SentrySdk.ConfigureScope(scope =>
            {
                scope.User = new Sentry.Protocol.User()
                {
                    Email = email,
                    Username = userName
                };
            });
        }

        public static void Error(Exception exception)
        {
            SentrySdk.CaptureException(exception);
        }

        public static void Event(string Message)
        {
            SentrySdk.CaptureMessage(Message);
        }
    }
}
