using System;

namespace Windows.Devices.Geolocation
{
    public sealed class Geocoordinate
    {
        public double Accuracy { get; set; }

        public double? Altitude { get; set; }

        public double? AltitudeAccuracy { get; set; }

        public double? Heading { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double? Speed { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}
