using System;

namespace Yandex.Metrica.Aides
{
    public class JsonProfile
    {
        public readonly string KeyValuePairName = "KeyValuePair`";

        public string NullLiteral { get; set; }

        public string TrueLiteral { get; set; }

        public string FalseLiteral { get; set; }

        public string EmptyArray { get; set; }

        public string EmptyObject { get; set; }

        public string DictionaryEntryPattern { get; set; }

        public System.Runtime.Serialization.DateTimeFormat DateTimeFormat { get; set; }

        public bool SimpleDictionaryFormat { get; set; }

        public string IndentChars { get; set; }

        public string NewLine { get; set; }

        public string Delimiter { get; set; }

        public static JsonProfile GetFormatted()
        {
            return new JsonProfile()
            {
                NullLiteral = "null",
                TrueLiteral = "true",
                FalseLiteral = "false",
                EmptyArray = "[ ]",
                EmptyObject = "{ }",
                DictionaryEntryPattern = "\"{0}\": {1}",
                SimpleDictionaryFormat = true,
                IndentChars = "  ",
                NewLine = Environment.NewLine,
                Delimiter = ","
            };
        }

        public static JsonProfile GetCompact()
        {
            return new JsonProfile()
            {
                NullLiteral = "null",
                TrueLiteral = "true",
                FalseLiteral = "false",
                EmptyArray = "[]",
                EmptyObject = "{}",
                DictionaryEntryPattern = "\"{0}\":{1}",
                SimpleDictionaryFormat = true,
                IndentChars = "",
                NewLine = "",
                Delimiter = ","
            };
        }
    }
}
