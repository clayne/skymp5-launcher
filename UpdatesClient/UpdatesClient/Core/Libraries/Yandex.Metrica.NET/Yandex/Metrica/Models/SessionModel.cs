using System.Runtime.Serialization;

namespace Yandex.Metrica.Models
{
    [DataContract]
    internal class SessionModel : ReportMessage.Session
    {
        [DataMember]
        public ulong EventCounter;

        public bool AsyncLocationLock { get; set; }

        [DataMember]
        public string ReportParameters { get; set; }

        [DataMember]
        public ulong? LastUpdateTimestamp { get; set; }

        [DataMember]
        public ulong? LastEventTimestamp { get; set; }

        [DataMember]
        public ulong? LastEventType { get; set; }
    }
}
