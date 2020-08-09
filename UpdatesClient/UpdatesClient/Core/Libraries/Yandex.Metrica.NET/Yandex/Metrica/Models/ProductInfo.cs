using System;
using System.Runtime.Serialization;

namespace Yandex.Metrica.Models
{
    [DataContract]
    internal class ProductInfo
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public Version Version { get; set; }
    }
}
