using SilentOrbit.ProtocolBuffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Yandex.Metrica.Models
{
    [DataContract]
    internal class ReportMessage
    {
        [DataMember]
        public ReportMessage.Time send_time { get; set; }

        [DataMember]
        public ReportMessage.Time receive_time { get; set; }

        [DataMember]
        public List<ReportMessage.Session> sessions { get; set; }

        [DataMember]
        public ReportMessage.RequestParameters report_request_parameters { get; set; }

        [DataMember]
        public List<ReportMessage.EnvironmentVariable> app_environment { get; set; }

        [DataMember]
        public List<ReportMessage.NetworkInterface> network_interfaces { get; set; }

        [DataMember]
        public List<string> imei { get; set; }

        [DataMember]
        public List<ReportMessage.SimInfo> sim_info { get; set; }

        internal static ReportMessage Deserialize(Stream stream)
        {
            ReportMessage instance = new ReportMessage();
            ReportMessage.Deserialize(stream, instance);
            return instance;
        }

        internal static ReportMessage DeserializeLengthDelimited(Stream stream)
        {
            ReportMessage instance = new ReportMessage();
            ReportMessage.DeserializeLengthDelimited(stream, instance);
            return instance;
        }

        internal static ReportMessage DeserializeLength(Stream stream, int length)
        {
            ReportMessage instance = new ReportMessage();
            ReportMessage.DeserializeLength(stream, length, instance);
            return instance;
        }

        internal static ReportMessage Deserialize(byte[] buffer)
        {
            ReportMessage instance = new ReportMessage();
            using (MemoryStream memoryStream = new MemoryStream(buffer))
                ReportMessage.Deserialize((Stream)memoryStream, instance);
            return instance;
        }

        internal static ReportMessage Deserialize(byte[] buffer, ReportMessage instance)
        {
            using (MemoryStream memoryStream = new MemoryStream(buffer))
                ReportMessage.Deserialize((Stream)memoryStream, instance);
            return instance;
        }

        internal static ReportMessage Deserialize(Stream stream, ReportMessage instance)
        {
            if (instance.sessions == null)
                instance.sessions = new List<ReportMessage.Session>();
            if (instance.app_environment == null)
                instance.app_environment = new List<ReportMessage.EnvironmentVariable>();
            if (instance.network_interfaces == null)
                instance.network_interfaces = new List<ReportMessage.NetworkInterface>();
            if (instance.imei == null)
                instance.imei = new List<string>();
            if (instance.sim_info == null)
                instance.sim_info = new List<ReportMessage.SimInfo>();
            while (true)
            {
                int num = stream.ReadByte();
                switch (num)
                {
                    case -1:
                        goto label_28;
                    case 10:
                        if (instance.send_time == null)
                        {
                            instance.send_time = ReportMessage.Time.DeserializeLengthDelimited(stream);
                            continue;
                        }
                        ReportMessage.Time.DeserializeLengthDelimited(stream, instance.send_time);
                        continue;
                    case 18:
                        if (instance.receive_time == null)
                        {
                            instance.receive_time = ReportMessage.Time.DeserializeLengthDelimited(stream);
                            continue;
                        }
                        ReportMessage.Time.DeserializeLengthDelimited(stream, instance.receive_time);
                        continue;
                    case 26:
                        instance.sessions.Add(ReportMessage.Session.DeserializeLengthDelimited(stream));
                        continue;
                    case 34:
                        if (instance.report_request_parameters == null)
                        {
                            instance.report_request_parameters = ReportMessage.RequestParameters.DeserializeLengthDelimited(stream);
                            continue;
                        }
                        ReportMessage.RequestParameters.DeserializeLengthDelimited(stream, instance.report_request_parameters);
                        continue;
                    case 58:
                        instance.app_environment.Add(ReportMessage.EnvironmentVariable.DeserializeLengthDelimited(stream));
                        continue;
                    case 66:
                        instance.network_interfaces.Add(ReportMessage.NetworkInterface.DeserializeLengthDelimited(stream));
                        continue;
                    case 74:
                        instance.imei.Add(ProtocolParser.ReadString(stream));
                        continue;
                    case 82:
                        instance.sim_info.Add(ReportMessage.SimInfo.DeserializeLengthDelimited(stream));
                        continue;
                    default:
                        Key key = ProtocolParser.ReadKey((byte)num, stream);
                        if (key.Field != 0U)
                        {
                            ProtocolParser.SkipKey(stream, key);
                            continue;
                        }
                        goto label_26;
                }
            }
            label_26:
            throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
            label_28:
            return instance;
        }

        internal static ReportMessage DeserializeLengthDelimited(
          Stream stream,
          ReportMessage instance)
        {
            if (instance.sessions == null)
                instance.sessions = new List<ReportMessage.Session>();
            if (instance.app_environment == null)
                instance.app_environment = new List<ReportMessage.EnvironmentVariable>();
            if (instance.network_interfaces == null)
                instance.network_interfaces = new List<ReportMessage.NetworkInterface>();
            if (instance.imei == null)
                instance.imei = new List<string>();
            if (instance.sim_info == null)
                instance.sim_info = new List<ReportMessage.SimInfo>();
            long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
            while (stream.Position < num1)
            {
                int num2 = stream.ReadByte();
                switch (num2)
                {
                    case -1:
                        throw new EndOfStreamException();
                    case 10:
                        if (instance.send_time == null)
                        {
                            instance.send_time = ReportMessage.Time.DeserializeLengthDelimited(stream);
                            continue;
                        }
                        ReportMessage.Time.DeserializeLengthDelimited(stream, instance.send_time);
                        continue;
                    case 18:
                        if (instance.receive_time == null)
                        {
                            instance.receive_time = ReportMessage.Time.DeserializeLengthDelimited(stream);
                            continue;
                        }
                        ReportMessage.Time.DeserializeLengthDelimited(stream, instance.receive_time);
                        continue;
                    case 26:
                        instance.sessions.Add(ReportMessage.Session.DeserializeLengthDelimited(stream));
                        continue;
                    case 34:
                        if (instance.report_request_parameters == null)
                        {
                            instance.report_request_parameters = ReportMessage.RequestParameters.DeserializeLengthDelimited(stream);
                            continue;
                        }
                        ReportMessage.RequestParameters.DeserializeLengthDelimited(stream, instance.report_request_parameters);
                        continue;
                    case 58:
                        instance.app_environment.Add(ReportMessage.EnvironmentVariable.DeserializeLengthDelimited(stream));
                        continue;
                    case 66:
                        instance.network_interfaces.Add(ReportMessage.NetworkInterface.DeserializeLengthDelimited(stream));
                        continue;
                    case 74:
                        instance.imei.Add(ProtocolParser.ReadString(stream));
                        continue;
                    case 82:
                        instance.sim_info.Add(ReportMessage.SimInfo.DeserializeLengthDelimited(stream));
                        continue;
                    default:
                        Key key = ProtocolParser.ReadKey((byte)num2, stream);
                        if (key.Field == 0U)
                            throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                        ProtocolParser.SkipKey(stream, key);
                        continue;
                }
            }
            if (stream.Position != num1)
                throw new ProtocolBufferException("Read past max limit");
            return instance;
        }

        internal static ReportMessage DeserializeLength(
          Stream stream,
          int length,
          ReportMessage instance)
        {
            if (instance.sessions == null)
                instance.sessions = new List<ReportMessage.Session>();
            if (instance.app_environment == null)
                instance.app_environment = new List<ReportMessage.EnvironmentVariable>();
            if (instance.network_interfaces == null)
                instance.network_interfaces = new List<ReportMessage.NetworkInterface>();
            if (instance.imei == null)
                instance.imei = new List<string>();
            if (instance.sim_info == null)
                instance.sim_info = new List<ReportMessage.SimInfo>();
            long num1 = stream.Position + (long)length;
            while (stream.Position < num1)
            {
                int num2 = stream.ReadByte();
                switch (num2)
                {
                    case -1:
                        throw new EndOfStreamException();
                    case 10:
                        if (instance.send_time == null)
                        {
                            instance.send_time = ReportMessage.Time.DeserializeLengthDelimited(stream);
                            continue;
                        }
                        ReportMessage.Time.DeserializeLengthDelimited(stream, instance.send_time);
                        continue;
                    case 18:
                        if (instance.receive_time == null)
                        {
                            instance.receive_time = ReportMessage.Time.DeserializeLengthDelimited(stream);
                            continue;
                        }
                        ReportMessage.Time.DeserializeLengthDelimited(stream, instance.receive_time);
                        continue;
                    case 26:
                        instance.sessions.Add(ReportMessage.Session.DeserializeLengthDelimited(stream));
                        continue;
                    case 34:
                        if (instance.report_request_parameters == null)
                        {
                            instance.report_request_parameters = ReportMessage.RequestParameters.DeserializeLengthDelimited(stream);
                            continue;
                        }
                        ReportMessage.RequestParameters.DeserializeLengthDelimited(stream, instance.report_request_parameters);
                        continue;
                    case 58:
                        instance.app_environment.Add(ReportMessage.EnvironmentVariable.DeserializeLengthDelimited(stream));
                        continue;
                    case 66:
                        instance.network_interfaces.Add(ReportMessage.NetworkInterface.DeserializeLengthDelimited(stream));
                        continue;
                    case 74:
                        instance.imei.Add(ProtocolParser.ReadString(stream));
                        continue;
                    case 82:
                        instance.sim_info.Add(ReportMessage.SimInfo.DeserializeLengthDelimited(stream));
                        continue;
                    default:
                        Key key = ProtocolParser.ReadKey((byte)num2, stream);
                        if (key.Field == 0U)
                            throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                        ProtocolParser.SkipKey(stream, key);
                        continue;
                }
            }
            if (stream.Position != num1)
                throw new ProtocolBufferException("Read past max limit");
            return instance;
        }

        internal static void Serialize(Stream stream, ReportMessage instance)
        {
            MemoryStream stream1 = ProtocolParser.Stack.Pop();
            if (instance.send_time == null)
                throw new ProtocolBufferException("send_time is required by the proto specification.");
            stream.WriteByte((byte)10);
            stream1.SetLength(0L);
            ReportMessage.Time.Serialize((Stream)stream1, instance.send_time);
            uint length1 = (uint)stream1.Length;
            ProtocolParser.WriteUInt32(stream, length1);
            stream1.WriteTo(stream);
            if (instance.receive_time != null)
            {
                stream.WriteByte((byte)18);
                stream1.SetLength(0L);
                ReportMessage.Time.Serialize((Stream)stream1, instance.receive_time);
                uint length2 = (uint)stream1.Length;
                ProtocolParser.WriteUInt32(stream, length2);
                stream1.WriteTo(stream);
            }
            if (instance.sessions != null)
            {
                foreach (ReportMessage.Session session in instance.sessions)
                {
                    stream.WriteByte((byte)26);
                    stream1.SetLength(0L);
                    ReportMessage.Session.Serialize((Stream)stream1, session);
                    uint length2 = (uint)stream1.Length;
                    ProtocolParser.WriteUInt32(stream, length2);
                    stream1.WriteTo(stream);
                }
            }
            if (instance.report_request_parameters != null)
            {
                stream.WriteByte((byte)34);
                stream1.SetLength(0L);
                ReportMessage.RequestParameters.Serialize((Stream)stream1, instance.report_request_parameters);
                uint length2 = (uint)stream1.Length;
                ProtocolParser.WriteUInt32(stream, length2);
                stream1.WriteTo(stream);
            }
            if (instance.app_environment != null)
            {
                foreach (ReportMessage.EnvironmentVariable instance1 in instance.app_environment)
                {
                    stream.WriteByte((byte)58);
                    stream1.SetLength(0L);
                    ReportMessage.EnvironmentVariable.Serialize((Stream)stream1, instance1);
                    uint length2 = (uint)stream1.Length;
                    ProtocolParser.WriteUInt32(stream, length2);
                    stream1.WriteTo(stream);
                }
            }
            if (instance.network_interfaces != null)
            {
                foreach (ReportMessage.NetworkInterface networkInterface in instance.network_interfaces)
                {
                    stream.WriteByte((byte)66);
                    stream1.SetLength(0L);
                    ReportMessage.NetworkInterface.Serialize((Stream)stream1, networkInterface);
                    uint length2 = (uint)stream1.Length;
                    ProtocolParser.WriteUInt32(stream, length2);
                    stream1.WriteTo(stream);
                }
            }
            if (instance.imei != null)
            {
                foreach (string s in instance.imei)
                {
                    stream.WriteByte((byte)74);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(s));
                }
            }
            if (instance.sim_info != null)
            {
                foreach (ReportMessage.SimInfo instance1 in instance.sim_info)
                {
                    stream.WriteByte((byte)82);
                    stream1.SetLength(0L);
                    ReportMessage.SimInfo.Serialize((Stream)stream1, instance1);
                    uint length2 = (uint)stream1.Length;
                    ProtocolParser.WriteUInt32(stream, length2);
                    stream1.WriteTo(stream);
                }
            }
            ProtocolParser.Stack.Push(stream1);
        }

        internal static byte[] SerializeToBytes(ReportMessage instance)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                ReportMessage.Serialize((Stream)memoryStream, instance);
                return memoryStream.ToArray();
            }
        }

        internal static void SerializeLengthDelimited(Stream stream, ReportMessage instance)
        {
            byte[] bytes = ReportMessage.SerializeToBytes(instance);
            ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
            stream.Write(bytes, 0, bytes.Length);
        }

        public enum OptionalBool
        {
            OPTIONAL_BOOL_UNDEFINED = -1, // 0xFFFFFFFF
            OPTIONAL_BOOL_FALSE = 0,
            OPTIONAL_BOOL_TRUE = 1,
        }

        [DataContract]
        public class Time
        {
            [DataMember]
            public ulong timestamp { get; set; }

            [DataMember]
            public int time_zone { get; set; }

            [DataMember]
            public long? server_time_offset { get; set; }

            public static ReportMessage.Time Deserialize(Stream stream)
            {
                ReportMessage.Time instance = new ReportMessage.Time();
                ReportMessage.Time.Deserialize(stream, instance);
                return instance;
            }

            public static ReportMessage.Time DeserializeLengthDelimited(Stream stream)
            {
                ReportMessage.Time instance = new ReportMessage.Time();
                ReportMessage.Time.DeserializeLengthDelimited(stream, instance);
                return instance;
            }

            public static ReportMessage.Time DeserializeLength(Stream stream, int length)
            {
                ReportMessage.Time instance = new ReportMessage.Time();
                ReportMessage.Time.DeserializeLength(stream, length, instance);
                return instance;
            }

            public static ReportMessage.Time Deserialize(byte[] buffer)
            {
                ReportMessage.Time instance = new ReportMessage.Time();
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    ReportMessage.Time.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            public static ReportMessage.Time Deserialize(
              byte[] buffer,
              ReportMessage.Time instance)
            {
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    ReportMessage.Time.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            public static ReportMessage.Time Deserialize(
              Stream stream,
              ReportMessage.Time instance)
            {
                while (true)
                {
                    int num = stream.ReadByte();
                    switch (num)
                    {
                        case -1:
                            goto label_7;
                        case 8:
                            instance.timestamp = ProtocolParser.ReadUInt64(stream);
                            continue;
                        case 16:
                            instance.time_zone = ProtocolParser.ReadZInt32(stream);
                            continue;
                        case 24:
                            instance.server_time_offset = new long?((long)ProtocolParser.ReadUInt64(stream));
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num, stream);
                            if (key.Field != 0U)
                            {
                                ProtocolParser.SkipKey(stream, key);
                                continue;
                            }
                            goto label_5;
                    }
                }
                label_5:
                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                label_7:
                return instance;
            }

            public static ReportMessage.Time DeserializeLengthDelimited(
              Stream stream,
              ReportMessage.Time instance)
            {
                long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                while (stream.Position < num1)
                {
                    int num2 = stream.ReadByte();
                    switch (num2)
                    {
                        case -1:
                            throw new EndOfStreamException();
                        case 8:
                            instance.timestamp = ProtocolParser.ReadUInt64(stream);
                            continue;
                        case 16:
                            instance.time_zone = ProtocolParser.ReadZInt32(stream);
                            continue;
                        case 24:
                            instance.server_time_offset = new long?((long)ProtocolParser.ReadUInt64(stream));
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num2, stream);
                            if (key.Field == 0U)
                                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            ProtocolParser.SkipKey(stream, key);
                            continue;
                    }
                }
                if (stream.Position != num1)
                    throw new ProtocolBufferException("Read past max limit");
                return instance;
            }

            public static ReportMessage.Time DeserializeLength(
              Stream stream,
              int length,
              ReportMessage.Time instance)
            {
                long num1 = stream.Position + (long)length;
                while (stream.Position < num1)
                {
                    int num2 = stream.ReadByte();
                    switch (num2)
                    {
                        case -1:
                            throw new EndOfStreamException();
                        case 8:
                            instance.timestamp = ProtocolParser.ReadUInt64(stream);
                            continue;
                        case 16:
                            instance.time_zone = ProtocolParser.ReadZInt32(stream);
                            continue;
                        case 24:
                            instance.server_time_offset = new long?((long)ProtocolParser.ReadUInt64(stream));
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num2, stream);
                            if (key.Field == 0U)
                                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            ProtocolParser.SkipKey(stream, key);
                            continue;
                    }
                }
                if (stream.Position != num1)
                    throw new ProtocolBufferException("Read past max limit");
                return instance;
            }

            public static void Serialize(Stream stream, ReportMessage.Time instance)
            {
                MemoryStream stream1 = ProtocolParser.Stack.Pop();
                stream.WriteByte((byte)8);
                ProtocolParser.WriteUInt64(stream, instance.timestamp);
                stream.WriteByte((byte)16);
                ProtocolParser.WriteZInt32(stream, instance.time_zone);
                if (instance.server_time_offset.HasValue)
                {
                    stream.WriteByte((byte)24);
                    ProtocolParser.WriteUInt64(stream, (ulong)instance.server_time_offset.Value);
                }
                ProtocolParser.Stack.Push(stream1);
            }

            public static byte[] SerializeToBytes(ReportMessage.Time instance)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ReportMessage.Time.Serialize((Stream)memoryStream, instance);
                    return memoryStream.ToArray();
                }
            }

            public static void SerializeLengthDelimited(Stream stream, ReportMessage.Time instance)
            {
                byte[] bytes = ReportMessage.Time.SerializeToBytes(instance);
                ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        [DataContract]
        public class Location
        {
            [DataMember]
            public double lat { get; set; }

            [DataMember]
            public double lon { get; set; }

            [DataMember]
            public ulong? timestamp { get; set; }

            [DataMember]
            public uint? precision { get; set; }

            [DataMember]
            public uint? direction { get; set; }

            [DataMember]
            public uint? speed { get; set; }

            [DataMember]
            public int? altitude { get; set; }

            [DataMember]
            public ReportMessage.Location.Provider? provider { get; set; }

            [DataMember]
            public ReportMessage.OptionalBool? enabled { get; set; }

            public static ReportMessage.Location Deserialize(Stream stream)
            {
                ReportMessage.Location instance = new ReportMessage.Location();
                ReportMessage.Location.Deserialize(stream, instance);
                return instance;
            }

            public static ReportMessage.Location DeserializeLengthDelimited(Stream stream)
            {
                ReportMessage.Location instance = new ReportMessage.Location();
                ReportMessage.Location.DeserializeLengthDelimited(stream, instance);
                return instance;
            }

            public static ReportMessage.Location DeserializeLength(Stream stream, int length)
            {
                ReportMessage.Location instance = new ReportMessage.Location();
                ReportMessage.Location.DeserializeLength(stream, length, instance);
                return instance;
            }

            public static ReportMessage.Location Deserialize(byte[] buffer)
            {
                ReportMessage.Location instance = new ReportMessage.Location();
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    ReportMessage.Location.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            public static ReportMessage.Location Deserialize(
              byte[] buffer,
              ReportMessage.Location instance)
            {
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    ReportMessage.Location.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            public static ReportMessage.Location Deserialize(
              Stream stream,
              ReportMessage.Location instance)
            {
                BinaryReader binaryReader = new BinaryReader(stream);
                instance.provider = new ReportMessage.Location.Provider?(ReportMessage.Location.Provider.PROVIDER_UNKNOWN);
                instance.enabled = new ReportMessage.OptionalBool?(ReportMessage.OptionalBool.OPTIONAL_BOOL_UNDEFINED);
                while (true)
                {
                    int num = stream.ReadByte();
                    switch (num)
                    {
                        case -1:
                            goto label_14;
                        case 9:
                            instance.lat = binaryReader.ReadDouble();
                            continue;
                        case 17:
                            instance.lon = binaryReader.ReadDouble();
                            continue;
                        case 24:
                            instance.timestamp = new ulong?(ProtocolParser.ReadUInt64(stream));
                            continue;
                        case 32:
                            instance.precision = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 40:
                            instance.direction = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 48:
                            instance.speed = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 56:
                            instance.altitude = new int?((int)ProtocolParser.ReadUInt64(stream));
                            continue;
                        case 64:
                            instance.provider = new ReportMessage.Location.Provider?((ReportMessage.Location.Provider)ProtocolParser.ReadUInt64(stream));
                            continue;
                        case 72:
                            instance.enabled = new ReportMessage.OptionalBool?((ReportMessage.OptionalBool)ProtocolParser.ReadUInt64(stream));
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num, stream);
                            if (key.Field != 0U)
                            {
                                ProtocolParser.SkipKey(stream, key);
                                continue;
                            }
                            goto label_12;
                    }
                }
                label_12:
                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                label_14:
                return instance;
            }

            public static ReportMessage.Location DeserializeLengthDelimited(
              Stream stream,
              ReportMessage.Location instance)
            {
                BinaryReader binaryReader = new BinaryReader(stream);
                instance.provider = new ReportMessage.Location.Provider?(ReportMessage.Location.Provider.PROVIDER_UNKNOWN);
                instance.enabled = new ReportMessage.OptionalBool?(ReportMessage.OptionalBool.OPTIONAL_BOOL_UNDEFINED);
                long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                while (stream.Position < num1)
                {
                    int num2 = stream.ReadByte();
                    switch (num2)
                    {
                        case -1:
                            throw new EndOfStreamException();
                        case 9:
                            instance.lat = binaryReader.ReadDouble();
                            continue;
                        case 17:
                            instance.lon = binaryReader.ReadDouble();
                            continue;
                        case 24:
                            instance.timestamp = new ulong?(ProtocolParser.ReadUInt64(stream));
                            continue;
                        case 32:
                            instance.precision = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 40:
                            instance.direction = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 48:
                            instance.speed = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 56:
                            instance.altitude = new int?((int)ProtocolParser.ReadUInt64(stream));
                            continue;
                        case 64:
                            instance.provider = new ReportMessage.Location.Provider?((ReportMessage.Location.Provider)ProtocolParser.ReadUInt64(stream));
                            continue;
                        case 72:
                            instance.enabled = new ReportMessage.OptionalBool?((ReportMessage.OptionalBool)ProtocolParser.ReadUInt64(stream));
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num2, stream);
                            if (key.Field == 0U)
                                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            ProtocolParser.SkipKey(stream, key);
                            continue;
                    }
                }
                if (stream.Position != num1)
                    throw new ProtocolBufferException("Read past max limit");
                return instance;
            }

            public static ReportMessage.Location DeserializeLength(
              Stream stream,
              int length,
              ReportMessage.Location instance)
            {
                BinaryReader binaryReader = new BinaryReader(stream);
                instance.provider = new ReportMessage.Location.Provider?(ReportMessage.Location.Provider.PROVIDER_UNKNOWN);
                instance.enabled = new ReportMessage.OptionalBool?(ReportMessage.OptionalBool.OPTIONAL_BOOL_UNDEFINED);
                long num1 = stream.Position + (long)length;
                while (stream.Position < num1)
                {
                    int num2 = stream.ReadByte();
                    switch (num2)
                    {
                        case -1:
                            throw new EndOfStreamException();
                        case 9:
                            instance.lat = binaryReader.ReadDouble();
                            continue;
                        case 17:
                            instance.lon = binaryReader.ReadDouble();
                            continue;
                        case 24:
                            instance.timestamp = new ulong?(ProtocolParser.ReadUInt64(stream));
                            continue;
                        case 32:
                            instance.precision = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 40:
                            instance.direction = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 48:
                            instance.speed = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 56:
                            instance.altitude = new int?((int)ProtocolParser.ReadUInt64(stream));
                            continue;
                        case 64:
                            instance.provider = new ReportMessage.Location.Provider?((ReportMessage.Location.Provider)ProtocolParser.ReadUInt64(stream));
                            continue;
                        case 72:
                            instance.enabled = new ReportMessage.OptionalBool?((ReportMessage.OptionalBool)ProtocolParser.ReadUInt64(stream));
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num2, stream);
                            if (key.Field == 0U)
                                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            ProtocolParser.SkipKey(stream, key);
                            continue;
                    }
                }
                if (stream.Position != num1)
                    throw new ProtocolBufferException("Read past max limit");
                return instance;
            }

            public static void Serialize(Stream stream, ReportMessage.Location instance)
            {
                BinaryWriter binaryWriter = new BinaryWriter(stream);
                MemoryStream stream1 = ProtocolParser.Stack.Pop();
                stream.WriteByte((byte)9);
                binaryWriter.Write(instance.lat);
                stream.WriteByte((byte)17);
                binaryWriter.Write(instance.lon);
                if (instance.timestamp.HasValue)
                {
                    stream.WriteByte((byte)24);
                    ProtocolParser.WriteUInt64(stream, instance.timestamp.Value);
                }
                uint? nullable;
                if (instance.precision.HasValue)
                {
                    stream.WriteByte((byte)32);
                    Stream stream2 = stream;
                    nullable = instance.precision;
                    int num = (int)nullable.Value;
                    ProtocolParser.WriteUInt32(stream2, (uint)num);
                }
                nullable = instance.direction;
                if (nullable.HasValue)
                {
                    stream.WriteByte((byte)40);
                    Stream stream2 = stream;
                    nullable = instance.direction;
                    int num = (int)nullable.Value;
                    ProtocolParser.WriteUInt32(stream2, (uint)num);
                }
                nullable = instance.speed;
                if (nullable.HasValue)
                {
                    stream.WriteByte((byte)48);
                    Stream stream2 = stream;
                    nullable = instance.speed;
                    int num = (int)nullable.Value;
                    ProtocolParser.WriteUInt32(stream2, (uint)num);
                }
                if (instance.altitude.HasValue)
                {
                    stream.WriteByte((byte)56);
                    ProtocolParser.WriteUInt64(stream, (ulong)instance.altitude.Value);
                }
                if (instance.provider.HasValue)
                {
                    stream.WriteByte((byte)64);
                    ProtocolParser.WriteUInt64(stream, (ulong)instance.provider.Value);
                }
                if (instance.enabled.HasValue)
                {
                    stream.WriteByte((byte)72);
                    ProtocolParser.WriteUInt64(stream, (ulong)instance.enabled.Value);
                }
                ProtocolParser.Stack.Push(stream1);
            }

            public static byte[] SerializeToBytes(ReportMessage.Location instance)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ReportMessage.Location.Serialize((Stream)memoryStream, instance);
                    return memoryStream.ToArray();
                }
            }

            public static void SerializeLengthDelimited(Stream stream, ReportMessage.Location instance)
            {
                byte[] bytes = ReportMessage.Location.SerializeToBytes(instance);
                ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                stream.Write(bytes, 0, bytes.Length);
            }

            public enum Provider
            {
                PROVIDER_UNKNOWN,
                PROVIDER_GPS,
                PROVIDER_NETWORK,
            }
        }

        [DataContract]
        public class Session
        {
            [DataMember]
            public ulong id { get; set; }

            [DataMember]
            public ReportMessage.Session.SessionDesc session_desc { get; set; }

            [DataMember]
            public List<ReportMessage.Session.Event> events { get; set; }

            public static ReportMessage.Session Deserialize(Stream stream)
            {
                ReportMessage.Session instance = new ReportMessage.Session();
                ReportMessage.Session.Deserialize(stream, instance);
                return instance;
            }

            public static ReportMessage.Session DeserializeLengthDelimited(Stream stream)
            {
                ReportMessage.Session instance = new ReportMessage.Session();
                ReportMessage.Session.DeserializeLengthDelimited(stream, instance);
                return instance;
            }

            public static ReportMessage.Session DeserializeLength(Stream stream, int length)
            {
                ReportMessage.Session instance = new ReportMessage.Session();
                ReportMessage.Session.DeserializeLength(stream, length, instance);
                return instance;
            }

            public static ReportMessage.Session Deserialize(byte[] buffer)
            {
                ReportMessage.Session instance = new ReportMessage.Session();
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    ReportMessage.Session.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            public static ReportMessage.Session Deserialize(
              byte[] buffer,
              ReportMessage.Session instance)
            {
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    ReportMessage.Session.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            public static ReportMessage.Session Deserialize(
              Stream stream,
              ReportMessage.Session instance)
            {
                if (instance.events == null)
                    instance.events = new List<ReportMessage.Session.Event>();
                while (true)
                {
                    int num = stream.ReadByte();
                    switch (num)
                    {
                        case -1:
                            goto label_11;
                        case 8:
                            instance.id = ProtocolParser.ReadUInt64(stream);
                            continue;
                        case 18:
                            if (instance.session_desc == null)
                            {
                                instance.session_desc = ReportMessage.Session.SessionDesc.DeserializeLengthDelimited(stream);
                                continue;
                            }
                            ReportMessage.Session.SessionDesc.DeserializeLengthDelimited(stream, instance.session_desc);
                            continue;
                        case 26:
                            instance.events.Add(ReportMessage.Session.Event.DeserializeLengthDelimited(stream));
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num, stream);
                            if (key.Field != 0U)
                            {
                                ProtocolParser.SkipKey(stream, key);
                                continue;
                            }
                            goto label_9;
                    }
                }
                label_9:
                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                label_11:
                return instance;
            }

            public static ReportMessage.Session DeserializeLengthDelimited(
              Stream stream,
              ReportMessage.Session instance)
            {
                if (instance.events == null)
                    instance.events = new List<ReportMessage.Session.Event>();
                long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                while (stream.Position < num1)
                {
                    int num2 = stream.ReadByte();
                    switch (num2)
                    {
                        case -1:
                            throw new EndOfStreamException();
                        case 8:
                            instance.id = ProtocolParser.ReadUInt64(stream);
                            continue;
                        case 18:
                            if (instance.session_desc == null)
                            {
                                instance.session_desc = ReportMessage.Session.SessionDesc.DeserializeLengthDelimited(stream);
                                continue;
                            }
                            ReportMessage.Session.SessionDesc.DeserializeLengthDelimited(stream, instance.session_desc);
                            continue;
                        case 26:
                            instance.events.Add(ReportMessage.Session.Event.DeserializeLengthDelimited(stream));
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num2, stream);
                            if (key.Field == 0U)
                                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            ProtocolParser.SkipKey(stream, key);
                            continue;
                    }
                }
                if (stream.Position != num1)
                    throw new ProtocolBufferException("Read past max limit");
                return instance;
            }

            public static ReportMessage.Session DeserializeLength(
              Stream stream,
              int length,
              ReportMessage.Session instance)
            {
                if (instance.events == null)
                    instance.events = new List<ReportMessage.Session.Event>();
                long num1 = stream.Position + (long)length;
                while (stream.Position < num1)
                {
                    int num2 = stream.ReadByte();
                    switch (num2)
                    {
                        case -1:
                            throw new EndOfStreamException();
                        case 8:
                            instance.id = ProtocolParser.ReadUInt64(stream);
                            continue;
                        case 18:
                            if (instance.session_desc == null)
                            {
                                instance.session_desc = ReportMessage.Session.SessionDesc.DeserializeLengthDelimited(stream);
                                continue;
                            }
                            ReportMessage.Session.SessionDesc.DeserializeLengthDelimited(stream, instance.session_desc);
                            continue;
                        case 26:
                            instance.events.Add(ReportMessage.Session.Event.DeserializeLengthDelimited(stream));
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num2, stream);
                            if (key.Field == 0U)
                                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            ProtocolParser.SkipKey(stream, key);
                            continue;
                    }
                }
                if (stream.Position != num1)
                    throw new ProtocolBufferException("Read past max limit");
                return instance;
            }

            public static void Serialize(Stream stream, ReportMessage.Session instance)
            {
                MemoryStream stream1 = ProtocolParser.Stack.Pop();
                stream.WriteByte((byte)8);
                ProtocolParser.WriteUInt64(stream, instance.id);
                if (instance.session_desc == null)
                    throw new ProtocolBufferException("session_desc is required by the proto specification.");
                stream.WriteByte((byte)18);
                stream1.SetLength(0L);
                ReportMessage.Session.SessionDesc.Serialize((Stream)stream1, instance.session_desc);
                uint length1 = (uint)stream1.Length;
                ProtocolParser.WriteUInt32(stream, length1);
                stream1.WriteTo(stream);
                if (instance.events != null)
                {
                    foreach (ReportMessage.Session.Event instance1 in instance.events)
                    {
                        stream.WriteByte((byte)26);
                        stream1.SetLength(0L);
                        ReportMessage.Session.Event.Serialize((Stream)stream1, instance1);
                        uint length2 = (uint)stream1.Length;
                        ProtocolParser.WriteUInt32(stream, length2);
                        stream1.WriteTo(stream);
                    }
                }
                ProtocolParser.Stack.Push(stream1);
            }

            public static byte[] SerializeToBytes(ReportMessage.Session instance)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ReportMessage.Session.Serialize((Stream)memoryStream, instance);
                    return memoryStream.ToArray();
                }
            }

            public static void SerializeLengthDelimited(Stream stream, ReportMessage.Session instance)
            {
                byte[] bytes = ReportMessage.Session.SerializeToBytes(instance);
                ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                stream.Write(bytes, 0, bytes.Length);
            }

            public enum ConnectionType
            {
                CONNECTION_CELL,
                CONNECTION_WIFI,
                CONNECTION_UNDEFINED,
            }

            [DataContract]
            public class WifiNetworkInfo
            {
                [DataMember]
                public string mac { get; set; }

                [DataMember]
                public int? signal_strength { get; set; }

                [DataMember]
                public string ssid { get; set; }

                [DataMember]
                public bool? is_connected { get; set; }

                public static ReportMessage.Session.WifiNetworkInfo Deserialize(Stream stream)
                {
                    ReportMessage.Session.WifiNetworkInfo instance = new ReportMessage.Session.WifiNetworkInfo();
                    ReportMessage.Session.WifiNetworkInfo.Deserialize(stream, instance);
                    return instance;
                }

                public static ReportMessage.Session.WifiNetworkInfo DeserializeLengthDelimited(
                  Stream stream)
                {
                    ReportMessage.Session.WifiNetworkInfo instance = new ReportMessage.Session.WifiNetworkInfo();
                    ReportMessage.Session.WifiNetworkInfo.DeserializeLengthDelimited(stream, instance);
                    return instance;
                }

                public static ReportMessage.Session.WifiNetworkInfo DeserializeLength(
                  Stream stream,
                  int length)
                {
                    ReportMessage.Session.WifiNetworkInfo instance = new ReportMessage.Session.WifiNetworkInfo();
                    ReportMessage.Session.WifiNetworkInfo.DeserializeLength(stream, length, instance);
                    return instance;
                }

                public static ReportMessage.Session.WifiNetworkInfo Deserialize(byte[] buffer)
                {
                    ReportMessage.Session.WifiNetworkInfo instance = new ReportMessage.Session.WifiNetworkInfo();
                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                        ReportMessage.Session.WifiNetworkInfo.Deserialize((Stream)memoryStream, instance);
                    return instance;
                }

                public static ReportMessage.Session.WifiNetworkInfo Deserialize(
                  byte[] buffer,
                  ReportMessage.Session.WifiNetworkInfo instance)
                {
                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                        ReportMessage.Session.WifiNetworkInfo.Deserialize((Stream)memoryStream, instance);
                    return instance;
                }

                public static ReportMessage.Session.WifiNetworkInfo Deserialize(
                  Stream stream,
                  ReportMessage.Session.WifiNetworkInfo instance)
                {
                    instance.is_connected = new bool?(false);
                    while (true)
                    {
                        int num = stream.ReadByte();
                        switch (num)
                        {
                            case -1:
                                goto label_9;
                            case 10:
                                instance.mac = ProtocolParser.ReadString(stream);
                                continue;
                            case 16:
                                instance.signal_strength = new int?(ProtocolParser.ReadZInt32(stream));
                                continue;
                            case 26:
                                instance.ssid = ProtocolParser.ReadString(stream);
                                continue;
                            case 32:
                                instance.is_connected = new bool?(ProtocolParser.ReadBool(stream));
                                continue;
                            default:
                                Key key = ProtocolParser.ReadKey((byte)num, stream);
                                if (key.Field != 0U)
                                {
                                    ProtocolParser.SkipKey(stream, key);
                                    continue;
                                }
                                goto label_7;
                        }
                    }
                    label_7:
                    throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                    label_9:
                    return instance;
                }

                public static ReportMessage.Session.WifiNetworkInfo DeserializeLengthDelimited(
                  Stream stream,
                  ReportMessage.Session.WifiNetworkInfo instance)
                {
                    instance.is_connected = new bool?(false);
                    long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                    while (stream.Position < num1)
                    {
                        int num2 = stream.ReadByte();
                        switch (num2)
                        {
                            case -1:
                                throw new EndOfStreamException();
                            case 10:
                                instance.mac = ProtocolParser.ReadString(stream);
                                continue;
                            case 16:
                                instance.signal_strength = new int?(ProtocolParser.ReadZInt32(stream));
                                continue;
                            case 26:
                                instance.ssid = ProtocolParser.ReadString(stream);
                                continue;
                            case 32:
                                instance.is_connected = new bool?(ProtocolParser.ReadBool(stream));
                                continue;
                            default:
                                Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                if (key.Field == 0U)
                                    throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                ProtocolParser.SkipKey(stream, key);
                                continue;
                        }
                    }
                    if (stream.Position != num1)
                        throw new ProtocolBufferException("Read past max limit");
                    return instance;
                }

                public static ReportMessage.Session.WifiNetworkInfo DeserializeLength(
                  Stream stream,
                  int length,
                  ReportMessage.Session.WifiNetworkInfo instance)
                {
                    instance.is_connected = new bool?(false);
                    long num1 = stream.Position + (long)length;
                    while (stream.Position < num1)
                    {
                        int num2 = stream.ReadByte();
                        switch (num2)
                        {
                            case -1:
                                throw new EndOfStreamException();
                            case 10:
                                instance.mac = ProtocolParser.ReadString(stream);
                                continue;
                            case 16:
                                instance.signal_strength = new int?(ProtocolParser.ReadZInt32(stream));
                                continue;
                            case 26:
                                instance.ssid = ProtocolParser.ReadString(stream);
                                continue;
                            case 32:
                                instance.is_connected = new bool?(ProtocolParser.ReadBool(stream));
                                continue;
                            default:
                                Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                if (key.Field == 0U)
                                    throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                ProtocolParser.SkipKey(stream, key);
                                continue;
                        }
                    }
                    if (stream.Position != num1)
                        throw new ProtocolBufferException("Read past max limit");
                    return instance;
                }

                public static void Serialize(Stream stream, ReportMessage.Session.WifiNetworkInfo instance)
                {
                    MemoryStream stream1 = ProtocolParser.Stack.Pop();
                    if (instance.mac == null)
                        throw new ProtocolBufferException("mac is required by the proto specification.");
                    stream.WriteByte((byte)10);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.mac));
                    int? signalStrength = instance.signal_strength;
                    if (signalStrength.HasValue)
                    {
                        stream.WriteByte((byte)16);
                        Stream stream2 = stream;
                        signalStrength = instance.signal_strength;
                        int val = signalStrength.Value;
                        ProtocolParser.WriteZInt32(stream2, val);
                    }
                    if (instance.ssid != null)
                    {
                        stream.WriteByte((byte)26);
                        ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.ssid));
                    }
                    if (instance.is_connected.HasValue)
                    {
                        stream.WriteByte((byte)32);
                        ProtocolParser.WriteBool(stream, instance.is_connected.Value);
                    }
                    ProtocolParser.Stack.Push(stream1);
                }

                public static byte[] SerializeToBytes(ReportMessage.Session.WifiNetworkInfo instance)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ReportMessage.Session.WifiNetworkInfo.Serialize((Stream)memoryStream, instance);
                        return memoryStream.ToArray();
                    }
                }

                public static void SerializeLengthDelimited(
                  Stream stream,
                  ReportMessage.Session.WifiNetworkInfo instance)
                {
                    byte[] bytes = ReportMessage.Session.WifiNetworkInfo.SerializeToBytes(instance);
                    ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                    stream.Write(bytes, 0, bytes.Length);
                }
            }

            [DataContract]
            public class SessionDesc
            {
                [DataMember]
                public ReportMessage.Time start_time { get; set; }

                [DataMember]
                public string locale { get; set; }

                [DataMember]
                public ReportMessage.Session.SessionDesc.Location location { get; set; }

                [DataMember]
                public ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo network_info { get; set; }

                [DataMember]
                public ReportMessage.Session.SessionDesc.SessionType? session_type { get; set; }

                public static ReportMessage.Session.SessionDesc Deserialize(Stream stream)
                {
                    ReportMessage.Session.SessionDesc instance = new ReportMessage.Session.SessionDesc();
                    ReportMessage.Session.SessionDesc.Deserialize(stream, instance);
                    return instance;
                }

                public static ReportMessage.Session.SessionDesc DeserializeLengthDelimited(
                  Stream stream)
                {
                    ReportMessage.Session.SessionDesc instance = new ReportMessage.Session.SessionDesc();
                    ReportMessage.Session.SessionDesc.DeserializeLengthDelimited(stream, instance);
                    return instance;
                }

                public static ReportMessage.Session.SessionDesc DeserializeLength(
                  Stream stream,
                  int length)
                {
                    ReportMessage.Session.SessionDesc instance = new ReportMessage.Session.SessionDesc();
                    ReportMessage.Session.SessionDesc.DeserializeLength(stream, length, instance);
                    return instance;
                }

                public static ReportMessage.Session.SessionDesc Deserialize(byte[] buffer)
                {
                    ReportMessage.Session.SessionDesc instance = new ReportMessage.Session.SessionDesc();
                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                        ReportMessage.Session.SessionDesc.Deserialize((Stream)memoryStream, instance);
                    return instance;
                }

                public static ReportMessage.Session.SessionDesc Deserialize(
                  byte[] buffer,
                  ReportMessage.Session.SessionDesc instance)
                {
                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                        ReportMessage.Session.SessionDesc.Deserialize((Stream)memoryStream, instance);
                    return instance;
                }

                public static ReportMessage.Session.SessionDesc Deserialize(
                  Stream stream,
                  ReportMessage.Session.SessionDesc instance)
                {
                    while (true)
                    {
                        int num = stream.ReadByte();
                        switch (num)
                        {
                            case -1:
                                goto label_15;
                            case 10:
                                if (instance.start_time == null)
                                {
                                    instance.start_time = ReportMessage.Time.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Time.DeserializeLengthDelimited(stream, instance.start_time);
                                continue;
                            case 18:
                                instance.locale = ProtocolParser.ReadString(stream);
                                continue;
                            case 26:
                                if (instance.location == null)
                                {
                                    instance.location = ReportMessage.Session.SessionDesc.Location.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Session.SessionDesc.Location.DeserializeLengthDelimited(stream, instance.location);
                                continue;
                            case 34:
                                if (instance.network_info == null)
                                {
                                    instance.network_info = ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.DeserializeLengthDelimited(stream, instance.network_info);
                                continue;
                            case 40:
                                instance.session_type = new ReportMessage.Session.SessionDesc.SessionType?((ReportMessage.Session.SessionDesc.SessionType)ProtocolParser.ReadUInt64(stream));
                                continue;
                            default:
                                Key key = ProtocolParser.ReadKey((byte)num, stream);
                                if (key.Field != 0U)
                                {
                                    ProtocolParser.SkipKey(stream, key);
                                    continue;
                                }
                                goto label_13;
                        }
                    }
                    label_13:
                    throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                    label_15:
                    return instance;
                }

                public static ReportMessage.Session.SessionDesc DeserializeLengthDelimited(
                  Stream stream,
                  ReportMessage.Session.SessionDesc instance)
                {
                    long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                    while (stream.Position < num1)
                    {
                        int num2 = stream.ReadByte();
                        switch (num2)
                        {
                            case -1:
                                throw new EndOfStreamException();
                            case 10:
                                if (instance.start_time == null)
                                {
                                    instance.start_time = ReportMessage.Time.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Time.DeserializeLengthDelimited(stream, instance.start_time);
                                continue;
                            case 18:
                                instance.locale = ProtocolParser.ReadString(stream);
                                continue;
                            case 26:
                                if (instance.location == null)
                                {
                                    instance.location = ReportMessage.Session.SessionDesc.Location.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Session.SessionDesc.Location.DeserializeLengthDelimited(stream, instance.location);
                                continue;
                            case 34:
                                if (instance.network_info == null)
                                {
                                    instance.network_info = ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.DeserializeLengthDelimited(stream, instance.network_info);
                                continue;
                            case 40:
                                instance.session_type = new ReportMessage.Session.SessionDesc.SessionType?((ReportMessage.Session.SessionDesc.SessionType)ProtocolParser.ReadUInt64(stream));
                                continue;
                            default:
                                Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                if (key.Field == 0U)
                                    throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                ProtocolParser.SkipKey(stream, key);
                                continue;
                        }
                    }
                    if (stream.Position != num1)
                        throw new ProtocolBufferException("Read past max limit");
                    return instance;
                }

                public static ReportMessage.Session.SessionDesc DeserializeLength(
                  Stream stream,
                  int length,
                  ReportMessage.Session.SessionDesc instance)
                {
                    long num1 = stream.Position + (long)length;
                    while (stream.Position < num1)
                    {
                        int num2 = stream.ReadByte();
                        switch (num2)
                        {
                            case -1:
                                throw new EndOfStreamException();
                            case 10:
                                if (instance.start_time == null)
                                {
                                    instance.start_time = ReportMessage.Time.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Time.DeserializeLengthDelimited(stream, instance.start_time);
                                continue;
                            case 18:
                                instance.locale = ProtocolParser.ReadString(stream);
                                continue;
                            case 26:
                                if (instance.location == null)
                                {
                                    instance.location = ReportMessage.Session.SessionDesc.Location.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Session.SessionDesc.Location.DeserializeLengthDelimited(stream, instance.location);
                                continue;
                            case 34:
                                if (instance.network_info == null)
                                {
                                    instance.network_info = ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.DeserializeLengthDelimited(stream, instance.network_info);
                                continue;
                            case 40:
                                instance.session_type = new ReportMessage.Session.SessionDesc.SessionType?((ReportMessage.Session.SessionDesc.SessionType)ProtocolParser.ReadUInt64(stream));
                                continue;
                            default:
                                Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                if (key.Field == 0U)
                                    throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                ProtocolParser.SkipKey(stream, key);
                                continue;
                        }
                    }
                    if (stream.Position != num1)
                        throw new ProtocolBufferException("Read past max limit");
                    return instance;
                }

                public static void Serialize(Stream stream, ReportMessage.Session.SessionDesc instance)
                {
                    MemoryStream stream1 = ProtocolParser.Stack.Pop();
                    if (instance.start_time == null)
                        throw new ProtocolBufferException("start_time is required by the proto specification.");
                    stream.WriteByte((byte)10);
                    stream1.SetLength(0L);
                    ReportMessage.Time.Serialize((Stream)stream1, instance.start_time);
                    uint length1 = (uint)stream1.Length;
                    ProtocolParser.WriteUInt32(stream, length1);
                    stream1.WriteTo(stream);
                    if (instance.locale == null)
                        throw new ProtocolBufferException("locale is required by the proto specification.");
                    stream.WriteByte((byte)18);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.locale));
                    if (instance.location != null)
                    {
                        stream.WriteByte((byte)26);
                        stream1.SetLength(0L);
                        ReportMessage.Session.SessionDesc.Location.Serialize((Stream)stream1, instance.location);
                        uint length2 = (uint)stream1.Length;
                        ProtocolParser.WriteUInt32(stream, length2);
                        stream1.WriteTo(stream);
                    }
                    if (instance.network_info != null)
                    {
                        stream.WriteByte((byte)34);
                        stream1.SetLength(0L);
                        ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.Serialize((Stream)stream1, instance.network_info);
                        uint length2 = (uint)stream1.Length;
                        ProtocolParser.WriteUInt32(stream, length2);
                        stream1.WriteTo(stream);
                    }
                    if (instance.session_type.HasValue)
                    {
                        stream.WriteByte((byte)40);
                        ProtocolParser.WriteUInt64(stream, (ulong)instance.session_type.Value);
                    }
                    ProtocolParser.Stack.Push(stream1);
                }

                public static byte[] SerializeToBytes(ReportMessage.Session.SessionDesc instance)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ReportMessage.Session.SessionDesc.Serialize((Stream)memoryStream, instance);
                        return memoryStream.ToArray();
                    }
                }

                public static void SerializeLengthDelimited(
                  Stream stream,
                  ReportMessage.Session.SessionDesc instance)
                {
                    byte[] bytes = ReportMessage.Session.SessionDesc.SerializeToBytes(instance);
                    ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                    stream.Write(bytes, 0, bytes.Length);
                }

                public enum SessionType
                {
                    SESSION_FOREGROUND,
                    SESSION_BACKGROUND,
                }

                [DataContract]
                public class Location
                {
                    [DataMember]
                    public double lat { get; set; }

                    [DataMember]
                    public double lon { get; set; }

                    public static ReportMessage.Session.SessionDesc.Location Deserialize(
                      Stream stream)
                    {
                        ReportMessage.Session.SessionDesc.Location instance = new ReportMessage.Session.SessionDesc.Location();
                        ReportMessage.Session.SessionDesc.Location.Deserialize(stream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.SessionDesc.Location DeserializeLengthDelimited(
                      Stream stream)
                    {
                        ReportMessage.Session.SessionDesc.Location instance = new ReportMessage.Session.SessionDesc.Location();
                        ReportMessage.Session.SessionDesc.Location.DeserializeLengthDelimited(stream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.SessionDesc.Location DeserializeLength(
                      Stream stream,
                      int length)
                    {
                        ReportMessage.Session.SessionDesc.Location instance = new ReportMessage.Session.SessionDesc.Location();
                        ReportMessage.Session.SessionDesc.Location.DeserializeLength(stream, length, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.SessionDesc.Location Deserialize(
                      byte[] buffer)
                    {
                        ReportMessage.Session.SessionDesc.Location instance = new ReportMessage.Session.SessionDesc.Location();
                        using (MemoryStream memoryStream = new MemoryStream(buffer))
                            ReportMessage.Session.SessionDesc.Location.Deserialize((Stream)memoryStream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.SessionDesc.Location Deserialize(
                      byte[] buffer,
                      ReportMessage.Session.SessionDesc.Location instance)
                    {
                        using (MemoryStream memoryStream = new MemoryStream(buffer))
                            ReportMessage.Session.SessionDesc.Location.Deserialize((Stream)memoryStream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.SessionDesc.Location Deserialize(
                      Stream stream,
                      ReportMessage.Session.SessionDesc.Location instance)
                    {
                        BinaryReader binaryReader = new BinaryReader(stream);
                        while (true)
                        {
                            int num = stream.ReadByte();
                            switch (num)
                            {
                                case -1:
                                    goto label_7;
                                case 9:
                                    instance.lat = binaryReader.ReadDouble();
                                    continue;
                                case 17:
                                    instance.lon = binaryReader.ReadDouble();
                                    continue;
                                default:
                                    Key key = ProtocolParser.ReadKey((byte)num, stream);
                                    if (key.Field != 0U)
                                    {
                                        ProtocolParser.SkipKey(stream, key);
                                        continue;
                                    }
                                    goto label_5;
                            }
                        }
                        label_5:
                        throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                        label_7:
                        return instance;
                    }

                    public static ReportMessage.Session.SessionDesc.Location DeserializeLengthDelimited(
                      Stream stream,
                      ReportMessage.Session.SessionDesc.Location instance)
                    {
                        BinaryReader binaryReader = new BinaryReader(stream);
                        long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                        while (stream.Position < num1)
                        {
                            int num2 = stream.ReadByte();
                            switch (num2)
                            {
                                case -1:
                                    throw new EndOfStreamException();
                                case 9:
                                    instance.lat = binaryReader.ReadDouble();
                                    continue;
                                case 17:
                                    instance.lon = binaryReader.ReadDouble();
                                    continue;
                                default:
                                    Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                    if (key.Field == 0U)
                                        throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                    ProtocolParser.SkipKey(stream, key);
                                    continue;
                            }
                        }
                        if (stream.Position != num1)
                            throw new ProtocolBufferException("Read past max limit");
                        return instance;
                    }

                    public static ReportMessage.Session.SessionDesc.Location DeserializeLength(
                      Stream stream,
                      int length,
                      ReportMessage.Session.SessionDesc.Location instance)
                    {
                        BinaryReader binaryReader = new BinaryReader(stream);
                        long num1 = stream.Position + (long)length;
                        while (stream.Position < num1)
                        {
                            int num2 = stream.ReadByte();
                            switch (num2)
                            {
                                case -1:
                                    throw new EndOfStreamException();
                                case 9:
                                    instance.lat = binaryReader.ReadDouble();
                                    continue;
                                case 17:
                                    instance.lon = binaryReader.ReadDouble();
                                    continue;
                                default:
                                    Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                    if (key.Field == 0U)
                                        throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                    ProtocolParser.SkipKey(stream, key);
                                    continue;
                            }
                        }
                        if (stream.Position != num1)
                            throw new ProtocolBufferException("Read past max limit");
                        return instance;
                    }

                    public static void Serialize(
                      Stream stream,
                      ReportMessage.Session.SessionDesc.Location instance)
                    {
                        BinaryWriter binaryWriter = new BinaryWriter(stream);
                        MemoryStream stream1 = ProtocolParser.Stack.Pop();
                        stream.WriteByte((byte)9);
                        binaryWriter.Write(instance.lat);
                        stream.WriteByte((byte)17);
                        binaryWriter.Write(instance.lon);
                        ProtocolParser.Stack.Push(stream1);
                    }

                    public static byte[] SerializeToBytes(
                      ReportMessage.Session.SessionDesc.Location instance)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            ReportMessage.Session.SessionDesc.Location.Serialize((Stream)memoryStream, instance);
                            return memoryStream.ToArray();
                        }
                    }

                    public static void SerializeLengthDelimited(
                      Stream stream,
                      ReportMessage.Session.SessionDesc.Location instance)
                    {
                        byte[] bytes = ReportMessage.Session.SessionDesc.Location.SerializeToBytes(instance);
                        ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }

                [DataContract]
                public class DeprecatedNetworkInfo
                {
                    [DataMember]
                    public ReportMessage.Session.ConnectionType connection_type { get; set; }

                    [DataMember]
                    public ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo cellular_network_info { get; set; }

                    [DataMember]
                    public List<ReportMessage.Session.WifiNetworkInfo> wifi_networks { get; set; }

                    public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo Deserialize(
                      Stream stream)
                    {
                        ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo instance = new ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo();
                        ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.Deserialize(stream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo DeserializeLengthDelimited(
                      Stream stream)
                    {
                        ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo instance = new ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo();
                        ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.DeserializeLengthDelimited(stream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo DeserializeLength(
                      Stream stream,
                      int length)
                    {
                        ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo instance = new ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo();
                        ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.DeserializeLength(stream, length, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo Deserialize(
                      byte[] buffer)
                    {
                        ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo instance = new ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo();
                        using (MemoryStream memoryStream = new MemoryStream(buffer))
                            ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.Deserialize((Stream)memoryStream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo Deserialize(
                      byte[] buffer,
                      ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo instance)
                    {
                        using (MemoryStream memoryStream = new MemoryStream(buffer))
                            ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.Deserialize((Stream)memoryStream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo Deserialize(
                      Stream stream,
                      ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo instance)
                    {
                        if (instance.wifi_networks == null)
                            instance.wifi_networks = new List<ReportMessage.Session.WifiNetworkInfo>();
                        while (true)
                        {
                            int num = stream.ReadByte();
                            switch (num)
                            {
                                case -1:
                                    goto label_11;
                                case 8:
                                    instance.connection_type = (ReportMessage.Session.ConnectionType)ProtocolParser.ReadUInt64(stream);
                                    continue;
                                case 18:
                                    if (instance.cellular_network_info == null)
                                    {
                                        instance.cellular_network_info = ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo.DeserializeLengthDelimited(stream);
                                        continue;
                                    }
                                    ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo.DeserializeLengthDelimited(stream, instance.cellular_network_info);
                                    continue;
                                case 26:
                                    instance.wifi_networks.Add(ReportMessage.Session.WifiNetworkInfo.DeserializeLengthDelimited(stream));
                                    continue;
                                default:
                                    Key key = ProtocolParser.ReadKey((byte)num, stream);
                                    if (key.Field != 0U)
                                    {
                                        ProtocolParser.SkipKey(stream, key);
                                        continue;
                                    }
                                    goto label_9;
                            }
                        }
                        label_9:
                        throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                        label_11:
                        return instance;
                    }

                    public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo DeserializeLengthDelimited(
                      Stream stream,
                      ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo instance)
                    {
                        if (instance.wifi_networks == null)
                            instance.wifi_networks = new List<ReportMessage.Session.WifiNetworkInfo>();
                        long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                        while (stream.Position < num1)
                        {
                            int num2 = stream.ReadByte();
                            switch (num2)
                            {
                                case -1:
                                    throw new EndOfStreamException();
                                case 8:
                                    instance.connection_type = (ReportMessage.Session.ConnectionType)ProtocolParser.ReadUInt64(stream);
                                    continue;
                                case 18:
                                    if (instance.cellular_network_info == null)
                                    {
                                        instance.cellular_network_info = ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo.DeserializeLengthDelimited(stream);
                                        continue;
                                    }
                                    ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo.DeserializeLengthDelimited(stream, instance.cellular_network_info);
                                    continue;
                                case 26:
                                    instance.wifi_networks.Add(ReportMessage.Session.WifiNetworkInfo.DeserializeLengthDelimited(stream));
                                    continue;
                                default:
                                    Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                    if (key.Field == 0U)
                                        throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                    ProtocolParser.SkipKey(stream, key);
                                    continue;
                            }
                        }
                        if (stream.Position != num1)
                            throw new ProtocolBufferException("Read past max limit");
                        return instance;
                    }

                    public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo DeserializeLength(
                      Stream stream,
                      int length,
                      ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo instance)
                    {
                        if (instance.wifi_networks == null)
                            instance.wifi_networks = new List<ReportMessage.Session.WifiNetworkInfo>();
                        long num1 = stream.Position + (long)length;
                        while (stream.Position < num1)
                        {
                            int num2 = stream.ReadByte();
                            switch (num2)
                            {
                                case -1:
                                    throw new EndOfStreamException();
                                case 8:
                                    instance.connection_type = (ReportMessage.Session.ConnectionType)ProtocolParser.ReadUInt64(stream);
                                    continue;
                                case 18:
                                    if (instance.cellular_network_info == null)
                                    {
                                        instance.cellular_network_info = ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo.DeserializeLengthDelimited(stream);
                                        continue;
                                    }
                                    ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo.DeserializeLengthDelimited(stream, instance.cellular_network_info);
                                    continue;
                                case 26:
                                    instance.wifi_networks.Add(ReportMessage.Session.WifiNetworkInfo.DeserializeLengthDelimited(stream));
                                    continue;
                                default:
                                    Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                    if (key.Field == 0U)
                                        throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                    ProtocolParser.SkipKey(stream, key);
                                    continue;
                            }
                        }
                        if (stream.Position != num1)
                            throw new ProtocolBufferException("Read past max limit");
                        return instance;
                    }

                    public static void Serialize(
                      Stream stream,
                      ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo instance)
                    {
                        MemoryStream stream1 = ProtocolParser.Stack.Pop();
                        stream.WriteByte((byte)8);
                        ProtocolParser.WriteUInt64(stream, (ulong)instance.connection_type);
                        if (instance.cellular_network_info != null)
                        {
                            stream.WriteByte((byte)18);
                            stream1.SetLength(0L);
                            ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo.Serialize((Stream)stream1, instance.cellular_network_info);
                            uint length = (uint)stream1.Length;
                            ProtocolParser.WriteUInt32(stream, length);
                            stream1.WriteTo(stream);
                        }
                        if (instance.wifi_networks != null)
                        {
                            foreach (ReportMessage.Session.WifiNetworkInfo wifiNetwork in instance.wifi_networks)
                            {
                                stream.WriteByte((byte)26);
                                stream1.SetLength(0L);
                                ReportMessage.Session.WifiNetworkInfo.Serialize((Stream)stream1, wifiNetwork);
                                uint length = (uint)stream1.Length;
                                ProtocolParser.WriteUInt32(stream, length);
                                stream1.WriteTo(stream);
                            }
                        }
                        ProtocolParser.Stack.Push(stream1);
                    }

                    public static byte[] SerializeToBytes(
                      ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo instance)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.Serialize((Stream)memoryStream, instance);
                            return memoryStream.ToArray();
                        }
                    }

                    public static void SerializeLengthDelimited(
                      Stream stream,
                      ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo instance)
                    {
                        byte[] bytes = ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.SerializeToBytes(instance);
                        ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                        stream.Write(bytes, 0, bytes.Length);
                    }

                    [DataContract]
                    public class CellularNetworkInfo
                    {
                        [DataMember]
                        public string network_type { get; set; }

                        [DataMember]
                        public uint? country_code { get; set; }

                        [DataMember]
                        public uint? operator_id { get; set; }

                        [DataMember]
                        public uint? cell_id { get; set; }

                        [DataMember]
                        public uint? lac { get; set; }

                        [DataMember]
                        public int? signal_strength { get; set; }

                        [DataMember]
                        public string operator_name { get; set; }

                        public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo Deserialize(
                          Stream stream)
                        {
                            ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo instance = new ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo();
                            ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo.Deserialize(stream, instance);
                            return instance;
                        }

                        public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo DeserializeLengthDelimited(
                          Stream stream)
                        {
                            ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo instance = new ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo();
                            ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo.DeserializeLengthDelimited(stream, instance);
                            return instance;
                        }

                        public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo DeserializeLength(
                          Stream stream,
                          int length)
                        {
                            ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo instance = new ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo();
                            ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo.DeserializeLength(stream, length, instance);
                            return instance;
                        }

                        public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo Deserialize(
                          byte[] buffer)
                        {
                            ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo instance = new ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo();
                            using (MemoryStream memoryStream = new MemoryStream(buffer))
                                ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo.Deserialize((Stream)memoryStream, instance);
                            return instance;
                        }

                        public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo Deserialize(
                          byte[] buffer,
                          ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo instance)
                        {
                            using (MemoryStream memoryStream = new MemoryStream(buffer))
                                ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo.Deserialize((Stream)memoryStream, instance);
                            return instance;
                        }

                        public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo Deserialize(
                          Stream stream,
                          ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo instance)
                        {
                            while (true)
                            {
                                int num = stream.ReadByte();
                                switch (num)
                                {
                                    case -1:
                                        goto label_11;
                                    case 10:
                                        instance.network_type = ProtocolParser.ReadString(stream);
                                        continue;
                                    case 16:
                                        instance.country_code = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 24:
                                        instance.operator_id = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 32:
                                        instance.cell_id = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 40:
                                        instance.lac = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 48:
                                        instance.signal_strength = new int?(ProtocolParser.ReadZInt32(stream));
                                        continue;
                                    case 58:
                                        instance.operator_name = ProtocolParser.ReadString(stream);
                                        continue;
                                    default:
                                        Key key = ProtocolParser.ReadKey((byte)num, stream);
                                        if (key.Field != 0U)
                                        {
                                            ProtocolParser.SkipKey(stream, key);
                                            continue;
                                        }
                                        goto label_9;
                                }
                            }
                            label_9:
                            throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            label_11:
                            return instance;
                        }

                        public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo DeserializeLengthDelimited(
                          Stream stream,
                          ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo instance)
                        {
                            long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                            while (stream.Position < num1)
                            {
                                int num2 = stream.ReadByte();
                                switch (num2)
                                {
                                    case -1:
                                        throw new EndOfStreamException();
                                    case 10:
                                        instance.network_type = ProtocolParser.ReadString(stream);
                                        continue;
                                    case 16:
                                        instance.country_code = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 24:
                                        instance.operator_id = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 32:
                                        instance.cell_id = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 40:
                                        instance.lac = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 48:
                                        instance.signal_strength = new int?(ProtocolParser.ReadZInt32(stream));
                                        continue;
                                    case 58:
                                        instance.operator_name = ProtocolParser.ReadString(stream);
                                        continue;
                                    default:
                                        Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                        if (key.Field == 0U)
                                            throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                        ProtocolParser.SkipKey(stream, key);
                                        continue;
                                }
                            }
                            if (stream.Position != num1)
                                throw new ProtocolBufferException("Read past max limit");
                            return instance;
                        }

                        public static ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo DeserializeLength(
                          Stream stream,
                          int length,
                          ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo instance)
                        {
                            long num1 = stream.Position + (long)length;
                            while (stream.Position < num1)
                            {
                                int num2 = stream.ReadByte();
                                switch (num2)
                                {
                                    case -1:
                                        throw new EndOfStreamException();
                                    case 10:
                                        instance.network_type = ProtocolParser.ReadString(stream);
                                        continue;
                                    case 16:
                                        instance.country_code = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 24:
                                        instance.operator_id = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 32:
                                        instance.cell_id = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 40:
                                        instance.lac = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 48:
                                        instance.signal_strength = new int?(ProtocolParser.ReadZInt32(stream));
                                        continue;
                                    case 58:
                                        instance.operator_name = ProtocolParser.ReadString(stream);
                                        continue;
                                    default:
                                        Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                        if (key.Field == 0U)
                                            throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                        ProtocolParser.SkipKey(stream, key);
                                        continue;
                                }
                            }
                            if (stream.Position != num1)
                                throw new ProtocolBufferException("Read past max limit");
                            return instance;
                        }

                        public static void Serialize(
                          Stream stream,
                          ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo instance)
                        {
                            MemoryStream stream1 = ProtocolParser.Stack.Pop();
                            if (instance.network_type != null)
                            {
                                stream.WriteByte((byte)10);
                                ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.network_type));
                            }
                            if (instance.country_code.HasValue)
                            {
                                stream.WriteByte((byte)16);
                                ProtocolParser.WriteUInt32(stream, instance.country_code.Value);
                            }
                            if (instance.operator_id.HasValue)
                            {
                                stream.WriteByte((byte)24);
                                ProtocolParser.WriteUInt32(stream, instance.operator_id.Value);
                            }
                            if (instance.cell_id.HasValue)
                            {
                                stream.WriteByte((byte)32);
                                ProtocolParser.WriteUInt32(stream, instance.cell_id.Value);
                            }
                            if (instance.lac.HasValue)
                            {
                                stream.WriteByte((byte)40);
                                ProtocolParser.WriteUInt32(stream, instance.lac.Value);
                            }
                            if (instance.signal_strength.HasValue)
                            {
                                stream.WriteByte((byte)48);
                                ProtocolParser.WriteZInt32(stream, instance.signal_strength.Value);
                            }
                            if (instance.operator_name != null)
                            {
                                stream.WriteByte((byte)58);
                                ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.operator_name));
                            }
                            ProtocolParser.Stack.Push(stream1);
                        }

                        public static byte[] SerializeToBytes(
                          ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo instance)
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo.Serialize((Stream)memoryStream, instance);
                                return memoryStream.ToArray();
                            }
                        }

                        public static void SerializeLengthDelimited(
                          Stream stream,
                          ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo instance)
                        {
                            byte[] bytes = ReportMessage.Session.SessionDesc.DeprecatedNetworkInfo.CellularNetworkInfo.SerializeToBytes(instance);
                            ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                            stream.Write(bytes, 0, bytes.Length);
                        }
                    }
                }
            }

            [DataContract]
            public class Event
            {
                [DataMember]
                public ulong number { get; set; }

                [DataMember]
                public ulong time { get; set; }

                [DataMember]
                public uint type { get; set; }

                [DataMember]
                public string name { get; set; }

                [DataMember]
                public byte[] value { get; set; }

                [DataMember]
                public ReportMessage.Location location { get; set; }

                [DataMember]
                public ReportMessage.Session.Event.NetworkInfo network_info { get; set; }

                [DataMember]
                public string environment { get; set; }

                [DataMember]
                public ReportMessage.Session.Event.Account account { get; set; }

                [DataMember]
                public uint? bytes_truncated { get; set; }

                [DataMember]
                public ReportMessage.Session.Event.EncryptionMode? encryption_mode { get; set; }

                public static ReportMessage.Session.Event Deserialize(Stream stream)
                {
                    ReportMessage.Session.Event instance = new ReportMessage.Session.Event();
                    ReportMessage.Session.Event.Deserialize(stream, instance);
                    return instance;
                }

                public static ReportMessage.Session.Event DeserializeLengthDelimited(
                  Stream stream)
                {
                    ReportMessage.Session.Event instance = new ReportMessage.Session.Event();
                    ReportMessage.Session.Event.DeserializeLengthDelimited(stream, instance);
                    return instance;
                }

                public static ReportMessage.Session.Event DeserializeLength(
                  Stream stream,
                  int length)
                {
                    ReportMessage.Session.Event instance = new ReportMessage.Session.Event();
                    ReportMessage.Session.Event.DeserializeLength(stream, length, instance);
                    return instance;
                }

                public static ReportMessage.Session.Event Deserialize(byte[] buffer)
                {
                    ReportMessage.Session.Event instance = new ReportMessage.Session.Event();
                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                        ReportMessage.Session.Event.Deserialize((Stream)memoryStream, instance);
                    return instance;
                }

                public static ReportMessage.Session.Event Deserialize(
                  byte[] buffer,
                  ReportMessage.Session.Event instance)
                {
                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                        ReportMessage.Session.Event.Deserialize((Stream)memoryStream, instance);
                    return instance;
                }

                public static ReportMessage.Session.Event Deserialize(
                  Stream stream,
                  ReportMessage.Session.Event instance)
                {
                    instance.encryption_mode = new ReportMessage.Session.Event.EncryptionMode?(ReportMessage.Session.Event.EncryptionMode.NONE);
                    while (true)
                    {
                        int num = stream.ReadByte();
                        switch (num)
                        {
                            case -1:
                                goto label_22;
                            case 8:
                                instance.number = ProtocolParser.ReadUInt64(stream);
                                continue;
                            case 16:
                                instance.time = ProtocolParser.ReadUInt64(stream);
                                continue;
                            case 24:
                                instance.type = ProtocolParser.ReadUInt32(stream);
                                continue;
                            case 34:
                                instance.name = ProtocolParser.ReadString(stream);
                                continue;
                            case 42:
                                instance.value = ProtocolParser.ReadBytes(stream);
                                continue;
                            case 50:
                                if (instance.location == null)
                                {
                                    instance.location = ReportMessage.Location.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Location.DeserializeLengthDelimited(stream, instance.location);
                                continue;
                            case 58:
                                if (instance.network_info == null)
                                {
                                    instance.network_info = ReportMessage.Session.Event.NetworkInfo.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Session.Event.NetworkInfo.DeserializeLengthDelimited(stream, instance.network_info);
                                continue;
                            case 66:
                                instance.environment = ProtocolParser.ReadString(stream);
                                continue;
                            case 74:
                                if (instance.account == null)
                                {
                                    instance.account = ReportMessage.Session.Event.Account.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Session.Event.Account.DeserializeLengthDelimited(stream, instance.account);
                                continue;
                            case 80:
                                instance.bytes_truncated = new uint?(ProtocolParser.ReadUInt32(stream));
                                continue;
                            case 96:
                                instance.encryption_mode = new ReportMessage.Session.Event.EncryptionMode?((ReportMessage.Session.Event.EncryptionMode)ProtocolParser.ReadUInt64(stream));
                                continue;
                            default:
                                Key key = ProtocolParser.ReadKey((byte)num, stream);
                                if (key.Field != 0U)
                                {
                                    ProtocolParser.SkipKey(stream, key);
                                    continue;
                                }
                                goto label_20;
                        }
                    }
                    label_20:
                    throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                    label_22:
                    return instance;
                }

                public static ReportMessage.Session.Event DeserializeLengthDelimited(
                  Stream stream,
                  ReportMessage.Session.Event instance)
                {
                    instance.encryption_mode = new ReportMessage.Session.Event.EncryptionMode?(ReportMessage.Session.Event.EncryptionMode.NONE);
                    long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                    while (stream.Position < num1)
                    {
                        int num2 = stream.ReadByte();
                        switch (num2)
                        {
                            case -1:
                                throw new EndOfStreamException();
                            case 8:
                                instance.number = ProtocolParser.ReadUInt64(stream);
                                continue;
                            case 16:
                                instance.time = ProtocolParser.ReadUInt64(stream);
                                continue;
                            case 24:
                                instance.type = ProtocolParser.ReadUInt32(stream);
                                continue;
                            case 34:
                                instance.name = ProtocolParser.ReadString(stream);
                                continue;
                            case 42:
                                instance.value = ProtocolParser.ReadBytes(stream);
                                continue;
                            case 50:
                                if (instance.location == null)
                                {
                                    instance.location = ReportMessage.Location.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Location.DeserializeLengthDelimited(stream, instance.location);
                                continue;
                            case 58:
                                if (instance.network_info == null)
                                {
                                    instance.network_info = ReportMessage.Session.Event.NetworkInfo.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Session.Event.NetworkInfo.DeserializeLengthDelimited(stream, instance.network_info);
                                continue;
                            case 66:
                                instance.environment = ProtocolParser.ReadString(stream);
                                continue;
                            case 74:
                                if (instance.account == null)
                                {
                                    instance.account = ReportMessage.Session.Event.Account.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Session.Event.Account.DeserializeLengthDelimited(stream, instance.account);
                                continue;
                            case 80:
                                instance.bytes_truncated = new uint?(ProtocolParser.ReadUInt32(stream));
                                continue;
                            case 96:
                                instance.encryption_mode = new ReportMessage.Session.Event.EncryptionMode?((ReportMessage.Session.Event.EncryptionMode)ProtocolParser.ReadUInt64(stream));
                                continue;
                            default:
                                Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                if (key.Field == 0U)
                                    throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                ProtocolParser.SkipKey(stream, key);
                                continue;
                        }
                    }
                    if (stream.Position != num1)
                        throw new ProtocolBufferException("Read past max limit");
                    return instance;
                }

                public static ReportMessage.Session.Event DeserializeLength(
                  Stream stream,
                  int length,
                  ReportMessage.Session.Event instance)
                {
                    instance.encryption_mode = new ReportMessage.Session.Event.EncryptionMode?(ReportMessage.Session.Event.EncryptionMode.NONE);
                    long num1 = stream.Position + (long)length;
                    while (stream.Position < num1)
                    {
                        int num2 = stream.ReadByte();
                        switch (num2)
                        {
                            case -1:
                                throw new EndOfStreamException();
                            case 8:
                                instance.number = ProtocolParser.ReadUInt64(stream);
                                continue;
                            case 16:
                                instance.time = ProtocolParser.ReadUInt64(stream);
                                continue;
                            case 24:
                                instance.type = ProtocolParser.ReadUInt32(stream);
                                continue;
                            case 34:
                                instance.name = ProtocolParser.ReadString(stream);
                                continue;
                            case 42:
                                instance.value = ProtocolParser.ReadBytes(stream);
                                continue;
                            case 50:
                                if (instance.location == null)
                                {
                                    instance.location = ReportMessage.Location.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Location.DeserializeLengthDelimited(stream, instance.location);
                                continue;
                            case 58:
                                if (instance.network_info == null)
                                {
                                    instance.network_info = ReportMessage.Session.Event.NetworkInfo.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Session.Event.NetworkInfo.DeserializeLengthDelimited(stream, instance.network_info);
                                continue;
                            case 66:
                                instance.environment = ProtocolParser.ReadString(stream);
                                continue;
                            case 74:
                                if (instance.account == null)
                                {
                                    instance.account = ReportMessage.Session.Event.Account.DeserializeLengthDelimited(stream);
                                    continue;
                                }
                                ReportMessage.Session.Event.Account.DeserializeLengthDelimited(stream, instance.account);
                                continue;
                            case 80:
                                instance.bytes_truncated = new uint?(ProtocolParser.ReadUInt32(stream));
                                continue;
                            case 96:
                                instance.encryption_mode = new ReportMessage.Session.Event.EncryptionMode?((ReportMessage.Session.Event.EncryptionMode)ProtocolParser.ReadUInt64(stream));
                                continue;
                            default:
                                Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                if (key.Field == 0U)
                                    throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                ProtocolParser.SkipKey(stream, key);
                                continue;
                        }
                    }
                    if (stream.Position != num1)
                        throw new ProtocolBufferException("Read past max limit");
                    return instance;
                }

                public static void Serialize(Stream stream, ReportMessage.Session.Event instance)
                {
                    MemoryStream stream1 = ProtocolParser.Stack.Pop();
                    stream.WriteByte((byte)8);
                    ProtocolParser.WriteUInt64(stream, instance.number);
                    stream.WriteByte((byte)16);
                    ProtocolParser.WriteUInt64(stream, instance.time);
                    stream.WriteByte((byte)24);
                    ProtocolParser.WriteUInt32(stream, instance.type);
                    if (instance.name != null)
                    {
                        stream.WriteByte((byte)34);
                        ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.name));
                    }
                    if (instance.value != null)
                    {
                        stream.WriteByte((byte)42);
                        ProtocolParser.WriteBytes(stream, instance.value);
                    }
                    if (instance.location != null)
                    {
                        stream.WriteByte((byte)50);
                        stream1.SetLength(0L);
                        ReportMessage.Location.Serialize((Stream)stream1, instance.location);
                        uint length = (uint)stream1.Length;
                        ProtocolParser.WriteUInt32(stream, length);
                        stream1.WriteTo(stream);
                    }
                    if (instance.network_info != null)
                    {
                        stream.WriteByte((byte)58);
                        stream1.SetLength(0L);
                        ReportMessage.Session.Event.NetworkInfo.Serialize((Stream)stream1, instance.network_info);
                        uint length = (uint)stream1.Length;
                        ProtocolParser.WriteUInt32(stream, length);
                        stream1.WriteTo(stream);
                    }
                    if (instance.environment != null)
                    {
                        stream.WriteByte((byte)66);
                        ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.environment));
                    }
                    if (instance.account != null)
                    {
                        stream.WriteByte((byte)74);
                        stream1.SetLength(0L);
                        ReportMessage.Session.Event.Account.Serialize((Stream)stream1, instance.account);
                        uint length = (uint)stream1.Length;
                        ProtocolParser.WriteUInt32(stream, length);
                        stream1.WriteTo(stream);
                    }
                    if (instance.bytes_truncated.HasValue)
                    {
                        stream.WriteByte((byte)80);
                        ProtocolParser.WriteUInt32(stream, instance.bytes_truncated.Value);
                    }
                    if (instance.encryption_mode.HasValue)
                    {
                        stream.WriteByte((byte)96);
                        ProtocolParser.WriteUInt64(stream, (ulong)instance.encryption_mode.Value);
                    }
                    ProtocolParser.Stack.Push(stream1);
                }

                public static byte[] SerializeToBytes(ReportMessage.Session.Event instance)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ReportMessage.Session.Event.Serialize((Stream)memoryStream, instance);
                        return memoryStream.ToArray();
                    }
                }

                public static void SerializeLengthDelimited(
                  Stream stream,
                  ReportMessage.Session.Event instance)
                {
                    byte[] bytes = ReportMessage.Session.Event.SerializeToBytes(instance);
                    ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                    stream.Write(bytes, 0, bytes.Length);
                }

                public enum EventType
                {
                    EVENT_INIT = 1,
                    EVENT_START = 2,
                    EVENT_CRASH = 3,
                    EVENT_CLIENT = 4,
                    EVENT_REFERRER = 5,
                    EVENT_ERROR = 6,
                    EVENT_ALIVE = 7,
                    EVENT_IDENTITY = 8,
                    EVENT_AD_CLICK = 9,
                    EVENT_AD_INSTALL = 10, // 0x0000000A
                    EVENT_STATBOX = 11, // 0x0000000B
                    EVENT_ACCOUNT = 12, // 0x0000000C
                    EVENT_FIRST = 13, // 0x0000000D
                    EVENT_PUSH_TOKEN = 14, // 0x0000000E
                    EVENT_NOTIFICATION = 15, // 0x0000000F
                    EVENT_OPEN = 16, // 0x00000010
                    EVENT_UPDATE = 17, // 0x00000011
                    EVENT_PERMISSIONS = 18, // 0x00000012
                    EVENT_APP_FEATURES = 19, // 0x00000013
                }

                public enum EncryptionMode
                {
                    NONE,
                    RSA_AES_CBC,
                }

                [DataContract]
                public class NetworkInfo
                {
                    [DataMember]
                    public List<ReportMessage.Session.Event.NetworkInfo.Cell> cell { get; set; }

                    [DataMember]
                    public List<ReportMessage.Session.WifiNetworkInfo> wifi_networks { get; set; }

                    [DataMember]
                    public ReportMessage.Session.ConnectionType? connection_type { get; set; }

                    [DataMember]
                    public string cellular_network_type { get; set; }

                    [DataMember]
                    public ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint wifi_access_point { get; set; }

                    public static ReportMessage.Session.Event.NetworkInfo Deserialize(
                      Stream stream)
                    {
                        ReportMessage.Session.Event.NetworkInfo instance = new ReportMessage.Session.Event.NetworkInfo();
                        ReportMessage.Session.Event.NetworkInfo.Deserialize(stream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.Event.NetworkInfo DeserializeLengthDelimited(
                      Stream stream)
                    {
                        ReportMessage.Session.Event.NetworkInfo instance = new ReportMessage.Session.Event.NetworkInfo();
                        ReportMessage.Session.Event.NetworkInfo.DeserializeLengthDelimited(stream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.Event.NetworkInfo DeserializeLength(
                      Stream stream,
                      int length)
                    {
                        ReportMessage.Session.Event.NetworkInfo instance = new ReportMessage.Session.Event.NetworkInfo();
                        ReportMessage.Session.Event.NetworkInfo.DeserializeLength(stream, length, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.Event.NetworkInfo Deserialize(
                      byte[] buffer)
                    {
                        ReportMessage.Session.Event.NetworkInfo instance = new ReportMessage.Session.Event.NetworkInfo();
                        using (MemoryStream memoryStream = new MemoryStream(buffer))
                            ReportMessage.Session.Event.NetworkInfo.Deserialize((Stream)memoryStream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.Event.NetworkInfo Deserialize(
                      byte[] buffer,
                      ReportMessage.Session.Event.NetworkInfo instance)
                    {
                        using (MemoryStream memoryStream = new MemoryStream(buffer))
                            ReportMessage.Session.Event.NetworkInfo.Deserialize((Stream)memoryStream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.Event.NetworkInfo Deserialize(
                      Stream stream,
                      ReportMessage.Session.Event.NetworkInfo instance)
                    {
                        if (instance.cell == null)
                            instance.cell = new List<ReportMessage.Session.Event.NetworkInfo.Cell>();
                        if (instance.wifi_networks == null)
                            instance.wifi_networks = new List<ReportMessage.Session.WifiNetworkInfo>();
                        instance.connection_type = new ReportMessage.Session.ConnectionType?(ReportMessage.Session.ConnectionType.CONNECTION_UNDEFINED);
                        while (true)
                        {
                            int num = stream.ReadByte();
                            switch (num)
                            {
                                case -1:
                                    goto label_16;
                                case 10:
                                    instance.cell.Add(ReportMessage.Session.Event.NetworkInfo.Cell.DeserializeLengthDelimited(stream));
                                    continue;
                                case 18:
                                    instance.wifi_networks.Add(ReportMessage.Session.WifiNetworkInfo.DeserializeLengthDelimited(stream));
                                    continue;
                                case 24:
                                    instance.connection_type = new ReportMessage.Session.ConnectionType?((ReportMessage.Session.ConnectionType)ProtocolParser.ReadUInt64(stream));
                                    continue;
                                case 34:
                                    instance.cellular_network_type = ProtocolParser.ReadString(stream);
                                    continue;
                                case 42:
                                    if (instance.wifi_access_point == null)
                                    {
                                        instance.wifi_access_point = ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.DeserializeLengthDelimited(stream);
                                        continue;
                                    }
                                    ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.DeserializeLengthDelimited(stream, instance.wifi_access_point);
                                    continue;
                                default:
                                    Key key = ProtocolParser.ReadKey((byte)num, stream);
                                    if (key.Field != 0U)
                                    {
                                        ProtocolParser.SkipKey(stream, key);
                                        continue;
                                    }
                                    goto label_14;
                            }
                        }
                        label_14:
                        throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                        label_16:
                        return instance;
                    }

                    public static ReportMessage.Session.Event.NetworkInfo DeserializeLengthDelimited(
                      Stream stream,
                      ReportMessage.Session.Event.NetworkInfo instance)
                    {
                        if (instance.cell == null)
                            instance.cell = new List<ReportMessage.Session.Event.NetworkInfo.Cell>();
                        if (instance.wifi_networks == null)
                            instance.wifi_networks = new List<ReportMessage.Session.WifiNetworkInfo>();
                        instance.connection_type = new ReportMessage.Session.ConnectionType?(ReportMessage.Session.ConnectionType.CONNECTION_UNDEFINED);
                        long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                        while (stream.Position < num1)
                        {
                            int num2 = stream.ReadByte();
                            switch (num2)
                            {
                                case -1:
                                    throw new EndOfStreamException();
                                case 10:
                                    instance.cell.Add(ReportMessage.Session.Event.NetworkInfo.Cell.DeserializeLengthDelimited(stream));
                                    continue;
                                case 18:
                                    instance.wifi_networks.Add(ReportMessage.Session.WifiNetworkInfo.DeserializeLengthDelimited(stream));
                                    continue;
                                case 24:
                                    instance.connection_type = new ReportMessage.Session.ConnectionType?((ReportMessage.Session.ConnectionType)ProtocolParser.ReadUInt64(stream));
                                    continue;
                                case 34:
                                    instance.cellular_network_type = ProtocolParser.ReadString(stream);
                                    continue;
                                case 42:
                                    if (instance.wifi_access_point == null)
                                    {
                                        instance.wifi_access_point = ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.DeserializeLengthDelimited(stream);
                                        continue;
                                    }
                                    ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.DeserializeLengthDelimited(stream, instance.wifi_access_point);
                                    continue;
                                default:
                                    Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                    if (key.Field == 0U)
                                        throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                    ProtocolParser.SkipKey(stream, key);
                                    continue;
                            }
                        }
                        if (stream.Position != num1)
                            throw new ProtocolBufferException("Read past max limit");
                        return instance;
                    }

                    public static ReportMessage.Session.Event.NetworkInfo DeserializeLength(
                      Stream stream,
                      int length,
                      ReportMessage.Session.Event.NetworkInfo instance)
                    {
                        if (instance.cell == null)
                            instance.cell = new List<ReportMessage.Session.Event.NetworkInfo.Cell>();
                        if (instance.wifi_networks == null)
                            instance.wifi_networks = new List<ReportMessage.Session.WifiNetworkInfo>();
                        instance.connection_type = new ReportMessage.Session.ConnectionType?(ReportMessage.Session.ConnectionType.CONNECTION_UNDEFINED);
                        long num1 = stream.Position + (long)length;
                        while (stream.Position < num1)
                        {
                            int num2 = stream.ReadByte();
                            switch (num2)
                            {
                                case -1:
                                    throw new EndOfStreamException();
                                case 10:
                                    instance.cell.Add(ReportMessage.Session.Event.NetworkInfo.Cell.DeserializeLengthDelimited(stream));
                                    continue;
                                case 18:
                                    instance.wifi_networks.Add(ReportMessage.Session.WifiNetworkInfo.DeserializeLengthDelimited(stream));
                                    continue;
                                case 24:
                                    instance.connection_type = new ReportMessage.Session.ConnectionType?((ReportMessage.Session.ConnectionType)ProtocolParser.ReadUInt64(stream));
                                    continue;
                                case 34:
                                    instance.cellular_network_type = ProtocolParser.ReadString(stream);
                                    continue;
                                case 42:
                                    if (instance.wifi_access_point == null)
                                    {
                                        instance.wifi_access_point = ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.DeserializeLengthDelimited(stream);
                                        continue;
                                    }
                                    ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.DeserializeLengthDelimited(stream, instance.wifi_access_point);
                                    continue;
                                default:
                                    Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                    if (key.Field == 0U)
                                        throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                    ProtocolParser.SkipKey(stream, key);
                                    continue;
                            }
                        }
                        if (stream.Position != num1)
                            throw new ProtocolBufferException("Read past max limit");
                        return instance;
                    }

                    public static void Serialize(
                      Stream stream,
                      ReportMessage.Session.Event.NetworkInfo instance)
                    {
                        MemoryStream stream1 = ProtocolParser.Stack.Pop();
                        if (instance.cell != null)
                        {
                            foreach (ReportMessage.Session.Event.NetworkInfo.Cell instance1 in instance.cell)
                            {
                                stream.WriteByte((byte)10);
                                stream1.SetLength(0L);
                                ReportMessage.Session.Event.NetworkInfo.Cell.Serialize((Stream)stream1, instance1);
                                uint length = (uint)stream1.Length;
                                ProtocolParser.WriteUInt32(stream, length);
                                stream1.WriteTo(stream);
                            }
                        }
                        if (instance.wifi_networks != null)
                        {
                            foreach (ReportMessage.Session.WifiNetworkInfo wifiNetwork in instance.wifi_networks)
                            {
                                stream.WriteByte((byte)18);
                                stream1.SetLength(0L);
                                ReportMessage.Session.WifiNetworkInfo.Serialize((Stream)stream1, wifiNetwork);
                                uint length = (uint)stream1.Length;
                                ProtocolParser.WriteUInt32(stream, length);
                                stream1.WriteTo(stream);
                            }
                        }
                        if (instance.connection_type.HasValue)
                        {
                            stream.WriteByte((byte)24);
                            ProtocolParser.WriteUInt64(stream, (ulong)instance.connection_type.Value);
                        }
                        if (instance.cellular_network_type != null)
                        {
                            stream.WriteByte((byte)34);
                            ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.cellular_network_type));
                        }
                        if (instance.wifi_access_point != null)
                        {
                            stream.WriteByte((byte)42);
                            stream1.SetLength(0L);
                            ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.Serialize((Stream)stream1, instance.wifi_access_point);
                            uint length = (uint)stream1.Length;
                            ProtocolParser.WriteUInt32(stream, length);
                            stream1.WriteTo(stream);
                        }
                        ProtocolParser.Stack.Push(stream1);
                    }

                    public static byte[] SerializeToBytes(ReportMessage.Session.Event.NetworkInfo instance)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            ReportMessage.Session.Event.NetworkInfo.Serialize((Stream)memoryStream, instance);
                            return memoryStream.ToArray();
                        }
                    }

                    public static void SerializeLengthDelimited(
                      Stream stream,
                      ReportMessage.Session.Event.NetworkInfo instance)
                    {
                        byte[] bytes = ReportMessage.Session.Event.NetworkInfo.SerializeToBytes(instance);
                        ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                        stream.Write(bytes, 0, bytes.Length);
                    }

                    [DataContract]
                    public class Cell
                    {
                        [DataMember]
                        public uint? cell_id { get; set; }

                        [DataMember]
                        public int? signal_strength { get; set; }

                        [DataMember]
                        public uint? lac { get; set; }

                        [DataMember]
                        public uint? country_code { get; set; }

                        [DataMember]
                        public uint? operator_id { get; set; }

                        [DataMember]
                        public string operator_name { get; set; }

                        [DataMember]
                        public bool? is_connected { get; set; }

                        [DataMember]
                        public ReportMessage.Session.Event.NetworkInfo.Cell.Type? type { get; set; }

                        [DataMember]
                        public uint? pci { get; set; }

                        public static ReportMessage.Session.Event.NetworkInfo.Cell Deserialize(
                          Stream stream)
                        {
                            ReportMessage.Session.Event.NetworkInfo.Cell instance = new ReportMessage.Session.Event.NetworkInfo.Cell();
                            ReportMessage.Session.Event.NetworkInfo.Cell.Deserialize(stream, instance);
                            return instance;
                        }

                        public static ReportMessage.Session.Event.NetworkInfo.Cell DeserializeLengthDelimited(
                          Stream stream)
                        {
                            ReportMessage.Session.Event.NetworkInfo.Cell instance = new ReportMessage.Session.Event.NetworkInfo.Cell();
                            ReportMessage.Session.Event.NetworkInfo.Cell.DeserializeLengthDelimited(stream, instance);
                            return instance;
                        }

                        public static ReportMessage.Session.Event.NetworkInfo.Cell DeserializeLength(
                          Stream stream,
                          int length)
                        {
                            ReportMessage.Session.Event.NetworkInfo.Cell instance = new ReportMessage.Session.Event.NetworkInfo.Cell();
                            ReportMessage.Session.Event.NetworkInfo.Cell.DeserializeLength(stream, length, instance);
                            return instance;
                        }

                        public static ReportMessage.Session.Event.NetworkInfo.Cell Deserialize(
                          byte[] buffer)
                        {
                            ReportMessage.Session.Event.NetworkInfo.Cell instance = new ReportMessage.Session.Event.NetworkInfo.Cell();
                            using (MemoryStream memoryStream = new MemoryStream(buffer))
                                ReportMessage.Session.Event.NetworkInfo.Cell.Deserialize((Stream)memoryStream, instance);
                            return instance;
                        }

                        public static ReportMessage.Session.Event.NetworkInfo.Cell Deserialize(
                          byte[] buffer,
                          ReportMessage.Session.Event.NetworkInfo.Cell instance)
                        {
                            using (MemoryStream memoryStream = new MemoryStream(buffer))
                                ReportMessage.Session.Event.NetworkInfo.Cell.Deserialize((Stream)memoryStream, instance);
                            return instance;
                        }

                        public static ReportMessage.Session.Event.NetworkInfo.Cell Deserialize(
                          Stream stream,
                          ReportMessage.Session.Event.NetworkInfo.Cell instance)
                        {
                            instance.is_connected = new bool?(false);
                            instance.type = new ReportMessage.Session.Event.NetworkInfo.Cell.Type?(ReportMessage.Session.Event.NetworkInfo.Cell.Type.TYPE_DEFAULT);
                            while (true)
                            {
                                int num = stream.ReadByte();
                                switch (num)
                                {
                                    case -1:
                                        goto label_14;
                                    case 8:
                                        instance.cell_id = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 16:
                                        instance.signal_strength = new int?(ProtocolParser.ReadZInt32(stream));
                                        continue;
                                    case 24:
                                        instance.lac = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 32:
                                        instance.country_code = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 40:
                                        instance.operator_id = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 50:
                                        instance.operator_name = ProtocolParser.ReadString(stream);
                                        continue;
                                    case 56:
                                        instance.is_connected = new bool?(ProtocolParser.ReadBool(stream));
                                        continue;
                                    case 64:
                                        instance.type = new ReportMessage.Session.Event.NetworkInfo.Cell.Type?((ReportMessage.Session.Event.NetworkInfo.Cell.Type)ProtocolParser.ReadUInt64(stream));
                                        continue;
                                    case 72:
                                        instance.pci = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    default:
                                        Key key = ProtocolParser.ReadKey((byte)num, stream);
                                        if (key.Field != 0U)
                                        {
                                            ProtocolParser.SkipKey(stream, key);
                                            continue;
                                        }
                                        goto label_12;
                                }
                            }
                            label_12:
                            throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            label_14:
                            return instance;
                        }

                        public static ReportMessage.Session.Event.NetworkInfo.Cell DeserializeLengthDelimited(
                          Stream stream,
                          ReportMessage.Session.Event.NetworkInfo.Cell instance)
                        {
                            instance.is_connected = new bool?(false);
                            instance.type = new ReportMessage.Session.Event.NetworkInfo.Cell.Type?(ReportMessage.Session.Event.NetworkInfo.Cell.Type.TYPE_DEFAULT);
                            long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                            while (stream.Position < num1)
                            {
                                int num2 = stream.ReadByte();
                                switch (num2)
                                {
                                    case -1:
                                        throw new EndOfStreamException();
                                    case 8:
                                        instance.cell_id = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 16:
                                        instance.signal_strength = new int?(ProtocolParser.ReadZInt32(stream));
                                        continue;
                                    case 24:
                                        instance.lac = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 32:
                                        instance.country_code = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 40:
                                        instance.operator_id = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 50:
                                        instance.operator_name = ProtocolParser.ReadString(stream);
                                        continue;
                                    case 56:
                                        instance.is_connected = new bool?(ProtocolParser.ReadBool(stream));
                                        continue;
                                    case 64:
                                        instance.type = new ReportMessage.Session.Event.NetworkInfo.Cell.Type?((ReportMessage.Session.Event.NetworkInfo.Cell.Type)ProtocolParser.ReadUInt64(stream));
                                        continue;
                                    case 72:
                                        instance.pci = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    default:
                                        Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                        if (key.Field == 0U)
                                            throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                        ProtocolParser.SkipKey(stream, key);
                                        continue;
                                }
                            }
                            if (stream.Position != num1)
                                throw new ProtocolBufferException("Read past max limit");
                            return instance;
                        }

                        public static ReportMessage.Session.Event.NetworkInfo.Cell DeserializeLength(
                          Stream stream,
                          int length,
                          ReportMessage.Session.Event.NetworkInfo.Cell instance)
                        {
                            instance.is_connected = new bool?(false);
                            instance.type = new ReportMessage.Session.Event.NetworkInfo.Cell.Type?(ReportMessage.Session.Event.NetworkInfo.Cell.Type.TYPE_DEFAULT);
                            long num1 = stream.Position + (long)length;
                            while (stream.Position < num1)
                            {
                                int num2 = stream.ReadByte();
                                switch (num2)
                                {
                                    case -1:
                                        throw new EndOfStreamException();
                                    case 8:
                                        instance.cell_id = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 16:
                                        instance.signal_strength = new int?(ProtocolParser.ReadZInt32(stream));
                                        continue;
                                    case 24:
                                        instance.lac = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 32:
                                        instance.country_code = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 40:
                                        instance.operator_id = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    case 50:
                                        instance.operator_name = ProtocolParser.ReadString(stream);
                                        continue;
                                    case 56:
                                        instance.is_connected = new bool?(ProtocolParser.ReadBool(stream));
                                        continue;
                                    case 64:
                                        instance.type = new ReportMessage.Session.Event.NetworkInfo.Cell.Type?((ReportMessage.Session.Event.NetworkInfo.Cell.Type)ProtocolParser.ReadUInt64(stream));
                                        continue;
                                    case 72:
                                        instance.pci = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    default:
                                        Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                        if (key.Field == 0U)
                                            throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                        ProtocolParser.SkipKey(stream, key);
                                        continue;
                                }
                            }
                            if (stream.Position != num1)
                                throw new ProtocolBufferException("Read past max limit");
                            return instance;
                        }

                        public static void Serialize(
                          Stream stream,
                          ReportMessage.Session.Event.NetworkInfo.Cell instance)
                        {
                            MemoryStream stream1 = ProtocolParser.Stack.Pop();
                            if (instance.cell_id.HasValue)
                            {
                                stream.WriteByte((byte)8);
                                ProtocolParser.WriteUInt32(stream, instance.cell_id.Value);
                            }
                            if (instance.signal_strength.HasValue)
                            {
                                stream.WriteByte((byte)16);
                                ProtocolParser.WriteZInt32(stream, instance.signal_strength.Value);
                            }
                            if (instance.lac.HasValue)
                            {
                                stream.WriteByte((byte)24);
                                ProtocolParser.WriteUInt32(stream, instance.lac.Value);
                            }
                            if (instance.country_code.HasValue)
                            {
                                stream.WriteByte((byte)32);
                                ProtocolParser.WriteUInt32(stream, instance.country_code.Value);
                            }
                            if (instance.operator_id.HasValue)
                            {
                                stream.WriteByte((byte)40);
                                ProtocolParser.WriteUInt32(stream, instance.operator_id.Value);
                            }
                            if (instance.operator_name != null)
                            {
                                stream.WriteByte((byte)50);
                                ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.operator_name));
                            }
                            if (instance.is_connected.HasValue)
                            {
                                stream.WriteByte((byte)56);
                                ProtocolParser.WriteBool(stream, instance.is_connected.Value);
                            }
                            if (instance.type.HasValue)
                            {
                                stream.WriteByte((byte)64);
                                ProtocolParser.WriteUInt64(stream, (ulong)instance.type.Value);
                            }
                            if (instance.pci.HasValue)
                            {
                                stream.WriteByte((byte)72);
                                ProtocolParser.WriteUInt32(stream, instance.pci.Value);
                            }
                            ProtocolParser.Stack.Push(stream1);
                        }

                        public static byte[] SerializeToBytes(
                          ReportMessage.Session.Event.NetworkInfo.Cell instance)
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                ReportMessage.Session.Event.NetworkInfo.Cell.Serialize((Stream)memoryStream, instance);
                                return memoryStream.ToArray();
                            }
                        }

                        public static void SerializeLengthDelimited(
                          Stream stream,
                          ReportMessage.Session.Event.NetworkInfo.Cell instance)
                        {
                            byte[] bytes = ReportMessage.Session.Event.NetworkInfo.Cell.SerializeToBytes(instance);
                            ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                            stream.Write(bytes, 0, bytes.Length);
                        }

                        public enum Type
                        {
                            TYPE_DEFAULT,
                            TYPE_GSM,
                            TYPE_CDMA,
                            TYPE_WCDMA,
                            TYPE_LTE,
                        }
                    }

                    [DataContract]
                    public class WifiAccessPoint
                    {
                        [DataMember]
                        public string ssid { get; set; }

                        [DataMember]
                        public ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.State? state { get; set; }

                        public static ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint Deserialize(
                          Stream stream)
                        {
                            ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint instance = new ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint();
                            ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.Deserialize(stream, instance);
                            return instance;
                        }

                        public static ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint DeserializeLengthDelimited(
                          Stream stream)
                        {
                            ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint instance = new ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint();
                            ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.DeserializeLengthDelimited(stream, instance);
                            return instance;
                        }

                        public static ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint DeserializeLength(
                          Stream stream,
                          int length)
                        {
                            ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint instance = new ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint();
                            ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.DeserializeLength(stream, length, instance);
                            return instance;
                        }

                        public static ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint Deserialize(
                          byte[] buffer)
                        {
                            ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint instance = new ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint();
                            using (MemoryStream memoryStream = new MemoryStream(buffer))
                                ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.Deserialize((Stream)memoryStream, instance);
                            return instance;
                        }

                        public static ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint Deserialize(
                          byte[] buffer,
                          ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint instance)
                        {
                            using (MemoryStream memoryStream = new MemoryStream(buffer))
                                ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.Deserialize((Stream)memoryStream, instance);
                            return instance;
                        }

                        public static ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint Deserialize(
                          Stream stream,
                          ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint instance)
                        {
                            instance.state = new ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.State?(ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.State.STATE_UNKNOWN);
                            while (true)
                            {
                                int num = stream.ReadByte();
                                switch (num)
                                {
                                    case -1:
                                        goto label_7;
                                    case 10:
                                        instance.ssid = ProtocolParser.ReadString(stream);
                                        continue;
                                    case 16:
                                        instance.state = new ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.State?((ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.State)ProtocolParser.ReadUInt64(stream));
                                        continue;
                                    default:
                                        Key key = ProtocolParser.ReadKey((byte)num, stream);
                                        if (key.Field != 0U)
                                        {
                                            ProtocolParser.SkipKey(stream, key);
                                            continue;
                                        }
                                        goto label_5;
                                }
                            }
                            label_5:
                            throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            label_7:
                            return instance;
                        }

                        public static ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint DeserializeLengthDelimited(
                          Stream stream,
                          ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint instance)
                        {
                            instance.state = new ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.State?(ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.State.STATE_UNKNOWN);
                            long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                            while (stream.Position < num1)
                            {
                                int num2 = stream.ReadByte();
                                switch (num2)
                                {
                                    case -1:
                                        throw new EndOfStreamException();
                                    case 10:
                                        instance.ssid = ProtocolParser.ReadString(stream);
                                        continue;
                                    case 16:
                                        instance.state = new ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.State?((ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.State)ProtocolParser.ReadUInt64(stream));
                                        continue;
                                    default:
                                        Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                        if (key.Field == 0U)
                                            throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                        ProtocolParser.SkipKey(stream, key);
                                        continue;
                                }
                            }
                            if (stream.Position != num1)
                                throw new ProtocolBufferException("Read past max limit");
                            return instance;
                        }

                        public static ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint DeserializeLength(
                          Stream stream,
                          int length,
                          ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint instance)
                        {
                            instance.state = new ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.State?(ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.State.STATE_UNKNOWN);
                            long num1 = stream.Position + (long)length;
                            while (stream.Position < num1)
                            {
                                int num2 = stream.ReadByte();
                                switch (num2)
                                {
                                    case -1:
                                        throw new EndOfStreamException();
                                    case 10:
                                        instance.ssid = ProtocolParser.ReadString(stream);
                                        continue;
                                    case 16:
                                        instance.state = new ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.State?((ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.State)ProtocolParser.ReadUInt64(stream));
                                        continue;
                                    default:
                                        Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                        if (key.Field == 0U)
                                            throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                        ProtocolParser.SkipKey(stream, key);
                                        continue;
                                }
                            }
                            if (stream.Position != num1)
                                throw new ProtocolBufferException("Read past max limit");
                            return instance;
                        }

                        public static void Serialize(
                          Stream stream,
                          ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint instance)
                        {
                            MemoryStream stream1 = ProtocolParser.Stack.Pop();
                            if (instance.ssid == null)
                                throw new ProtocolBufferException("ssid is required by the proto specification.");
                            stream.WriteByte((byte)10);
                            ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.ssid));
                            ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.State? state = instance.state;
                            if (state.HasValue)
                            {
                                stream.WriteByte((byte)16);
                                Stream stream2 = stream;
                                state = instance.state;
                                long num = (long)state.Value;
                                ProtocolParser.WriteUInt64(stream2, (ulong)num);
                            }
                            ProtocolParser.Stack.Push(stream1);
                        }

                        public static byte[] SerializeToBytes(
                          ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint instance)
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.Serialize((Stream)memoryStream, instance);
                                return memoryStream.ToArray();
                            }
                        }

                        public static void SerializeLengthDelimited(
                          Stream stream,
                          ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint instance)
                        {
                            byte[] bytes = ReportMessage.Session.Event.NetworkInfo.WifiAccessPoint.SerializeToBytes(instance);
                            ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                            stream.Write(bytes, 0, bytes.Length);
                        }

                        public enum State
                        {
                            STATE_UNKNOWN,
                            STATE_DISABLED,
                            STATE_ENABLED,
                        }
                    }
                }

                [DataContract]
                public class Account
                {
                    [DataMember]
                    public string id { get; set; }

                    [DataMember]
                    public string type { get; set; }

                    [DataMember]
                    public string options { get; set; }

                    public static ReportMessage.Session.Event.Account Deserialize(Stream stream)
                    {
                        ReportMessage.Session.Event.Account instance = new ReportMessage.Session.Event.Account();
                        ReportMessage.Session.Event.Account.Deserialize(stream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.Event.Account DeserializeLengthDelimited(
                      Stream stream)
                    {
                        ReportMessage.Session.Event.Account instance = new ReportMessage.Session.Event.Account();
                        ReportMessage.Session.Event.Account.DeserializeLengthDelimited(stream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.Event.Account DeserializeLength(
                      Stream stream,
                      int length)
                    {
                        ReportMessage.Session.Event.Account instance = new ReportMessage.Session.Event.Account();
                        ReportMessage.Session.Event.Account.DeserializeLength(stream, length, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.Event.Account Deserialize(byte[] buffer)
                    {
                        ReportMessage.Session.Event.Account instance = new ReportMessage.Session.Event.Account();
                        using (MemoryStream memoryStream = new MemoryStream(buffer))
                            ReportMessage.Session.Event.Account.Deserialize((Stream)memoryStream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.Event.Account Deserialize(
                      byte[] buffer,
                      ReportMessage.Session.Event.Account instance)
                    {
                        using (MemoryStream memoryStream = new MemoryStream(buffer))
                            ReportMessage.Session.Event.Account.Deserialize((Stream)memoryStream, instance);
                        return instance;
                    }

                    public static ReportMessage.Session.Event.Account Deserialize(
                      Stream stream,
                      ReportMessage.Session.Event.Account instance)
                    {
                        while (true)
                        {
                            int num = stream.ReadByte();
                            switch (num)
                            {
                                case -1:
                                    goto label_7;
                                case 10:
                                    instance.id = ProtocolParser.ReadString(stream);
                                    continue;
                                case 18:
                                    instance.type = ProtocolParser.ReadString(stream);
                                    continue;
                                case 26:
                                    instance.options = ProtocolParser.ReadString(stream);
                                    continue;
                                default:
                                    Key key = ProtocolParser.ReadKey((byte)num, stream);
                                    if (key.Field != 0U)
                                    {
                                        ProtocolParser.SkipKey(stream, key);
                                        continue;
                                    }
                                    goto label_5;
                            }
                        }
                        label_5:
                        throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                        label_7:
                        return instance;
                    }

                    public static ReportMessage.Session.Event.Account DeserializeLengthDelimited(
                      Stream stream,
                      ReportMessage.Session.Event.Account instance)
                    {
                        long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                        while (stream.Position < num1)
                        {
                            int num2 = stream.ReadByte();
                            switch (num2)
                            {
                                case -1:
                                    throw new EndOfStreamException();
                                case 10:
                                    instance.id = ProtocolParser.ReadString(stream);
                                    continue;
                                case 18:
                                    instance.type = ProtocolParser.ReadString(stream);
                                    continue;
                                case 26:
                                    instance.options = ProtocolParser.ReadString(stream);
                                    continue;
                                default:
                                    Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                    if (key.Field == 0U)
                                        throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                    ProtocolParser.SkipKey(stream, key);
                                    continue;
                            }
                        }
                        if (stream.Position != num1)
                            throw new ProtocolBufferException("Read past max limit");
                        return instance;
                    }

                    public static ReportMessage.Session.Event.Account DeserializeLength(
                      Stream stream,
                      int length,
                      ReportMessage.Session.Event.Account instance)
                    {
                        long num1 = stream.Position + (long)length;
                        while (stream.Position < num1)
                        {
                            int num2 = stream.ReadByte();
                            switch (num2)
                            {
                                case -1:
                                    throw new EndOfStreamException();
                                case 10:
                                    instance.id = ProtocolParser.ReadString(stream);
                                    continue;
                                case 18:
                                    instance.type = ProtocolParser.ReadString(stream);
                                    continue;
                                case 26:
                                    instance.options = ProtocolParser.ReadString(stream);
                                    continue;
                                default:
                                    Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                    if (key.Field == 0U)
                                        throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                    ProtocolParser.SkipKey(stream, key);
                                    continue;
                            }
                        }
                        if (stream.Position != num1)
                            throw new ProtocolBufferException("Read past max limit");
                        return instance;
                    }

                    public static void Serialize(Stream stream, ReportMessage.Session.Event.Account instance)
                    {
                        MemoryStream stream1 = ProtocolParser.Stack.Pop();
                        if (instance.id == null)
                            throw new ProtocolBufferException("id is required by the proto specification.");
                        stream.WriteByte((byte)10);
                        ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.id));
                        if (instance.type != null)
                        {
                            stream.WriteByte((byte)18);
                            ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.type));
                        }
                        if (instance.options != null)
                        {
                            stream.WriteByte((byte)26);
                            ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.options));
                        }
                        ProtocolParser.Stack.Push(stream1);
                    }

                    public static byte[] SerializeToBytes(ReportMessage.Session.Event.Account instance)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            ReportMessage.Session.Event.Account.Serialize((Stream)memoryStream, instance);
                            return memoryStream.ToArray();
                        }
                    }

                    public static void SerializeLengthDelimited(
                      Stream stream,
                      ReportMessage.Session.Event.Account instance)
                    {
                        byte[] bytes = ReportMessage.Session.Event.Account.SerializeToBytes(instance);
                        ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        [DataContract]
        public class RequestParameters
        {
            [DataMember]
            public string uuid { get; set; }

            [DataMember]
            public string device_id { get; set; }

            [DataMember]
            public string app_platform { get; set; }

            [DataMember]
            public string app_version_name { get; set; }

            [DataMember]
            public uint? kit_version { get; set; }

            [DataMember]
            public uint? api_key { get; set; }

            [DataMember]
            public string app_id { get; set; }

            [DataMember]
            public string manufacturer { get; set; }

            [DataMember]
            public string model { get; set; }

            [DataMember]
            public string os_version { get; set; }

            [DataMember]
            public uint? screen_width { get; set; }

            [DataMember]
            public uint? screen_height { get; set; }

            [DataMember]
            public uint? screen_dpi { get; set; }

            [DataMember]
            public double? scale_factor { get; set; }

            [DataMember]
            public string locale { get; set; }

            [DataMember]
            public ReportMessage.RequestParameters.DeviceType? device_type { get; set; }

            [DataMember]
            public bool? is_rooted { get; set; }

            [DataMember]
            public uint? app_build_number { get; set; }

            [DataMember]
            public string ifv { get; set; }

            [DataMember]
            public string android_id { get; set; }

            [DataMember]
            public string adv_id { get; set; }

            [DataMember]
            public uint? client_kit_version { get; set; }

            [DataMember]
            public List<ReportMessage.RequestParameters.Clid> clids { get; set; }

            [DataMember]
            public string api_key_128 { get; set; }

            [DataMember]
            public string ifa { get; set; }

            [DataMember]
            public ReportMessage.RequestParameters.AppFramework? app_framework { get; set; }

            [DataMember]
            public string windows_aid { get; set; }

            [DataMember]
            public string storage_type { get; set; }

            [DataMember]
            public uint? os_api_level { get; set; }

            [DataMember]
            public ReportMessage.RequestParameters.KitBuildType? kit_build_type { get; set; }

            [DataMember]
            public uint? kit_build_number { get; set; }

            [DataMember]
            public bool? app_debuggable { get; set; }

            public static ReportMessage.RequestParameters Deserialize(Stream stream)
            {
                ReportMessage.RequestParameters instance = new ReportMessage.RequestParameters();
                ReportMessage.RequestParameters.Deserialize(stream, instance);
                return instance;
            }

            public static ReportMessage.RequestParameters DeserializeLengthDelimited(
              Stream stream)
            {
                ReportMessage.RequestParameters instance = new ReportMessage.RequestParameters();
                ReportMessage.RequestParameters.DeserializeLengthDelimited(stream, instance);
                return instance;
            }

            public static ReportMessage.RequestParameters DeserializeLength(
              Stream stream,
              int length)
            {
                ReportMessage.RequestParameters instance = new ReportMessage.RequestParameters();
                ReportMessage.RequestParameters.DeserializeLength(stream, length, instance);
                return instance;
            }

            public static ReportMessage.RequestParameters Deserialize(byte[] buffer)
            {
                ReportMessage.RequestParameters instance = new ReportMessage.RequestParameters();
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    ReportMessage.RequestParameters.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            public static ReportMessage.RequestParameters Deserialize(
              byte[] buffer,
              ReportMessage.RequestParameters instance)
            {
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    ReportMessage.RequestParameters.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            public static ReportMessage.RequestParameters Deserialize(Stream stream, ReportMessage.RequestParameters instance)
            {
                BinaryReader binaryReader = new BinaryReader(stream);
                if (instance.clids == null)
                {
                    instance.clids = new List<ReportMessage.RequestParameters.Clid>();
                }
                instance.app_framework = new ReportMessage.RequestParameters.AppFramework?(ReportMessage.RequestParameters.AppFramework.NATIVE);
                for (; ; )
                {
                    int num = stream.ReadByte();
                    if (num == -1)
                    {
                        return instance;
                    }
                    if (num <= 58)
                    {
                        if (num <= 26)
                        {
                            if (num == 10)
                            {
                                instance.uuid = ProtocolParser.ReadString(stream);
                                continue;
                            }
                            if (num == 18)
                            {
                                instance.device_id = ProtocolParser.ReadString(stream);
                                continue;
                            }
                            if (num == 26)
                            {
                                instance.app_platform = ProtocolParser.ReadString(stream);
                                continue;
                            }
                        }
                        else if (num <= 40)
                        {
                            if (num == 34)
                            {
                                instance.app_version_name = ProtocolParser.ReadString(stream);
                                continue;
                            }
                            if (num == 40)
                            {
                                instance.kit_version = new uint?(ProtocolParser.ReadUInt32(stream));
                                continue;
                            }
                        }
                        else
                        {
                            if (num == 48)
                            {
                                instance.api_key = new uint?(ProtocolParser.ReadUInt32(stream));
                                continue;
                            }
                            if (num == 58)
                            {
                                instance.app_id = ProtocolParser.ReadString(stream);
                                continue;
                            }
                        }
                    }
                    else if (num <= 88)
                    {
                        if (num <= 74)
                        {
                            if (num == 66)
                            {
                                instance.manufacturer = ProtocolParser.ReadString(stream);
                                continue;
                            }
                            if (num == 74)
                            {
                                instance.model = ProtocolParser.ReadString(stream);
                                continue;
                            }
                        }
                        else
                        {
                            if (num == 82)
                            {
                                instance.os_version = ProtocolParser.ReadString(stream);
                                continue;
                            }
                            if (num == 88)
                            {
                                instance.screen_width = new uint?(ProtocolParser.ReadUInt32(stream));
                                continue;
                            }
                        }
                    }
                    else if (num <= 104)
                    {
                        if (num == 96)
                        {
                            instance.screen_height = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        }
                        if (num == 104)
                        {
                            instance.screen_dpi = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        }
                    }
                    else
                    {
                        if (num == 113)
                        {
                            instance.scale_factor = new double?(binaryReader.ReadDouble());
                            continue;
                        }
                        if (num == 122)
                        {
                            instance.locale = ProtocolParser.ReadString(stream);
                            continue;
                        }
                    }
                    Key key = ProtocolParser.ReadKey((byte)num, stream);
                    switch (key.Field)
                    {
                        case 0u:
                            goto IL_2A9;
                        case 16u:
                            if (key.WireType == Wire.Varint)
                            {
                                instance.device_type = new ReportMessage.RequestParameters.DeviceType?((ReportMessage.RequestParameters.DeviceType)ProtocolParser.ReadUInt64(stream));
                                continue;
                            }
                            continue;
                        case 17u:
                            if (key.WireType == Wire.Varint)
                            {
                                instance.is_rooted = new bool?(ProtocolParser.ReadBool(stream));
                                continue;
                            }
                            continue;
                        case 18u:
                            if (key.WireType == Wire.Varint)
                            {
                                instance.app_build_number = new uint?(ProtocolParser.ReadUInt32(stream));
                                continue;
                            }
                            continue;
                        case 19u:
                            if (key.WireType == Wire.LengthDelimited)
                            {
                                instance.ifv = ProtocolParser.ReadString(stream);
                                continue;
                            }
                            continue;
                        case 20u:
                            if (key.WireType == Wire.LengthDelimited)
                            {
                                instance.android_id = ProtocolParser.ReadString(stream);
                                continue;
                            }
                            continue;
                        case 21u:
                            if (key.WireType == Wire.LengthDelimited)
                            {
                                instance.adv_id = ProtocolParser.ReadString(stream);
                                continue;
                            }
                            continue;
                        case 22u:
                            if (key.WireType == Wire.Varint)
                            {
                                instance.client_kit_version = new uint?(ProtocolParser.ReadUInt32(stream));
                                continue;
                            }
                            continue;
                        case 23u:
                            if (key.WireType == Wire.LengthDelimited)
                            {
                                instance.clids.Add(ReportMessage.RequestParameters.Clid.DeserializeLengthDelimited(stream));
                                continue;
                            }
                            continue;
                        case 24u:
                            if (key.WireType == Wire.LengthDelimited)
                            {
                                instance.api_key_128 = ProtocolParser.ReadString(stream);
                                continue;
                            }
                            continue;
                        case 25u:
                            if (key.WireType == Wire.LengthDelimited)
                            {
                                instance.ifa = ProtocolParser.ReadString(stream);
                                continue;
                            }
                            continue;
                        case 26u:
                            if (key.WireType == Wire.Varint)
                            {
                                instance.app_framework = new ReportMessage.RequestParameters.AppFramework?((ReportMessage.RequestParameters.AppFramework)ProtocolParser.ReadUInt64(stream));
                                continue;
                            }
                            continue;
                        case 27u:
                            if (key.WireType == Wire.LengthDelimited)
                            {
                                instance.windows_aid = ProtocolParser.ReadString(stream);
                                continue;
                            }
                            continue;
                        case 28u:
                            if (key.WireType == Wire.LengthDelimited)
                            {
                                instance.storage_type = ProtocolParser.ReadString(stream);
                                continue;
                            }
                            continue;
                        case 29u:
                            if (key.WireType == Wire.Varint)
                            {
                                instance.os_api_level = new uint?(ProtocolParser.ReadUInt32(stream));
                                continue;
                            }
                            continue;
                        case 30u:
                            if (key.WireType == Wire.Varint)
                            {
                                instance.kit_build_type = new ReportMessage.RequestParameters.KitBuildType?((ReportMessage.RequestParameters.KitBuildType)ProtocolParser.ReadUInt64(stream));
                                continue;
                            }
                            continue;
                        case 31u:
                            if (key.WireType == Wire.Varint)
                            {
                                instance.kit_build_number = new uint?(ProtocolParser.ReadUInt32(stream));
                                continue;
                            }
                            continue;
                        case 32u:
                            if (key.WireType == Wire.Varint)
                            {
                                instance.app_debuggable = new bool?(ProtocolParser.ReadBool(stream));
                                continue;
                            }
                            continue;
                    }
                    ProtocolParser.SkipKey(stream, key);
                }
                IL_2A9:
                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
            }

            public static ReportMessage.RequestParameters DeserializeLengthDelimited(
        Stream stream,
        ReportMessage.RequestParameters instance)
            {
                BinaryReader binaryReader = new BinaryReader(stream);
                if (instance.clids == null)
                    instance.clids = new List<ReportMessage.RequestParameters.Clid>();
                instance.app_framework = new ReportMessage.RequestParameters.AppFramework?(ReportMessage.RequestParameters.AppFramework.NATIVE);
                long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                while (stream.Position < num1)
                {
                    int num2 = stream.ReadByte();
                    switch (num2)
                    {
                        case -1:
                            throw new EndOfStreamException();
                        case 10:
                            instance.uuid = ProtocolParser.ReadString(stream);
                            continue;
                        case 18:
                            instance.device_id = ProtocolParser.ReadString(stream);
                            continue;
                        case 26:
                            instance.app_platform = ProtocolParser.ReadString(stream);
                            continue;
                        case 34:
                            instance.app_version_name = ProtocolParser.ReadString(stream);
                            continue;
                        case 40:
                            instance.kit_version = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 48:
                            instance.api_key = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 58:
                            instance.app_id = ProtocolParser.ReadString(stream);
                            continue;
                        case 66:
                            instance.manufacturer = ProtocolParser.ReadString(stream);
                            continue;
                        case 74:
                            instance.model = ProtocolParser.ReadString(stream);
                            continue;
                        case 82:
                            instance.os_version = ProtocolParser.ReadString(stream);
                            continue;
                        case 88:
                            instance.screen_width = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 96:
                            instance.screen_height = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 104:
                            instance.screen_dpi = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 113:
                            instance.scale_factor = new double?(binaryReader.ReadDouble());
                            continue;
                        case 122:
                            instance.locale = ProtocolParser.ReadString(stream);
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num2, stream);
                            switch (key.Field)
                            {
                                case 0:
                                    throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                case 16:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.device_type = new ReportMessage.RequestParameters.DeviceType?((ReportMessage.RequestParameters.DeviceType)ProtocolParser.ReadUInt64(stream));
                                        continue;
                                    }
                                    continue;
                                case 17:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.is_rooted = new bool?(ProtocolParser.ReadBool(stream));
                                        continue;
                                    }
                                    continue;
                                case 18:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.app_build_number = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    }
                                    continue;
                                case 19:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.ifv = ProtocolParser.ReadString(stream);
                                        continue;
                                    }
                                    continue;
                                case 20:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.android_id = ProtocolParser.ReadString(stream);
                                        continue;
                                    }
                                    continue;
                                case 21:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.adv_id = ProtocolParser.ReadString(stream);
                                        continue;
                                    }
                                    continue;
                                case 22:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.client_kit_version = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    }
                                    continue;
                                case 23:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.clids.Add(ReportMessage.RequestParameters.Clid.DeserializeLengthDelimited(stream));
                                        continue;
                                    }
                                    continue;
                                case 24:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.api_key_128 = ProtocolParser.ReadString(stream);
                                        continue;
                                    }
                                    continue;
                                case 25:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.ifa = ProtocolParser.ReadString(stream);
                                        continue;
                                    }
                                    continue;
                                case 26:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.app_framework = new ReportMessage.RequestParameters.AppFramework?((ReportMessage.RequestParameters.AppFramework)ProtocolParser.ReadUInt64(stream));
                                        continue;
                                    }
                                    continue;
                                case 27:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.windows_aid = ProtocolParser.ReadString(stream);
                                        continue;
                                    }
                                    continue;
                                case 28:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.storage_type = ProtocolParser.ReadString(stream);
                                        continue;
                                    }
                                    continue;
                                case 29:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.os_api_level = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    }
                                    continue;
                                case 30:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.kit_build_type = new ReportMessage.RequestParameters.KitBuildType?((ReportMessage.RequestParameters.KitBuildType)ProtocolParser.ReadUInt64(stream));
                                        continue;
                                    }
                                    continue;
                                case 31:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.kit_build_number = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    }
                                    continue;
                                case 32:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.app_debuggable = new bool?(ProtocolParser.ReadBool(stream));
                                        continue;
                                    }
                                    continue;
                                default:
                                    ProtocolParser.SkipKey(stream, key);
                                    continue;
                            }
                    }
                }
                if (stream.Position != num1)
                    throw new ProtocolBufferException("Read past max limit");
                return instance;
            }

            public static ReportMessage.RequestParameters DeserializeLength(
              Stream stream,
              int length,
              ReportMessage.RequestParameters instance)
            {
                BinaryReader binaryReader = new BinaryReader(stream);
                if (instance.clids == null)
                    instance.clids = new List<ReportMessage.RequestParameters.Clid>();
                instance.app_framework = new ReportMessage.RequestParameters.AppFramework?(ReportMessage.RequestParameters.AppFramework.NATIVE);
                long num1 = stream.Position + (long)length;
                while (stream.Position < num1)
                {
                    int num2 = stream.ReadByte();
                    switch (num2)
                    {
                        case -1:
                            throw new EndOfStreamException();
                        case 10:
                            instance.uuid = ProtocolParser.ReadString(stream);
                            continue;
                        case 18:
                            instance.device_id = ProtocolParser.ReadString(stream);
                            continue;
                        case 26:
                            instance.app_platform = ProtocolParser.ReadString(stream);
                            continue;
                        case 34:
                            instance.app_version_name = ProtocolParser.ReadString(stream);
                            continue;
                        case 40:
                            instance.kit_version = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 48:
                            instance.api_key = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 58:
                            instance.app_id = ProtocolParser.ReadString(stream);
                            continue;
                        case 66:
                            instance.manufacturer = ProtocolParser.ReadString(stream);
                            continue;
                        case 74:
                            instance.model = ProtocolParser.ReadString(stream);
                            continue;
                        case 82:
                            instance.os_version = ProtocolParser.ReadString(stream);
                            continue;
                        case 88:
                            instance.screen_width = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 96:
                            instance.screen_height = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 104:
                            instance.screen_dpi = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 113:
                            instance.scale_factor = new double?(binaryReader.ReadDouble());
                            continue;
                        case 122:
                            instance.locale = ProtocolParser.ReadString(stream);
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num2, stream);
                            switch (key.Field)
                            {
                                case 0:
                                    throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                case 16:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.device_type = new ReportMessage.RequestParameters.DeviceType?((ReportMessage.RequestParameters.DeviceType)ProtocolParser.ReadUInt64(stream));
                                        continue;
                                    }
                                    continue;
                                case 17:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.is_rooted = new bool?(ProtocolParser.ReadBool(stream));
                                        continue;
                                    }
                                    continue;
                                case 18:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.app_build_number = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    }
                                    continue;
                                case 19:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.ifv = ProtocolParser.ReadString(stream);
                                        continue;
                                    }
                                    continue;
                                case 20:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.android_id = ProtocolParser.ReadString(stream);
                                        continue;
                                    }
                                    continue;
                                case 21:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.adv_id = ProtocolParser.ReadString(stream);
                                        continue;
                                    }
                                    continue;
                                case 22:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.client_kit_version = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    }
                                    continue;
                                case 23:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.clids.Add(ReportMessage.RequestParameters.Clid.DeserializeLengthDelimited(stream));
                                        continue;
                                    }
                                    continue;
                                case 24:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.api_key_128 = ProtocolParser.ReadString(stream);
                                        continue;
                                    }
                                    continue;
                                case 25:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.ifa = ProtocolParser.ReadString(stream);
                                        continue;
                                    }
                                    continue;
                                case 26:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.app_framework = new ReportMessage.RequestParameters.AppFramework?((ReportMessage.RequestParameters.AppFramework)ProtocolParser.ReadUInt64(stream));
                                        continue;
                                    }
                                    continue;
                                case 27:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.windows_aid = ProtocolParser.ReadString(stream);
                                        continue;
                                    }
                                    continue;
                                case 28:
                                    if (key.WireType == Wire.LengthDelimited)
                                    {
                                        instance.storage_type = ProtocolParser.ReadString(stream);
                                        continue;
                                    }
                                    continue;
                                case 29:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.os_api_level = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    }
                                    continue;
                                case 30:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.kit_build_type = new ReportMessage.RequestParameters.KitBuildType?((ReportMessage.RequestParameters.KitBuildType)ProtocolParser.ReadUInt64(stream));
                                        continue;
                                    }
                                    continue;
                                case 31:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.kit_build_number = new uint?(ProtocolParser.ReadUInt32(stream));
                                        continue;
                                    }
                                    continue;
                                case 32:
                                    if (key.WireType == Wire.Varint)
                                    {
                                        instance.app_debuggable = new bool?(ProtocolParser.ReadBool(stream));
                                        continue;
                                    }
                                    continue;
                                default:
                                    ProtocolParser.SkipKey(stream, key);
                                    continue;
                            }
                    }
                }
                if (stream.Position != num1)
                    throw new ProtocolBufferException("Read past max limit");
                return instance;
            }

            public static void Serialize(Stream stream, ReportMessage.RequestParameters instance)
            {
                BinaryWriter binaryWriter = new BinaryWriter(stream);
                MemoryStream stream1 = ProtocolParser.Stack.Pop();
                if (instance.uuid != null)
                {
                    stream.WriteByte((byte)10);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.uuid));
                }
                if (instance.device_id != null)
                {
                    stream.WriteByte((byte)18);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.device_id));
                }
                if (instance.app_platform != null)
                {
                    stream.WriteByte((byte)26);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.app_platform));
                }
                if (instance.app_version_name != null)
                {
                    stream.WriteByte((byte)34);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.app_version_name));
                }
                if (instance.kit_version.HasValue)
                {
                    stream.WriteByte((byte)40);
                    ProtocolParser.WriteUInt32(stream, instance.kit_version.Value);
                }
                if (instance.api_key.HasValue)
                {
                    stream.WriteByte((byte)48);
                    ProtocolParser.WriteUInt32(stream, instance.api_key.Value);
                }
                if (instance.app_id != null)
                {
                    stream.WriteByte((byte)58);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.app_id));
                }
                if (instance.manufacturer != null)
                {
                    stream.WriteByte((byte)66);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.manufacturer));
                }
                if (instance.model != null)
                {
                    stream.WriteByte((byte)74);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.model));
                }
                if (instance.os_version != null)
                {
                    stream.WriteByte((byte)82);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.os_version));
                }
                if (instance.screen_width.HasValue)
                {
                    stream.WriteByte((byte)88);
                    ProtocolParser.WriteUInt32(stream, instance.screen_width.Value);
                }
                if (instance.screen_height.HasValue)
                {
                    stream.WriteByte((byte)96);
                    ProtocolParser.WriteUInt32(stream, instance.screen_height.Value);
                }
                if (instance.screen_dpi.HasValue)
                {
                    stream.WriteByte((byte)104);
                    ProtocolParser.WriteUInt32(stream, instance.screen_dpi.Value);
                }
                if (instance.scale_factor.HasValue)
                {
                    stream.WriteByte((byte)113);
                    binaryWriter.Write(instance.scale_factor.Value);
                }
                if (instance.locale != null)
                {
                    stream.WriteByte((byte)122);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.locale));
                }
                if (instance.device_type.HasValue)
                {
                    stream.WriteByte((byte)128);
                    stream.WriteByte((byte)1);
                    ProtocolParser.WriteUInt64(stream, (ulong)instance.device_type.Value);
                }
                if (instance.is_rooted.HasValue)
                {
                    stream.WriteByte((byte)136);
                    stream.WriteByte((byte)1);
                    ProtocolParser.WriteBool(stream, instance.is_rooted.Value);
                }
                if (instance.app_build_number.HasValue)
                {
                    stream.WriteByte((byte)144);
                    stream.WriteByte((byte)1);
                    ProtocolParser.WriteUInt32(stream, instance.app_build_number.Value);
                }
                if (instance.ifv != null)
                {
                    stream.WriteByte((byte)154);
                    stream.WriteByte((byte)1);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.ifv));
                }
                if (instance.android_id != null)
                {
                    stream.WriteByte((byte)162);
                    stream.WriteByte((byte)1);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.android_id));
                }
                if (instance.adv_id != null)
                {
                    stream.WriteByte((byte)170);
                    stream.WriteByte((byte)1);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.adv_id));
                }
                if (instance.client_kit_version.HasValue)
                {
                    stream.WriteByte((byte)176);
                    stream.WriteByte((byte)1);
                    ProtocolParser.WriteUInt32(stream, instance.client_kit_version.Value);
                }
                if (instance.clids != null)
                {
                    foreach (ReportMessage.RequestParameters.Clid clid in instance.clids)
                    {
                        stream.WriteByte((byte)186);
                        stream.WriteByte((byte)1);
                        stream1.SetLength(0L);
                        ReportMessage.RequestParameters.Clid.Serialize((Stream)stream1, clid);
                        uint length = (uint)stream1.Length;
                        ProtocolParser.WriteUInt32(stream, length);
                        stream1.WriteTo(stream);
                    }
                }
                if (instance.api_key_128 != null)
                {
                    stream.WriteByte((byte)194);
                    stream.WriteByte((byte)1);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.api_key_128));
                }
                if (instance.ifa != null)
                {
                    stream.WriteByte((byte)202);
                    stream.WriteByte((byte)1);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.ifa));
                }
                if (instance.app_framework.HasValue)
                {
                    stream.WriteByte((byte)208);
                    stream.WriteByte((byte)1);
                    ProtocolParser.WriteUInt64(stream, (ulong)instance.app_framework.Value);
                }
                if (instance.windows_aid != null)
                {
                    stream.WriteByte((byte)218);
                    stream.WriteByte((byte)1);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.windows_aid));
                }
                if (instance.storage_type != null)
                {
                    stream.WriteByte((byte)226);
                    stream.WriteByte((byte)1);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.storage_type));
                }
                if (instance.os_api_level.HasValue)
                {
                    stream.WriteByte((byte)232);
                    stream.WriteByte((byte)1);
                    ProtocolParser.WriteUInt32(stream, instance.os_api_level.Value);
                }
                if (instance.kit_build_type.HasValue)
                {
                    stream.WriteByte((byte)240);
                    stream.WriteByte((byte)1);
                    ProtocolParser.WriteUInt64(stream, (ulong)instance.kit_build_type.Value);
                }
                if (instance.kit_build_number.HasValue)
                {
                    stream.WriteByte((byte)248);
                    stream.WriteByte((byte)1);
                    ProtocolParser.WriteUInt32(stream, instance.kit_build_number.Value);
                }
                if (instance.app_debuggable.HasValue)
                {
                    stream.WriteByte((byte)128);
                    stream.WriteByte((byte)2);
                    ProtocolParser.WriteBool(stream, instance.app_debuggable.Value);
                }
                ProtocolParser.Stack.Push(stream1);
            }

            public static byte[] SerializeToBytes(ReportMessage.RequestParameters instance)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ReportMessage.RequestParameters.Serialize((Stream)memoryStream, instance);
                    return memoryStream.ToArray();
                }
            }

            public static void SerializeLengthDelimited(
              Stream stream,
              ReportMessage.RequestParameters instance)
            {
                byte[] bytes = ReportMessage.RequestParameters.SerializeToBytes(instance);
                ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                stream.Write(bytes, 0, bytes.Length);
            }

            public enum DeviceType
            {
                PHONE = 1,
                TABLET = 2,
                PHABLET = 3,
                TV = 4,
                DESKTOP = 5,
            }

            public enum AppFramework
            {
                NATIVE,
                UNITY,
                XAMARIN,
                REACT,
                CORDOVA,
            }

            public enum KitBuildType
            {
                UNDEFINED = 0,
                NET_DESKTOP = 1,
                WINRT8 = 2,
                WINRT10 = 3,
                WP8S = 4,
                WP81 = 5,
                UWP10 = 6,
                WP7S = 7,
                SOURCE = 100, // 0x00000064
                STATIC = 101, // 0x00000065
                DYNAMIC = 102, // 0x00000066
                PUBLIC = 200, // 0x000000C8
                PUBLIC_SNAPSHOT = 201, // 0x000000C9
                INTERNAL = 202, // 0x000000CA
                INTERNAL_SNAPSHOT = 203, // 0x000000CB
                LIMITED = 204, // 0x000000CC
                LIMITED_SNAPSHOT = 205, // 0x000000CD
            }

            [DataContract]
            public class Clid
            {
                [DataMember]
                public string name { get; set; }

                [DataMember]
                public ulong value { get; set; }

                public static ReportMessage.RequestParameters.Clid Deserialize(Stream stream)
                {
                    ReportMessage.RequestParameters.Clid instance = new ReportMessage.RequestParameters.Clid();
                    ReportMessage.RequestParameters.Clid.Deserialize(stream, instance);
                    return instance;
                }

                public static ReportMessage.RequestParameters.Clid DeserializeLengthDelimited(
                  Stream stream)
                {
                    ReportMessage.RequestParameters.Clid instance = new ReportMessage.RequestParameters.Clid();
                    ReportMessage.RequestParameters.Clid.DeserializeLengthDelimited(stream, instance);
                    return instance;
                }

                public static ReportMessage.RequestParameters.Clid DeserializeLength(
                  Stream stream,
                  int length)
                {
                    ReportMessage.RequestParameters.Clid instance = new ReportMessage.RequestParameters.Clid();
                    ReportMessage.RequestParameters.Clid.DeserializeLength(stream, length, instance);
                    return instance;
                }

                public static ReportMessage.RequestParameters.Clid Deserialize(byte[] buffer)
                {
                    ReportMessage.RequestParameters.Clid instance = new ReportMessage.RequestParameters.Clid();
                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                        ReportMessage.RequestParameters.Clid.Deserialize((Stream)memoryStream, instance);
                    return instance;
                }

                public static ReportMessage.RequestParameters.Clid Deserialize(
                  byte[] buffer,
                  ReportMessage.RequestParameters.Clid instance)
                {
                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                        ReportMessage.RequestParameters.Clid.Deserialize((Stream)memoryStream, instance);
                    return instance;
                }

                public static ReportMessage.RequestParameters.Clid Deserialize(
                  Stream stream,
                  ReportMessage.RequestParameters.Clid instance)
                {
                    while (true)
                    {
                        int num = stream.ReadByte();
                        switch (num)
                        {
                            case -1:
                                goto label_6;
                            case 10:
                                instance.name = ProtocolParser.ReadString(stream);
                                continue;
                            case 16:
                                instance.value = ProtocolParser.ReadUInt64(stream);
                                continue;
                            default:
                                Key key = ProtocolParser.ReadKey((byte)num, stream);
                                if (key.Field != 0U)
                                {
                                    ProtocolParser.SkipKey(stream, key);
                                    continue;
                                }
                                goto label_4;
                        }
                    }
                    label_4:
                    throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                    label_6:
                    return instance;
                }

                public static ReportMessage.RequestParameters.Clid DeserializeLengthDelimited(
                  Stream stream,
                  ReportMessage.RequestParameters.Clid instance)
                {
                    long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                    while (stream.Position < num1)
                    {
                        int num2 = stream.ReadByte();
                        switch (num2)
                        {
                            case -1:
                                throw new EndOfStreamException();
                            case 10:
                                instance.name = ProtocolParser.ReadString(stream);
                                continue;
                            case 16:
                                instance.value = ProtocolParser.ReadUInt64(stream);
                                continue;
                            default:
                                Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                if (key.Field == 0U)
                                    throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                ProtocolParser.SkipKey(stream, key);
                                continue;
                        }
                    }
                    if (stream.Position != num1)
                        throw new ProtocolBufferException("Read past max limit");
                    return instance;
                }

                public static ReportMessage.RequestParameters.Clid DeserializeLength(
                  Stream stream,
                  int length,
                  ReportMessage.RequestParameters.Clid instance)
                {
                    long num1 = stream.Position + (long)length;
                    while (stream.Position < num1)
                    {
                        int num2 = stream.ReadByte();
                        switch (num2)
                        {
                            case -1:
                                throw new EndOfStreamException();
                            case 10:
                                instance.name = ProtocolParser.ReadString(stream);
                                continue;
                            case 16:
                                instance.value = ProtocolParser.ReadUInt64(stream);
                                continue;
                            default:
                                Key key = ProtocolParser.ReadKey((byte)num2, stream);
                                if (key.Field == 0U)
                                    throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                                ProtocolParser.SkipKey(stream, key);
                                continue;
                        }
                    }
                    if (stream.Position != num1)
                        throw new ProtocolBufferException("Read past max limit");
                    return instance;
                }

                public static void Serialize(Stream stream, ReportMessage.RequestParameters.Clid instance)
                {
                    MemoryStream stream1 = ProtocolParser.Stack.Pop();
                    if (instance.name == null)
                        throw new ProtocolBufferException("name is required by the proto specification.");
                    stream.WriteByte((byte)10);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.name));
                    stream.WriteByte((byte)16);
                    ProtocolParser.WriteUInt64(stream, instance.value);
                    ProtocolParser.Stack.Push(stream1);
                }

                public static byte[] SerializeToBytes(ReportMessage.RequestParameters.Clid instance)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ReportMessage.RequestParameters.Clid.Serialize((Stream)memoryStream, instance);
                        return memoryStream.ToArray();
                    }
                }

                public static void SerializeLengthDelimited(
                  Stream stream,
                  ReportMessage.RequestParameters.Clid instance)
                {
                    byte[] bytes = ReportMessage.RequestParameters.Clid.SerializeToBytes(instance);
                    ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        [DataContract]
        public class EnvironmentVariable
        {
            [DataMember]
            public string name { get; set; }

            [DataMember]
            public string value { get; set; }

            public static ReportMessage.EnvironmentVariable Deserialize(Stream stream)
            {
                ReportMessage.EnvironmentVariable instance = new ReportMessage.EnvironmentVariable();
                ReportMessage.EnvironmentVariable.Deserialize(stream, instance);
                return instance;
            }

            public static ReportMessage.EnvironmentVariable DeserializeLengthDelimited(
              Stream stream)
            {
                ReportMessage.EnvironmentVariable instance = new ReportMessage.EnvironmentVariable();
                ReportMessage.EnvironmentVariable.DeserializeLengthDelimited(stream, instance);
                return instance;
            }

            public static ReportMessage.EnvironmentVariable DeserializeLength(
              Stream stream,
              int length)
            {
                ReportMessage.EnvironmentVariable instance = new ReportMessage.EnvironmentVariable();
                ReportMessage.EnvironmentVariable.DeserializeLength(stream, length, instance);
                return instance;
            }

            public static ReportMessage.EnvironmentVariable Deserialize(byte[] buffer)
            {
                ReportMessage.EnvironmentVariable instance = new ReportMessage.EnvironmentVariable();
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    ReportMessage.EnvironmentVariable.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            public static ReportMessage.EnvironmentVariable Deserialize(
              byte[] buffer,
              ReportMessage.EnvironmentVariable instance)
            {
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    ReportMessage.EnvironmentVariable.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            public static ReportMessage.EnvironmentVariable Deserialize(
              Stream stream,
              ReportMessage.EnvironmentVariable instance)
            {
                while (true)
                {
                    int num = stream.ReadByte();
                    switch (num)
                    {
                        case -1:
                            goto label_6;
                        case 10:
                            instance.name = ProtocolParser.ReadString(stream);
                            continue;
                        case 18:
                            instance.value = ProtocolParser.ReadString(stream);
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num, stream);
                            if (key.Field != 0U)
                            {
                                ProtocolParser.SkipKey(stream, key);
                                continue;
                            }
                            goto label_4;
                    }
                }
                label_4:
                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                label_6:
                return instance;
            }

            public static ReportMessage.EnvironmentVariable DeserializeLengthDelimited(
              Stream stream,
              ReportMessage.EnvironmentVariable instance)
            {
                long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                while (stream.Position < num1)
                {
                    int num2 = stream.ReadByte();
                    switch (num2)
                    {
                        case -1:
                            throw new EndOfStreamException();
                        case 10:
                            instance.name = ProtocolParser.ReadString(stream);
                            continue;
                        case 18:
                            instance.value = ProtocolParser.ReadString(stream);
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num2, stream);
                            if (key.Field == 0U)
                                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            ProtocolParser.SkipKey(stream, key);
                            continue;
                    }
                }
                if (stream.Position != num1)
                    throw new ProtocolBufferException("Read past max limit");
                return instance;
            }

            public static ReportMessage.EnvironmentVariable DeserializeLength(
              Stream stream,
              int length,
              ReportMessage.EnvironmentVariable instance)
            {
                long num1 = stream.Position + (long)length;
                while (stream.Position < num1)
                {
                    int num2 = stream.ReadByte();
                    switch (num2)
                    {
                        case -1:
                            throw new EndOfStreamException();
                        case 10:
                            instance.name = ProtocolParser.ReadString(stream);
                            continue;
                        case 18:
                            instance.value = ProtocolParser.ReadString(stream);
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num2, stream);
                            if (key.Field == 0U)
                                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            ProtocolParser.SkipKey(stream, key);
                            continue;
                    }
                }
                if (stream.Position != num1)
                    throw new ProtocolBufferException("Read past max limit");
                return instance;
            }

            public static void Serialize(Stream stream, ReportMessage.EnvironmentVariable instance)
            {
                MemoryStream stream1 = ProtocolParser.Stack.Pop();
                if (instance.name == null)
                    throw new ProtocolBufferException("name is required by the proto specification.");
                stream.WriteByte((byte)10);
                ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.name));
                if (instance.value == null)
                    throw new ProtocolBufferException("value is required by the proto specification.");
                stream.WriteByte((byte)18);
                ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.value));
                ProtocolParser.Stack.Push(stream1);
            }

            public static byte[] SerializeToBytes(ReportMessage.EnvironmentVariable instance)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ReportMessage.EnvironmentVariable.Serialize((Stream)memoryStream, instance);
                    return memoryStream.ToArray();
                }
            }

            public static void SerializeLengthDelimited(
              Stream stream,
              ReportMessage.EnvironmentVariable instance)
            {
                byte[] bytes = ReportMessage.EnvironmentVariable.SerializeToBytes(instance);
                ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        [DataContract]
        public class NetworkInterface
        {
            [DataMember]
            public string name { get; set; }

            [DataMember]
            public string mac { get; set; }

            public static ReportMessage.NetworkInterface Deserialize(Stream stream)
            {
                ReportMessage.NetworkInterface instance = new ReportMessage.NetworkInterface();
                ReportMessage.NetworkInterface.Deserialize(stream, instance);
                return instance;
            }

            public static ReportMessage.NetworkInterface DeserializeLengthDelimited(
              Stream stream)
            {
                ReportMessage.NetworkInterface instance = new ReportMessage.NetworkInterface();
                ReportMessage.NetworkInterface.DeserializeLengthDelimited(stream, instance);
                return instance;
            }

            public static ReportMessage.NetworkInterface DeserializeLength(
              Stream stream,
              int length)
            {
                ReportMessage.NetworkInterface instance = new ReportMessage.NetworkInterface();
                ReportMessage.NetworkInterface.DeserializeLength(stream, length, instance);
                return instance;
            }

            public static ReportMessage.NetworkInterface Deserialize(byte[] buffer)
            {
                ReportMessage.NetworkInterface instance = new ReportMessage.NetworkInterface();
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    ReportMessage.NetworkInterface.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            public static ReportMessage.NetworkInterface Deserialize(
              byte[] buffer,
              ReportMessage.NetworkInterface instance)
            {
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    ReportMessage.NetworkInterface.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            public static ReportMessage.NetworkInterface Deserialize(
              Stream stream,
              ReportMessage.NetworkInterface instance)
            {
                while (true)
                {
                    int num = stream.ReadByte();
                    switch (num)
                    {
                        case -1:
                            goto label_6;
                        case 10:
                            instance.name = ProtocolParser.ReadString(stream);
                            continue;
                        case 18:
                            instance.mac = ProtocolParser.ReadString(stream);
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num, stream);
                            if (key.Field != 0U)
                            {
                                ProtocolParser.SkipKey(stream, key);
                                continue;
                            }
                            goto label_4;
                    }
                }
                label_4:
                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                label_6:
                return instance;
            }

            public static ReportMessage.NetworkInterface DeserializeLengthDelimited(
              Stream stream,
              ReportMessage.NetworkInterface instance)
            {
                long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                while (stream.Position < num1)
                {
                    int num2 = stream.ReadByte();
                    switch (num2)
                    {
                        case -1:
                            throw new EndOfStreamException();
                        case 10:
                            instance.name = ProtocolParser.ReadString(stream);
                            continue;
                        case 18:
                            instance.mac = ProtocolParser.ReadString(stream);
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num2, stream);
                            if (key.Field == 0U)
                                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            ProtocolParser.SkipKey(stream, key);
                            continue;
                    }
                }
                if (stream.Position != num1)
                    throw new ProtocolBufferException("Read past max limit");
                return instance;
            }

            public static ReportMessage.NetworkInterface DeserializeLength(
              Stream stream,
              int length,
              ReportMessage.NetworkInterface instance)
            {
                long num1 = stream.Position + (long)length;
                while (stream.Position < num1)
                {
                    int num2 = stream.ReadByte();
                    switch (num2)
                    {
                        case -1:
                            throw new EndOfStreamException();
                        case 10:
                            instance.name = ProtocolParser.ReadString(stream);
                            continue;
                        case 18:
                            instance.mac = ProtocolParser.ReadString(stream);
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num2, stream);
                            if (key.Field == 0U)
                                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            ProtocolParser.SkipKey(stream, key);
                            continue;
                    }
                }
                if (stream.Position != num1)
                    throw new ProtocolBufferException("Read past max limit");
                return instance;
            }

            public static void Serialize(Stream stream, ReportMessage.NetworkInterface instance)
            {
                MemoryStream stream1 = ProtocolParser.Stack.Pop();
                if (instance.name == null)
                    throw new ProtocolBufferException("name is required by the proto specification.");
                stream.WriteByte((byte)10);
                ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.name));
                if (instance.mac == null)
                    throw new ProtocolBufferException("mac is required by the proto specification.");
                stream.WriteByte((byte)18);
                ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.mac));
                ProtocolParser.Stack.Push(stream1);
            }

            public static byte[] SerializeToBytes(ReportMessage.NetworkInterface instance)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ReportMessage.NetworkInterface.Serialize((Stream)memoryStream, instance);
                    return memoryStream.ToArray();
                }
            }

            public static void SerializeLengthDelimited(
              Stream stream,
              ReportMessage.NetworkInterface instance)
            {
                byte[] bytes = ReportMessage.NetworkInterface.SerializeToBytes(instance);
                ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        [DataContract]
        public class SimInfo
        {
            [DataMember]
            public uint? country_code { get; set; }

            [DataMember]
            public uint? operator_id { get; set; }

            [DataMember]
            public string operator_name { get; set; }

            [DataMember]
            public bool? data_roaming { get; set; }

            [DataMember]
            public string icc_id { get; set; }

            public static ReportMessage.SimInfo Deserialize(Stream stream)
            {
                ReportMessage.SimInfo instance = new ReportMessage.SimInfo();
                ReportMessage.SimInfo.Deserialize(stream, instance);
                return instance;
            }

            public static ReportMessage.SimInfo DeserializeLengthDelimited(Stream stream)
            {
                ReportMessage.SimInfo instance = new ReportMessage.SimInfo();
                ReportMessage.SimInfo.DeserializeLengthDelimited(stream, instance);
                return instance;
            }

            public static ReportMessage.SimInfo DeserializeLength(Stream stream, int length)
            {
                ReportMessage.SimInfo instance = new ReportMessage.SimInfo();
                ReportMessage.SimInfo.DeserializeLength(stream, length, instance);
                return instance;
            }

            public static ReportMessage.SimInfo Deserialize(byte[] buffer)
            {
                ReportMessage.SimInfo instance = new ReportMessage.SimInfo();
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    ReportMessage.SimInfo.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            public static ReportMessage.SimInfo Deserialize(
              byte[] buffer,
              ReportMessage.SimInfo instance)
            {
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    ReportMessage.SimInfo.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            public static ReportMessage.SimInfo Deserialize(
              Stream stream,
              ReportMessage.SimInfo instance)
            {
                instance.data_roaming = new bool?(false);
                while (true)
                {
                    int num = stream.ReadByte();
                    switch (num)
                    {
                        case -1:
                            goto label_10;
                        case 8:
                            instance.country_code = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 16:
                            instance.operator_id = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 26:
                            instance.operator_name = ProtocolParser.ReadString(stream);
                            continue;
                        case 32:
                            instance.data_roaming = new bool?(ProtocolParser.ReadBool(stream));
                            continue;
                        case 42:
                            instance.icc_id = ProtocolParser.ReadString(stream);
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num, stream);
                            if (key.Field != 0U)
                            {
                                ProtocolParser.SkipKey(stream, key);
                                continue;
                            }
                            goto label_8;
                    }
                }
                label_8:
                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                label_10:
                return instance;
            }

            public static ReportMessage.SimInfo DeserializeLengthDelimited(
              Stream stream,
              ReportMessage.SimInfo instance)
            {
                instance.data_roaming = new bool?(false);
                long num1 = (long)ProtocolParser.ReadUInt32(stream) + stream.Position;
                while (stream.Position < num1)
                {
                    int num2 = stream.ReadByte();
                    switch (num2)
                    {
                        case -1:
                            throw new EndOfStreamException();
                        case 8:
                            instance.country_code = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 16:
                            instance.operator_id = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 26:
                            instance.operator_name = ProtocolParser.ReadString(stream);
                            continue;
                        case 32:
                            instance.data_roaming = new bool?(ProtocolParser.ReadBool(stream));
                            continue;
                        case 42:
                            instance.icc_id = ProtocolParser.ReadString(stream);
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num2, stream);
                            if (key.Field == 0U)
                                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            ProtocolParser.SkipKey(stream, key);
                            continue;
                    }
                }
                if (stream.Position != num1)
                    throw new ProtocolBufferException("Read past max limit");
                return instance;
            }

            public static ReportMessage.SimInfo DeserializeLength(
              Stream stream,
              int length,
              ReportMessage.SimInfo instance)
            {
                instance.data_roaming = new bool?(false);
                long num1 = stream.Position + (long)length;
                while (stream.Position < num1)
                {
                    int num2 = stream.ReadByte();
                    switch (num2)
                    {
                        case -1:
                            throw new EndOfStreamException();
                        case 8:
                            instance.country_code = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 16:
                            instance.operator_id = new uint?(ProtocolParser.ReadUInt32(stream));
                            continue;
                        case 26:
                            instance.operator_name = ProtocolParser.ReadString(stream);
                            continue;
                        case 32:
                            instance.data_roaming = new bool?(ProtocolParser.ReadBool(stream));
                            continue;
                        case 42:
                            instance.icc_id = ProtocolParser.ReadString(stream);
                            continue;
                        default:
                            Key key = ProtocolParser.ReadKey((byte)num2, stream);
                            if (key.Field == 0U)
                                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            ProtocolParser.SkipKey(stream, key);
                            continue;
                    }
                }
                if (stream.Position != num1)
                    throw new ProtocolBufferException("Read past max limit");
                return instance;
            }

            public static void Serialize(Stream stream, ReportMessage.SimInfo instance)
            {
                MemoryStream stream1 = ProtocolParser.Stack.Pop();
                if (instance.country_code.HasValue)
                {
                    stream.WriteByte((byte)8);
                    ProtocolParser.WriteUInt32(stream, instance.country_code.Value);
                }
                if (instance.operator_id.HasValue)
                {
                    stream.WriteByte((byte)16);
                    ProtocolParser.WriteUInt32(stream, instance.operator_id.Value);
                }
                if (instance.operator_name != null)
                {
                    stream.WriteByte((byte)26);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.operator_name));
                }
                if (instance.data_roaming.HasValue)
                {
                    stream.WriteByte((byte)32);
                    ProtocolParser.WriteBool(stream, instance.data_roaming.Value);
                }
                if (instance.icc_id != null)
                {
                    stream.WriteByte((byte)42);
                    ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.icc_id));
                }
                ProtocolParser.Stack.Push(stream1);
            }

            public static byte[] SerializeToBytes(ReportMessage.SimInfo instance)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ReportMessage.SimInfo.Serialize((Stream)memoryStream, instance);
                    return memoryStream.ToArray();
                }
            }

            public static void SerializeLengthDelimited(Stream stream, ReportMessage.SimInfo instance)
            {
                byte[] bytes = ReportMessage.SimInfo.SerializeToBytes(instance);
                ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                stream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
