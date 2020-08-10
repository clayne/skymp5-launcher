namespace SilentOrbit.ProtocolBuffers
{
    public class KeyValue
    {
        public Key Key { get; set; }

        public byte[] Value { get; set; }

        public KeyValue(Key key, byte[] value)
        {
            this.Key = key;
            this.Value = value;
        }

        public override string ToString()
        {
            return string.Format("[KeyValue: {0}, {1}, {2} bytes]", (object)this.Key.Field, (object)this.Key.WireType, (object)this.Value.Length);
        }
    }
}
