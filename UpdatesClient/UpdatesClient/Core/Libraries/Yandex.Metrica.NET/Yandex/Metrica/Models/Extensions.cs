using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Devices.Geolocation;
using Yandex.Metrica.Aides;

namespace Yandex.Metrica.Models
{
    internal static class Extensions
    {
        public const int MaxValueLength = 50000;

        public static byte[] ToEventValue(this string value, out bool isTruncated)
        {
            byte[] numArray = value == null ? (byte[])null : Encoding.UTF8.GetBytes(value);
            isTruncated = numArray != null && numArray.Length > 50000;
            return isTruncated ? ((IEnumerable<byte>)numArray).Take<byte>(50000).ToArray<byte>() : numArray;
        }

        public static List<ReportPackage> ToReportPackages(
          this List<SessionModel> sessions)
        {
            Dictionary<SessionModel, string> dictionary1 = sessions.ToDictionary<SessionModel, SessionModel, string>((Func<SessionModel, SessionModel>)(s => s), (Func<SessionModel, string>)(s => s.ReportParameters));
            Dictionary<string, List<ReportMessage.Session>> dictionary2 = dictionary1.Values.Distinct<string>().ToDictionary<string, string, List<ReportMessage.Session>>((Func<string, string>)(p => p), (Func<string, List<ReportMessage.Session>>)(p => new List<ReportMessage.Session>()));
            foreach (KeyValuePair<SessionModel, string> keyValuePair in dictionary1)
                dictionary2[keyValuePair.Value].Add((ReportMessage.Session)keyValuePair.Key);
            List<ReportPackage> list1 = dictionary2.Select<KeyValuePair<string, List<ReportMessage.Session>>, ReportPackage>((Func<KeyValuePair<string, List<ReportMessage.Session>>, ReportPackage>)(i => new ReportPackage(i.Key, (IEnumerable<ReportMessage.Session>)i.Value))).ToList<ReportPackage>();
            label_6:
            List<ReportPackage> list2 = list1.Where<ReportPackage>((Func<ReportPackage, bool>)(p => p.IsLarge)).ToList<ReportPackage>();
            if (list2.Count == 0)
                return list1;
            using (List<ReportPackage>.Enumerator enumerator = list2.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ReportPackage current = enumerator.Current;
                    list1.Remove(current);
                    list1.AddRange((IEnumerable<ReportPackage>)current.Split());
                }
                goto label_6;
            }
        }

        public static ReportMessage.Location ToMetricaLocation(
          this Geocoordinate geocoordinate)
        {
            return new ReportMessage.Location()
            {
                lat = geocoordinate.Latitude,
                lon = geocoordinate.Longitude,
                precision = new uint?((uint)geocoordinate.Accuracy),
                timestamp = new ulong?(geocoordinate.Timestamp.DateTime.ToUnixTime()),
                speed = new uint?(!geocoordinate.Speed.HasValue ? 0U : (uint)geocoordinate.Speed.Value),
                direction = new uint?(!geocoordinate.Heading.HasValue ? 0U : (uint)geocoordinate.Heading.Value),
                altitude = new int?(!geocoordinate.Altitude.HasValue ? 0 : (int)geocoordinate.Altitude.Value)
            };
        }
    }
}
