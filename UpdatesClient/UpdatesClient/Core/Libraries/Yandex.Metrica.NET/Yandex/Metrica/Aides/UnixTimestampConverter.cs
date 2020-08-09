using System;

namespace Yandex.Metrica.Aides
{
    internal static class UnixTimestampConverter
    {
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public static DateTime ToDateTime(this uint value)
        {
            return UnixTimestampConverter.Epoch.AddSeconds((double)value);
        }

        public static DateTime ToDateTime(this ulong value)
        {
            return UnixTimestampConverter.Epoch.AddSeconds((double)value);
        }

        public static ulong ToUnixTime(this DateTime value)
        {
            return (ulong)(value.ToUniversalTime() - UnixTimestampConverter.Epoch).TotalSeconds;
        }
    }
}
