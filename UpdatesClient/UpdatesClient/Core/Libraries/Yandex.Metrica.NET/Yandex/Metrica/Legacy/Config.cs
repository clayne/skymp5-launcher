using SilentOrbit.ProtocolBuffers;
using System.IO;
using System.Text;

namespace Yandex.Metrica.Legacy
{
    internal class Config
    {
        public string UUID { get; set; }

        public ulong LastStartupTime { get; set; }

        public ulong LastStopTime { get; set; }

        public ulong CurrentSessionId { get; set; }

        public ulong CurrentSessionEventCounter { get; set; }

        public bool Init { get; set; }

        public string ApiKey { get; set; }

        public ulong DispatchPeriodMilliseconds { get; set; }

        public uint MaxReportsCount { get; set; }

        public bool TrackLocationEnabled { get; set; }

        public bool ReportCrashesEnabled { get; set; }

        public string CustomStartupUrl { get; set; }

        public string CustomAppVersion { get; set; }

        public Config.Location CustomLocation { get; set; }

        public string ReportUrl { get; set; }

        public string CheckUpdatesUrl { get; set; }

        public bool Suspended { get; set; }

        public ulong BackgroundSessionEventCounter { get; set; }

        public ulong SessionInactivityTimeoutMilliseconds { get; set; }

        public ulong LastIdentityEventMilliseconds { get; set; }

        public long ServerTimeOffset { get; set; }

        internal static Config Deserialize(Stream stream)
        {
            Config instance = new Config();
            Config.Deserialize(stream, instance);
            return instance;
        }

        internal static Config DeserializeLengthDelimited(Stream stream)
        {
            Config instance = new Config();
            Config.DeserializeLengthDelimited(stream, instance);
            return instance;
        }

        internal static Config DeserializeLength(Stream stream, int length)
        {
            Config instance = new Config();
            Config.DeserializeLength(stream, length, instance);
            return instance;
        }

        internal static Config Deserialize(byte[] buffer)
        {
            Config instance = new Config();
            using (MemoryStream memoryStream = new MemoryStream(buffer))
                Config.Deserialize((Stream)memoryStream, instance);
            return instance;
        }

        internal static Config Deserialize(byte[] buffer, Config instance)
        {
            using (MemoryStream memoryStream = new MemoryStream(buffer))
                Config.Deserialize((Stream)memoryStream, instance);
            return instance;
        }

        internal static Config Deserialize(Stream stream, Config instance)
        {
            for (; ; )
            {
                int num = stream.ReadByte();
                if (num == -1)
                {
                    return instance;
                }
                if (num <= 58)
                {
                    if (num <= 24)
                    {
                        if (num == 10)
                        {
                            instance.UUID = ProtocolParser.ReadString(stream);
                            continue;
                        }
                        if (num == 16)
                        {
                            instance.LastStartupTime = ProtocolParser.ReadUInt64(stream);
                            continue;
                        }
                        if (num == 24)
                        {
                            instance.LastStopTime = ProtocolParser.ReadUInt64(stream);
                            continue;
                        }
                    }
                    else if (num <= 40)
                    {
                        if (num == 32)
                        {
                            instance.CurrentSessionId = ProtocolParser.ReadUInt64(stream);
                            continue;
                        }
                        if (num == 40)
                        {
                            instance.CurrentSessionEventCounter = ProtocolParser.ReadUInt64(stream);
                            continue;
                        }
                    }
                    else
                    {
                        if (num == 48)
                        {
                            instance.Init = ProtocolParser.ReadBool(stream);
                            continue;
                        }
                        if (num == 58)
                        {
                            instance.ApiKey = ProtocolParser.ReadString(stream);
                            continue;
                        }
                    }
                }
                else if (num <= 88)
                {
                    if (num <= 72)
                    {
                        if (num == 64)
                        {
                            instance.DispatchPeriodMilliseconds = ProtocolParser.ReadUInt64(stream);
                            continue;
                        }
                        if (num == 72)
                        {
                            instance.MaxReportsCount = ProtocolParser.ReadUInt32(stream);
                            continue;
                        }
                    }
                    else
                    {
                        if (num == 80)
                        {
                            instance.TrackLocationEnabled = ProtocolParser.ReadBool(stream);
                            continue;
                        }
                        if (num == 88)
                        {
                            instance.ReportCrashesEnabled = ProtocolParser.ReadBool(stream);
                            continue;
                        }
                    }
                }
                else if (num <= 106)
                {
                    if (num == 98)
                    {
                        instance.CustomStartupUrl = ProtocolParser.ReadString(stream);
                        continue;
                    }
                    if (num == 106)
                    {
                        instance.CustomAppVersion = ProtocolParser.ReadString(stream);
                        continue;
                    }
                }
                else if (num != 114)
                {
                    if (num == 122)
                    {
                        instance.ReportUrl = ProtocolParser.ReadString(stream);
                        continue;
                    }
                }
                else
                {
                    if (instance.CustomLocation == null)
                    {
                        instance.CustomLocation = Config.Location.DeserializeLengthDelimited(stream);
                        continue;
                    }
                    Config.Location.DeserializeLengthDelimited(stream, instance.CustomLocation);
                    continue;
                }
                Key key = ProtocolParser.ReadKey((byte)num, stream);
                uint field = key.Field;
                if (field == 0u)
                {
                    break;
                }
                switch (field)
                {
                    case 16u:
                        if (key.WireType == Wire.LengthDelimited)
                        {
                            instance.CheckUpdatesUrl = ProtocolParser.ReadString(stream);
                        }
                        break;
                    case 17u:
                        if (key.WireType == Wire.Varint)
                        {
                            instance.Suspended = ProtocolParser.ReadBool(stream);
                        }
                        break;
                    case 18u:
                        if (key.WireType == Wire.Varint)
                        {
                            instance.BackgroundSessionEventCounter = ProtocolParser.ReadUInt64(stream);
                        }
                        break;
                    case 19u:
                        if (key.WireType == Wire.Varint)
                        {
                            instance.SessionInactivityTimeoutMilliseconds = ProtocolParser.ReadUInt64(stream);
                        }
                        break;
                    case 20u:
                        if (key.WireType == Wire.Varint)
                        {
                            instance.LastIdentityEventMilliseconds = ProtocolParser.ReadUInt64(stream);
                        }
                        break;
                    case 21u:
                        if (key.WireType == Wire.Varint)
                        {
                            instance.ServerTimeOffset = (long)ProtocolParser.ReadUInt64(stream);
                        }
                        break;
                    default:
                        ProtocolParser.SkipKey(stream, key);
                        break;
                }
            }
            throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
        }

        internal static Config DeserializeLengthDelimited(Stream stream, Config instance)
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
                        instance.UUID = ProtocolParser.ReadString(stream);
                        continue;
                    case 16:
                        instance.LastStartupTime = ProtocolParser.ReadUInt64(stream);
                        continue;
                    case 24:
                        instance.LastStopTime = ProtocolParser.ReadUInt64(stream);
                        continue;
                    case 32:
                        instance.CurrentSessionId = ProtocolParser.ReadUInt64(stream);
                        continue;
                    case 40:
                        instance.CurrentSessionEventCounter = ProtocolParser.ReadUInt64(stream);
                        continue;
                    case 48:
                        instance.Init = ProtocolParser.ReadBool(stream);
                        continue;
                    case 58:
                        instance.ApiKey = ProtocolParser.ReadString(stream);
                        continue;
                    case 64:
                        instance.DispatchPeriodMilliseconds = ProtocolParser.ReadUInt64(stream);
                        continue;
                    case 72:
                        instance.MaxReportsCount = ProtocolParser.ReadUInt32(stream);
                        continue;
                    case 80:
                        instance.TrackLocationEnabled = ProtocolParser.ReadBool(stream);
                        continue;
                    case 88:
                        instance.ReportCrashesEnabled = ProtocolParser.ReadBool(stream);
                        continue;
                    case 98:
                        instance.CustomStartupUrl = ProtocolParser.ReadString(stream);
                        continue;
                    case 106:
                        instance.CustomAppVersion = ProtocolParser.ReadString(stream);
                        continue;
                    case 114:
                        if (instance.CustomLocation == null)
                        {
                            instance.CustomLocation = Config.Location.DeserializeLengthDelimited(stream);
                            continue;
                        }
                        Config.Location.DeserializeLengthDelimited(stream, instance.CustomLocation);
                        continue;
                    case 122:
                        instance.ReportUrl = ProtocolParser.ReadString(stream);
                        continue;
                    default:
                        Key key = ProtocolParser.ReadKey((byte)num2, stream);
                        switch (key.Field)
                        {
                            case 0:
                                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            case 16:
                                if (key.WireType == Wire.LengthDelimited)
                                {
                                    instance.CheckUpdatesUrl = ProtocolParser.ReadString(stream);
                                    continue;
                                }
                                continue;
                            case 17:
                                if (key.WireType == Wire.Varint)
                                {
                                    instance.Suspended = ProtocolParser.ReadBool(stream);
                                    continue;
                                }
                                continue;
                            case 18:
                                if (key.WireType == Wire.Varint)
                                {
                                    instance.BackgroundSessionEventCounter = ProtocolParser.ReadUInt64(stream);
                                    continue;
                                }
                                continue;
                            case 19:
                                if (key.WireType == Wire.Varint)
                                {
                                    instance.SessionInactivityTimeoutMilliseconds = ProtocolParser.ReadUInt64(stream);
                                    continue;
                                }
                                continue;
                            case 20:
                                if (key.WireType == Wire.Varint)
                                {
                                    instance.LastIdentityEventMilliseconds = ProtocolParser.ReadUInt64(stream);
                                    continue;
                                }
                                continue;
                            case 21:
                                if (key.WireType == Wire.Varint)
                                {
                                    instance.ServerTimeOffset = (long)ProtocolParser.ReadUInt64(stream);
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

        internal static Config DeserializeLength(Stream stream, int length, Config instance)
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
                        instance.UUID = ProtocolParser.ReadString(stream);
                        continue;
                    case 16:
                        instance.LastStartupTime = ProtocolParser.ReadUInt64(stream);
                        continue;
                    case 24:
                        instance.LastStopTime = ProtocolParser.ReadUInt64(stream);
                        continue;
                    case 32:
                        instance.CurrentSessionId = ProtocolParser.ReadUInt64(stream);
                        continue;
                    case 40:
                        instance.CurrentSessionEventCounter = ProtocolParser.ReadUInt64(stream);
                        continue;
                    case 48:
                        instance.Init = ProtocolParser.ReadBool(stream);
                        continue;
                    case 58:
                        instance.ApiKey = ProtocolParser.ReadString(stream);
                        continue;
                    case 64:
                        instance.DispatchPeriodMilliseconds = ProtocolParser.ReadUInt64(stream);
                        continue;
                    case 72:
                        instance.MaxReportsCount = ProtocolParser.ReadUInt32(stream);
                        continue;
                    case 80:
                        instance.TrackLocationEnabled = ProtocolParser.ReadBool(stream);
                        continue;
                    case 88:
                        instance.ReportCrashesEnabled = ProtocolParser.ReadBool(stream);
                        continue;
                    case 98:
                        instance.CustomStartupUrl = ProtocolParser.ReadString(stream);
                        continue;
                    case 106:
                        instance.CustomAppVersion = ProtocolParser.ReadString(stream);
                        continue;
                    case 114:
                        if (instance.CustomLocation == null)
                        {
                            instance.CustomLocation = Config.Location.DeserializeLengthDelimited(stream);
                            continue;
                        }
                        Config.Location.DeserializeLengthDelimited(stream, instance.CustomLocation);
                        continue;
                    case 122:
                        instance.ReportUrl = ProtocolParser.ReadString(stream);
                        continue;
                    default:
                        Key key = ProtocolParser.ReadKey((byte)num2, stream);
                        switch (key.Field)
                        {
                            case 0:
                                throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
                            case 16:
                                if (key.WireType == Wire.LengthDelimited)
                                {
                                    instance.CheckUpdatesUrl = ProtocolParser.ReadString(stream);
                                    continue;
                                }
                                continue;
                            case 17:
                                if (key.WireType == Wire.Varint)
                                {
                                    instance.Suspended = ProtocolParser.ReadBool(stream);
                                    continue;
                                }
                                continue;
                            case 18:
                                if (key.WireType == Wire.Varint)
                                {
                                    instance.BackgroundSessionEventCounter = ProtocolParser.ReadUInt64(stream);
                                    continue;
                                }
                                continue;
                            case 19:
                                if (key.WireType == Wire.Varint)
                                {
                                    instance.SessionInactivityTimeoutMilliseconds = ProtocolParser.ReadUInt64(stream);
                                    continue;
                                }
                                continue;
                            case 20:
                                if (key.WireType == Wire.Varint)
                                {
                                    instance.LastIdentityEventMilliseconds = ProtocolParser.ReadUInt64(stream);
                                    continue;
                                }
                                continue;
                            case 21:
                                if (key.WireType == Wire.Varint)
                                {
                                    instance.ServerTimeOffset = (long)ProtocolParser.ReadUInt64(stream);
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

        internal static void Serialize(Stream stream, Config instance)
        {
            MemoryStream stream1 = ProtocolParser.Stack.Pop();
            if (instance.UUID != null)
            {
                stream.WriteByte((byte)10);
                ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.UUID));
            }
            stream.WriteByte((byte)16);
            ProtocolParser.WriteUInt64(stream, instance.LastStartupTime);
            stream.WriteByte((byte)24);
            ProtocolParser.WriteUInt64(stream, instance.LastStopTime);
            stream.WriteByte((byte)32);
            ProtocolParser.WriteUInt64(stream, instance.CurrentSessionId);
            stream.WriteByte((byte)40);
            ProtocolParser.WriteUInt64(stream, instance.CurrentSessionEventCounter);
            stream.WriteByte((byte)48);
            ProtocolParser.WriteBool(stream, instance.Init);
            if (instance.ApiKey != null)
            {
                stream.WriteByte((byte)58);
                ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.ApiKey));
            }
            stream.WriteByte((byte)64);
            ProtocolParser.WriteUInt64(stream, instance.DispatchPeriodMilliseconds);
            stream.WriteByte((byte)72);
            ProtocolParser.WriteUInt32(stream, instance.MaxReportsCount);
            stream.WriteByte((byte)80);
            ProtocolParser.WriteBool(stream, instance.TrackLocationEnabled);
            stream.WriteByte((byte)88);
            ProtocolParser.WriteBool(stream, instance.ReportCrashesEnabled);
            if (instance.CustomStartupUrl != null)
            {
                stream.WriteByte((byte)98);
                ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.CustomStartupUrl));
            }
            if (instance.CustomAppVersion != null)
            {
                stream.WriteByte((byte)106);
                ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.CustomAppVersion));
            }
            if (instance.CustomLocation != null)
            {
                stream.WriteByte((byte)114);
                stream1.SetLength(0L);
                Config.Location.Serialize((Stream)stream1, instance.CustomLocation);
                uint length = (uint)stream1.Length;
                ProtocolParser.WriteUInt32(stream, length);
                stream1.WriteTo(stream);
            }
            if (instance.ReportUrl != null)
            {
                stream.WriteByte((byte)122);
                ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.ReportUrl));
            }
            if (instance.CheckUpdatesUrl != null)
            {
                stream.WriteByte((byte)130);
                stream.WriteByte((byte)1);
                ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.CheckUpdatesUrl));
            }
            stream.WriteByte((byte)136);
            stream.WriteByte((byte)1);
            ProtocolParser.WriteBool(stream, instance.Suspended);
            stream.WriteByte((byte)144);
            stream.WriteByte((byte)1);
            ProtocolParser.WriteUInt64(stream, instance.BackgroundSessionEventCounter);
            stream.WriteByte((byte)152);
            stream.WriteByte((byte)1);
            ProtocolParser.WriteUInt64(stream, instance.SessionInactivityTimeoutMilliseconds);
            stream.WriteByte((byte)160);
            stream.WriteByte((byte)1);
            ProtocolParser.WriteUInt64(stream, instance.LastIdentityEventMilliseconds);
            stream.WriteByte((byte)168);
            stream.WriteByte((byte)1);
            ProtocolParser.WriteUInt64(stream, (ulong)instance.ServerTimeOffset);
            ProtocolParser.Stack.Push(stream1);
        }

        internal static byte[] SerializeToBytes(Config instance)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Config.Serialize((Stream)memoryStream, instance);
                return memoryStream.ToArray();
            }
        }

        internal static void SerializeLengthDelimited(Stream stream, Config instance)
        {
            byte[] bytes = Config.SerializeToBytes(instance);
            ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
            stream.Write(bytes, 0, bytes.Length);
        }

        internal class Location
        {
            public double Lat { get; set; }

            public double Lon { get; set; }

            internal static Config.Location Deserialize(Stream stream)
            {
                Config.Location instance = new Config.Location();
                Config.Location.Deserialize(stream, instance);
                return instance;
            }

            internal static Config.Location DeserializeLengthDelimited(Stream stream)
            {
                Config.Location instance = new Config.Location();
                Config.Location.DeserializeLengthDelimited(stream, instance);
                return instance;
            }

            internal static Config.Location DeserializeLength(Stream stream, int length)
            {
                Config.Location instance = new Config.Location();
                Config.Location.DeserializeLength(stream, length, instance);
                return instance;
            }

            internal static Config.Location Deserialize(byte[] buffer)
            {
                Config.Location instance = new Config.Location();
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    Config.Location.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            internal static Config.Location Deserialize(byte[] buffer, Config.Location instance)
            {
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    Config.Location.Deserialize((Stream)memoryStream, instance);
                return instance;
            }

            internal static Config.Location Deserialize(Stream stream, Config.Location instance)
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
                            instance.Lat = binaryReader.ReadDouble();
                            continue;
                        case 17:
                            instance.Lon = binaryReader.ReadDouble();
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

            internal static Config.Location DeserializeLengthDelimited(
              Stream stream,
              Config.Location instance)
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
                            instance.Lat = binaryReader.ReadDouble();
                            continue;
                        case 17:
                            instance.Lon = binaryReader.ReadDouble();
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

            internal static Config.Location DeserializeLength(
              Stream stream,
              int length,
              Config.Location instance)
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
                            instance.Lat = binaryReader.ReadDouble();
                            continue;
                        case 17:
                            instance.Lon = binaryReader.ReadDouble();
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

            internal static void Serialize(Stream stream, Config.Location instance)
            {
                BinaryWriter binaryWriter = new BinaryWriter(stream);
                MemoryStream stream1 = ProtocolParser.Stack.Pop();
                stream.WriteByte((byte)9);
                binaryWriter.Write(instance.Lat);
                stream.WriteByte((byte)17);
                binaryWriter.Write(instance.Lon);
                ProtocolParser.Stack.Push(stream1);
            }

            internal static byte[] SerializeToBytes(Config.Location instance)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Config.Location.Serialize((Stream)memoryStream, instance);
                    return memoryStream.ToArray();
                }
            }

            internal static void SerializeLengthDelimited(Stream stream, Config.Location instance)
            {
                byte[] bytes = Config.Location.SerializeToBytes(instance);
                ProtocolParser.WriteUInt32(stream, (uint)bytes.Length);
                stream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
