using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Xml;

namespace Yandex.Metrica.Aero
{
    internal class Memory : IMemoryBox
    {
        public readonly DataContractJsonSerializerSettings Settings = new DataContractJsonSerializerSettings()
        {
            UseSimpleDictionaryFormat = true,
            KnownTypes = (IEnumerable<Type>)new List<Type>()
      {
        typeof (Type),
        typeof (Dictionary<string, string>)
      }
        };

        public static Memory ActiveBox { get; set; }

        public IStorage Storage { get; }

        public string KeyFormat { get; }

        public string IndentChars { get; set; }

        public bool Indent { get; set; }

        public List<Type> KnownTypes
        {
            get
            {
                return this.Settings.KnownTypes as List<Type>;
            }
        }

        public Memory(IStorage storage, string keyFormat = "{0}.json", bool indent = true, string indentChars = "  ")
        {
            this.Storage = storage;
            this.KeyFormat = keyFormat;
            this.Indent = indent;
            this.IndentChars = indentChars;
        }

        public TValue Revive<TValue>(string key = null, params object[] constructorArgs)
        {
            try
            {
                Type type = typeof(TValue);
                if ((System.Attribute.IsDefined((MemberInfo)type, typeof(DataContractAttribute)) ? 1 : (System.Attribute.IsDefined((MemberInfo)type, typeof(CollectionDataContractAttribute)) ? 1 : 0)) == 0)
                    return (TValue)Activator.CreateInstance(type, constructorArgs);
                using (Stream readStream = this.Storage.GetReadStream(this.MakeStorageKey(key, typeof(TValue))))
                {
                    CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    try
                    {
                        TValue obj = (TValue)new DataContractJsonSerializer(type, this.Settings).ReadObject(readStream);
                        if (object.Equals((object)obj, (object)null))
                            throw new Exception();
                        return obj;
                    }
                    catch (Exception)
                    {
                        return (TValue)Activator.CreateInstance(type, constructorArgs);
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentCulture = currentCulture;
                    }
                }
            }
            catch
            {
                return (TValue)Activator.CreateInstance(typeof(TValue), constructorArgs);
            }
        }

        public void Keep<TValue>(TValue item, string key = null)
        {
            try
            {
                Type type = item.GetType();
                if ((System.Attribute.IsDefined((MemberInfo)type, typeof(DataContractAttribute)) ? 1 : (System.Attribute.IsDefined((MemberInfo)type, typeof(CollectionDataContractAttribute)) ? 1 : 0)) == 0)
                    return;
                using (Stream writeStream = this.Storage.GetWriteStream(this.MakeStorageKey(key, type)))
                {
                    CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    try
                    {
                        using (XmlDictionaryWriter jsonWriter = JsonReaderWriterFactory.CreateJsonWriter(writeStream, Encoding.UTF8, true, this.Indent, this.IndentChars))
                        {
                            new DataContractJsonSerializer(type, this.Settings).WriteObject(jsonWriter, (object)item);
                            jsonWriter.Flush();
                        }
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentCulture = currentCulture;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public bool Check<TValue>(string key = null)
        {
            return this.Storage.HasKey(this.MakeStorageKey(key, typeof(TValue)));
        }

        public void Destroy<TValue>(string key = null)
        {
            this.Storage.DeleteKey(this.MakeStorageKey(key, typeof(TValue)));
        }

        private string MakeStorageKey(string key, Type type)
        {
            string path = string.Format(this.KeyFormat ?? "{0}", (object)(key ?? type.Name));
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName == null || this.Storage.DirectoryExists(directoryName))
                return path;
            this.Storage.CreateDirectory(directoryName);
            return path;
        }
    }
}
