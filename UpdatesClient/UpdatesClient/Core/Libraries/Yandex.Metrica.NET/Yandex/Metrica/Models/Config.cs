using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Yandex.Metrica.Aero;

namespace Yandex.Metrica.Models
{
    [DataContract]
    internal class Config : IExposable
    {
        private static readonly TimeSpan MinSessionTimeout = TimeSpan.FromSeconds(10.0);
        private static readonly TimeSpan MinDispatchPeriod = TimeSpan.FromSeconds(4.0);
        private static readonly int MinFlushThresholdEventsCounts = 7;
        private static readonly TimeSpan MinFlushThresholdTimeout = TimeSpan.FromSeconds(4.0);
        private static readonly TimeSpan MinStartupExpirationTimeSpan = TimeSpan.FromDays(1.0);
        private static readonly TimeSpan MinIdentitySendIntervalTimeSpan = TimeSpan.FromDays(1.0);

        public static Config Global { get; internal set; } = Store.Get<Config>();

        void IExposable.Expose()
        {
            this.IsNew = this.Id == Guid.Empty;
            this.Id = this.IsNew ? Guid.NewGuid() : this.Id;
            this.KnownKeys = this.KnownKeys ?? new List<Guid>();
            this.SessionTimeout = this.IsNew ? Config.MinSessionTimeout : this.SessionTimeout;
            this.DispatchPeriod = this.IsNew ? Config.MinDispatchPeriod : this.DispatchPeriod;
            this.MaxCacheSize = this.IsNew ? 5242880L : this.MaxCacheSize;
            this.StartupExpirationTimeSpan = this.IsNew ? Config.MinStartupExpirationTimeSpan : this.StartupExpirationTimeSpan;
            this.StartupTimestamp = this.IsNew ? DateTime.UtcNow - this.StartupExpirationTimeSpan : this.StartupTimestamp;
            this.IdentitySendInterval = this.IsNew ? Config.MinIdentitySendIntervalTimeSpan : this.IdentitySendInterval;
            this.IdentityTimestamp = this.IsNew ? DateTime.UtcNow - this.IdentitySendInterval : this.IdentityTimestamp;
            this.CrashTracking = this.IsNew || this.CrashTracking;
            this.LocationTracking = this.IsNew || this.LocationTracking;
            this.FlushThresholdEventsCounts = this.IsNew ? 7 : this.FlushThresholdEventsCounts;
            this.FlushThresholdTimeout = this.IsNew ? TimeSpan.FromSeconds(90.0) : this.FlushThresholdTimeout;
            this.JsonSerializerSettings = new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true
            };
            this.CheckValues();
        }

        private void CheckValues()
        {
            this.SessionTimeout = this.SessionTimeout < Config.MinSessionTimeout ? Config.MinSessionTimeout : this.SessionTimeout;
            this.DispatchPeriod = this.DispatchPeriod < Config.MinDispatchPeriod ? Config.MinDispatchPeriod : this.DispatchPeriod;
            this.FlushThresholdEventsCounts = this.FlushThresholdEventsCounts < Config.MinFlushThresholdEventsCounts ? Config.MinFlushThresholdEventsCounts : this.FlushThresholdEventsCounts;
            this.FlushThresholdTimeout = this.FlushThresholdTimeout < Config.MinFlushThresholdTimeout ? Config.MinFlushThresholdTimeout : this.FlushThresholdTimeout;
            this.StartupExpirationTimeSpan = this.StartupExpirationTimeSpan < Config.MinStartupExpirationTimeSpan ? Config.MinStartupExpirationTimeSpan : this.StartupExpirationTimeSpan;
            this.IdentitySendInterval = this.IdentitySendInterval < Config.MinIdentitySendIntervalTimeSpan ? Config.MinIdentitySendIntervalTimeSpan : this.IdentitySendInterval;
        }

        [DataMember]
        public Guid ApiKey { get; internal set; }

        [DataMember]
        public bool OfflineMode { get; set; }

        [DataMember]
        public bool CrashTracking { get; set; }

        [DataMember]
        public bool LocationTracking { get; set; }

        [DataMember]
        public string CustomAppId { get; set; }

        [DataMember]
        public Version CustomAppVersion { get; set; }

        [DataMember]
        public TimeSpan SessionTimeout { get; private set; }

        [DataMember]
        public bool HandleFirstActivationAsUpdate { get; set; }

        [DataMember]
        public long MaxCacheSize { get; set; }

        [DataMember]
        public Guid Id { get; private set; }

        [DataMember]
        public List<Guid> KnownKeys { get; set; }

        [DataMember]
        public TimeSpan StartupExpirationTimeSpan { get; set; }

        [DataMember]
        public DateTime StartupTimestamp { get; set; }

        [DataMember]
        public TimeSpan IdentitySendInterval { get; set; }

        [DataMember]
        public DateTime IdentityTimestamp { get; set; }

        [DataMember]
        public DateTime? LastWakeTime { get; set; }

        [DataMember]
        public DateTime? LastLullTime { get; set; }

        [DataMember]
        public TimeSpan DispatchPeriod { get; set; }

        [DataMember]
        public int FlushThresholdEventsCounts { get; private set; }

        [DataMember]
        public TimeSpan FlushThresholdTimeout { get; private set; }

        [DataMember]
        public string CustomStartupUrl { get; set; }

        [DataMember]
        public string ReportUrl { get; set; }

        internal bool IsNew { get; private set; }

        internal DataContractJsonSerializerSettings JsonSerializerSettings { get; set; }

        public Version LibraryVersion
        {
            get
            {
                return new Version("3.5.1");
            }
        }

        [DataMember]
        public ReportMessage.Location CustomLocation { get; set; }

        public void SetCustomLocation(YandexMetrica.Location location)
        {
            ReportMessage.Location location1;
            if (location != null)
                location1 = new ReportMessage.Location()
                {
                    lat = location.Lat,
                    lon = location.Lon,
                    speed = new uint?(location.Speed),
                    altitude = new int?(location.Altitude),
                    direction = new uint?(location.Direction),
                    precision = new uint?(location.Precision),
                    timestamp = new ulong?(location.Timestamp)
                };
            else
                location1 = (ReportMessage.Location)null;
            this.CustomLocation = location1;
        }

        public void SetSessionTimeout(TimeSpan value)
        {
            this.SessionTimeout = value;
            this.CheckValues();
        }

        public void SetFlushThresholdEventsCounts(int value)
        {
            this.FlushThresholdEventsCounts = value;
            this.CheckValues();
        }

        public void SetFlushThresholdTimeout(TimeSpan value)
        {
            this.FlushThresholdTimeout = value;
            this.CheckValues();
        }

        internal static string GetLocale()
        {
            return Config.GetSpecificName(CultureInfo.CurrentUICulture, RegionInfo.CurrentRegion.TwoLetterISORegionName);
        }

        internal static string GetSpecificName(CultureInfo culture, string regionName)
        {
            if (!culture.IsNeutralCulture)
                return culture.Name;
            string str = culture.TextInfo.CultureName;
            if (str == culture.Name)
                str = culture.TwoLetterISOLanguageName + "-" + regionName;
            return str;
        }
    }
}
