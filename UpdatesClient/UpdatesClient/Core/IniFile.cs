using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Yandex.Metrica;

namespace UpdatesClient.Core
{
    public class IniFile
    {
        private readonly string Path;

        [DllImport("kernel32")]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath)
        {
            Path = new FileInfo(IniPath).ToString();
        }

        public string ReadINI(string Section, string Key)
        {
            try
            {
                var RetVal = new StringBuilder(65500);
                GetPrivateProfileString(Section, Key, "", RetVal, 65500, Path);
                return RetVal.ToString();
            }
            catch (Exception Ex)
            {
                YandexMetrica.ReportError("IniFile_Read", Ex);
                Logger.Error(Ex);
                return "0";
            }


        }
        //Записываем в ini-файл. Запись происходит в выбранную секцию в выбранный ключ.
        public void WriteINI(string Section, string Key, string Value)
        {
            try
            {
                WritePrivateProfileString(Section, Key, Value, Path);
            }
            catch (Exception Ex)
            {
                YandexMetrica.ReportError("IniFile_Write", Ex);
                Logger.Error(Ex);
            }

        }

        //Удаляем ключ из выбранной секции.
        public void DeleteKey(string Key, string Section = null)
        {
            try
            {
                WriteINI(Section, Key, null);
            }
            catch (Exception Ex)
            {
                YandexMetrica.ReportError("IniFile_DeleteKey", Ex);
                Logger.Error(Ex);
            }

        }
        //Удаляем выбранную секцию
        public void DeleteSection(string Section = null)
        {
            try
            {
                WriteINI(Section, null, null);
            }
            catch (Exception Ex)
            {
                YandexMetrica.ReportError("IniFile_DeleteSection", Ex);
                Logger.Error(Ex);
            }

        }
        //Проверяем, есть ли такой ключ, в этой секции
        public bool KeyExists(string Section, string Key)
        {
            try
            {
                return ReadINI(Section, Key).Length > 0;
            }
            catch (Exception Ex)
            {
                YandexMetrica.ReportError("IniFile_KeyExists", Ex);
                Logger.Error(Ex);
                return false; 
            }
        }
    }
}
