using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Yandex.Metrica.Aero;

namespace Yandex.Metrica.Aides
{
    public static class JsonSerializer
    {
        internal static readonly long DatetimeMinTimeTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

        private static string GetIndent(int indentLevel, string indent)
        {
            string empty = string.Empty;
            for (int index = 0; index < indentLevel; ++index)
                empty += indent;
            return empty;
        }

        public static string ToJson(this object value, Type memberType = null)
        {
            return value.ToJson(JsonProfile.GetFormatted(), memberType, 1);
        }

        public static string ToJson(
          this object value,
          JsonProfile profile,
          Type memberType = null,
          int indentLevel = 1)
        {
            if (value.IsSimpleValue(profile))
                return value.ToSimpleValue(profile);
            return !value.IsArray(profile) ? value.ToObject(profile, memberType, indentLevel) : value.ToArray(profile, indentLevel);
        }

        private static bool IsSimpleValue(this object value, JsonProfile profile)
        {
            switch (value)
            {
                case null:
                case string _:
                case Guid _:
                    label_9:
                    return true;
                default:
                    if ((value as Uri) is null)
                    {
                        switch (value)
                        {
                            case DateTime _:
                            case Decimal _:
                            case Enum _:
                                goto label_9;
                            default:
                                if (!value.GetType().GetTypeInfo().IsPrimitive)
                                {
                                    if (!profile.SimpleDictionaryFormat)
                                        return false;
                                    return value.GetType().Name.StartsWith(profile.KeyValuePairName) || value is DictionaryEntry;
                                }
                                goto label_9;
                        }
                    }
                    else
                        goto case null;
            }
        }

        private static bool IsArray(this object value, JsonProfile profile)
        {
            return (!profile.SimpleDictionaryFormat || !(value is IDictionary)) && value is ICollection collection && !((IEnumerable<object>)collection.GetType().GetCustomAttributes(typeof(DataContractAttribute), true)).Any<object>();
        }

        private static string ToSimpleValue(this object value, JsonProfile profile)
        {
            if (value == null)
                return profile.NullLiteral;
            if (value is string || value is Guid || (value as Uri) is object)
                return "\"" + JsonSerializer.Escape(value.ToString()) + "\"";
            switch (value)
            {
                case Decimal _:
                    return value.Of<Decimal>().ToString("G", (IFormatProvider)CultureInfo.InvariantCulture);
                case double _:
                    return value.Of<double>().ToString("G", (IFormatProvider)CultureInfo.InvariantCulture);
                case float _:
                    return value.Of<float>().ToString("G", (IFormatProvider)CultureInfo.InvariantCulture);
                case Enum _:
                    return value.Of<int>().ToString();
                default:
                    if (profile.SimpleDictionaryFormat && value is DictionaryEntry dictionaryEntry)
                        return string.Format(profile.DictionaryEntryPattern, (object)JsonSerializer.Escape(dictionaryEntry.Key.ToString()), (object)dictionaryEntry.Value.ToJson(profile, typeof(object), 1));
                    if (value is DateTime)
                    {
                        DateTime dateTime = value.Of<DateTime>();
                        if (profile.DateTimeFormat != null)
                            return dateTime.ToString(profile.DateTimeFormat.FormatProvider);
                        return "\"\\/Date(" + (object)((dateTime.ToUniversalTime().Ticks - JsonSerializer.DatetimeMinTimeTicks) / 10000L) + "+" + DateTimeOffset.Now.Offset.ToString("hhmm") + ")\\/\"";
                    }
                    if (object.Equals(value, (object)true))
                        return profile.TrueLiteral;
                    return !object.Equals(value, (object)false) ? JsonSerializer.Escape(value.ToString()) : profile.FalseLiteral;
            }
        }

        private static string ToArray(this object value, JsonProfile profile, int indentLevel)
        {
            if (!(value is ICollection items) || items.Count == 0)
                return profile.EmptyArray;
            string emptyDelimiter = profile.NewLine + JsonSerializer.GetIndent(indentLevel - 1, profile.IndentChars);
            string indent = profile.NewLine + JsonSerializer.GetIndent(indentLevel, profile.IndentChars);
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append(items, profile, profile.Delimiter, emptyDelimiter, indent, indentLevel);
            return "[" + (object)jsonBuilder + "]";
        }

        private static string ToObject(
          this object value,
          JsonProfile profile,
          Type memberType,
          int indentLevel)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            string indent = profile.NewLine + JsonSerializer.GetIndent(indentLevel, profile.IndentChars);
            string emptyDelimiter = profile.NewLine + JsonSerializer.GetIndent(indentLevel - 1, profile.IndentChars);
            if (value is IDictionary items && profile.SimpleDictionaryFormat)
            {
                if (items.Count == 0)
                    return profile.EmptyObject;
                jsonBuilder.Append(items, profile, profile.Delimiter, emptyDelimiter, indent);
            }
            else
            {
                Type type = value.GetType();
                Dictionary<string, MemberInfo> dataMembers = JsonSerializer.GetDataMembers(value);
                bool flag = dataMembers.Count == 0;
                if (memberType != (Type)null && memberType != type)
                {
                    string str1 = flag ? emptyDelimiter : profile.Delimiter;
                    jsonBuilder.Append(indent);
                    string str2 = type.FullName.Substring(type.Namespace == null ? 0 : type.Namespace.Length + 1).Replace("+", ".");
                    jsonBuilder.Append(string.Format(profile.DictionaryEntryPattern, (object)"__type", (object)("\"" + str2 + ":#" + type.Namespace + "\"")));
                    jsonBuilder.Append(str1);
                    if (flag)
                        return "{" + (object)jsonBuilder + "}";
                }
                if (flag)
                    return profile.EmptyObject;
                Dictionary<string, string> dictionary = dataMembers.ToDictionary<KeyValuePair<string, MemberInfo>, string, string>((Func<KeyValuePair<string, MemberInfo>, string>)(p => p.Key), (Func<KeyValuePair<string, MemberInfo>, string>)(p => p.Value.GetValue(value).ToJson(profile, p.Value.GetMemberType(), indentLevel + 1)));
                jsonBuilder.Append(dictionary, profile.DictionaryEntryPattern, profile.Delimiter, emptyDelimiter, indent);
            }
            return "{" + (object)jsonBuilder + "}";
        }

        private static Dictionary<string, MemberInfo> GetDataMembers(object item)
        {
            Type type = item.GetType();
            return (item is DictionaryEntry ? (IEnumerable<KeyValuePair<MemberInfo, DataMemberAttribute>>)((IEnumerable<MemberInfo>)type.GetMembers()).ToDictionary<MemberInfo, MemberInfo, DataMemberAttribute>((Func<MemberInfo, MemberInfo>)(i => i), (Func<MemberInfo, DataMemberAttribute>)(i => (DataMemberAttribute)null)).ToList<KeyValuePair<MemberInfo, DataMemberAttribute>>() : (IEnumerable<KeyValuePair<MemberInfo, DataMemberAttribute>>)((IEnumerable<MemberInfo>)type.GetMembers()).ToDictionary<MemberInfo, MemberInfo, DataMemberAttribute>((Func<MemberInfo, MemberInfo>)(i => i), (Func<MemberInfo, DataMemberAttribute>)(i => i.GetCustomAttribute<DataMemberAttribute>())).Where<KeyValuePair<MemberInfo, DataMemberAttribute>>((Func<KeyValuePair<MemberInfo, DataMemberAttribute>, bool>)(p => p.Value != null)).OrderBy<KeyValuePair<MemberInfo, DataMemberAttribute>, string>((Func<KeyValuePair<MemberInfo, DataMemberAttribute>, string>)(p => p.Value.Name)).ThenBy<KeyValuePair<MemberInfo, DataMemberAttribute>, int>((Func<KeyValuePair<MemberInfo, DataMemberAttribute>, int>)(p => p.Value.Order)).ToList<KeyValuePair<MemberInfo, DataMemberAttribute>>()).ToDictionary<KeyValuePair<MemberInfo, DataMemberAttribute>, string, MemberInfo>((Func<KeyValuePair<MemberInfo, DataMemberAttribute>, string>)(p => p.Value != null ? p.Value.Name ?? p.Key.Name : p.Key.Name), (Func<KeyValuePair<MemberInfo, DataMemberAttribute>, MemberInfo>)(p => p.Key));
        }

        private static Type GetMemberType(this MemberInfo memberInfo)
        {
            PropertyInfo propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != (PropertyInfo)null)
                return propertyInfo.PropertyType;
            FieldInfo fieldInfo = memberInfo as FieldInfo;
            return fieldInfo != (FieldInfo)null ? fieldInfo.FieldType : (Type)null;
        }

        private static object GetValue(this MemberInfo memberInfo, object obj)
        {
            PropertyInfo propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != (PropertyInfo)null)
                return propertyInfo.GetValue(obj, (object[])null);
            FieldInfo fieldInfo = memberInfo as FieldInfo;
            return fieldInfo != (FieldInfo)null ? fieldInfo.GetValue(obj) : (object)null;
        }

        private static void Append(
          this StringBuilder jsonBuilder,
          Dictionary<string, string> items,
          string keyValuePattern,
          string actualDelimiter,
          string emptyDelimiter,
          string indent)
        {
            int num = 1;
            foreach (KeyValuePair<string, string> keyValuePair in items)
            {
                string str = num++ == items.Count ? emptyDelimiter : actualDelimiter;
                jsonBuilder.Append(indent);
                jsonBuilder.Append(string.Format(keyValuePattern, (object)keyValuePair.Key, (object)keyValuePair.Value));
                jsonBuilder.Append(str);
            }
        }

        private static void Append(
          this StringBuilder jsonBuilder,
          IDictionary items,
          JsonProfile profile,
          string actualDelimiter,
          string emptyDelimiter,
          string indent)
        {
            int num = 1;
            foreach (object obj in items)
            {
                string str = num++ == items.Count ? emptyDelimiter : actualDelimiter;
                jsonBuilder.Append(indent);
                jsonBuilder.Append(obj.ToJson(profile, (Type)null, 1));
                jsonBuilder.Append(str);
            }
        }

        private static void Append(
          this StringBuilder jsonBuilder,
          ICollection items,
          JsonProfile profile,
          string actualDelimiter,
          string emptyDelimiter,
          string indent,
          int indentLevel)
        {
            int num = 1;
            foreach (object obj in (IEnumerable)items)
            {
                string str = num++ == items.Count ? emptyDelimiter : actualDelimiter;
                jsonBuilder.Append(indent);
                jsonBuilder.Append(obj.ToJson(profile, typeof(object), indentLevel + 1));
                jsonBuilder.Append(str);
            }
        }

        public static string Escape(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char ch in value)
            {
                switch (ch)
                {
                    case '"':
                        stringBuilder.Append("\\\"");
                        break;
                    case '/':
                        stringBuilder.Append("\\/");
                        break;
                    case '\\':
                        stringBuilder.Append("\\\\");
                        break;
                    default:
                        int num = (int)ch;
                        if (num < 32 || num > (int)sbyte.MaxValue)
                        {
                            stringBuilder.AppendFormat("\\u{0:x04}", (object)num);
                            break;
                        }
                        stringBuilder.Append(ch);
                        break;
                }
            }
            return stringBuilder.ToString();
        }
    }
}
