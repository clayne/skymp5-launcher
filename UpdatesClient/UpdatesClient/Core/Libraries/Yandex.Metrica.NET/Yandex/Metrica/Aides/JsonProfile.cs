// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Aides.JsonProfile
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

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
