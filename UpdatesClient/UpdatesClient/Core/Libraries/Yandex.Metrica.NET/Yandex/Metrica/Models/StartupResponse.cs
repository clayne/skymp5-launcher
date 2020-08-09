using System;
using System.Runtime.Serialization;

namespace Yandex.Metrica.Models
{
    [DataContract]
    internal class StartupResponse
    {
        [DataMember(Name = "query_hosts")]
        public StartupResponse.HostContainer QueryHosts { get; set; }

        [DataMember(Name = "uuid")]
        public StartupResponse.ValueContainer UuidContainer { get; set; }

        [DataMember(Name = "device_id")]
        public StartupResponse.ValueContainer DeviceIdContainer { get; set; }

        [DataMember]
        public TimeSpan ServerTimeOffset { get; set; }

        [DataMember]
        public DateTime ServerDateTime { get; set; }

        public string Uuid
        {
            get
            {
                return this.UuidContainer?.Value;
            }
        }

        public string DeviceId
        {
            get
            {
                return this.DeviceIdContainer?.Value;
            }
        }

        public string ReportUrl
        {
            get
            {
                return this.QueryHosts?.List?.Report?.Url;
            }
        }

        [DataContract]
        internal class ValueContainer
        {
            [DataMember(Name = "value")]
            public string Value { get; set; }
        }

        [DataContract]
        internal class HostContainer
        {
            [DataMember(Name = "list")]
            internal StartupResponse.HostContainer.HostList List { get; set; }

            [DataContract]
            internal class HostList
            {
                [DataMember(Name = "report")]
                public StartupResponse.HostContainer.HostList.Host Report { get; set; }

                [DataContract]
                public class Host
                {
                    [DataMember(Name = "url")]
                    public string Url { get; set; }
                }
            }
        }
    }
}
