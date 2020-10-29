using System;
using System.IO;
using System.Net;
using System.Windows;
using UpdatesClient.Core;
using Yandex.Metrica;

namespace UpdatesClient.Modules.SelfUpdater
{
    internal class Downloader
    {
        public delegate void DownloaderStateHandler(double Value, double LenFile, double prDown);
        public event DownloaderStateHandler DownloadChanged;

        public bool IsHidden = false;

        private long iFileSize = 0;
        private double DownValue = 0;
        private double DownMax = 0;
        private readonly int iBufferSize = 1024;
        private long iExistLen = 0;

        private readonly string sDestinationPath;
        private readonly string sInternetPath;

        public Downloader(string _sInternetPath, string _sDestinationPath)
        {
            sDestinationPath = _sDestinationPath;
            sInternetPath = _sInternetPath;
            iBufferSize *= 10;
        }

        public bool Download()
        {
            try
            {
                DownloadFile();
                return true;
            }
            catch (Exception e) { YandexMetrica.ReportError("SelfUpdate_Downloader", e); Logger.Error(e); }
            return false;
        }

        private void DownloadFile()
        {
            HttpWebRequest hwRq;
            string sPath = $"{sDestinationPath}";

            string path = Path.GetDirectoryName(sPath);
            if (path != null && path != "") Directory.CreateDirectory(path);

            hwRq = (HttpWebRequest)HttpWebRequest.Create(new Uri($"{sInternetPath}"));
            hwRq.Timeout = 10000;
            hwRq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            hwRq.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:74.0) Gecko/20100101 Firefox/74.0";
            if (File.Exists(sPath)) iExistLen = new FileInfo(sPath).Length;
            using (FileStream saveFileStream = iExistLen > 0 ? new FileStream(sPath, FileMode.Append, FileAccess.Write) : new FileStream(sPath, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    if (IsHidden) File.SetAttributes(sPath, FileAttributes.Hidden);
                }
                catch (Exception e)
                {
                    YandexMetrica.ReportError("Down_Hidden", e);
                    Logger.Error(e);
                }
                hwRq.AddRange(iExistLen);

                using (HttpWebResponse hwRes = (HttpWebResponse)hwRq.GetResponse())
                {
                    if (hwRes.StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable) return;

                    using (Stream smRespStream = hwRes.GetResponseStream())
                    {
                        iFileSize = hwRes.ContentLength;

                        DownMax = (int)((iFileSize + iExistLen) / 1024);

                        int iByteSize;
                        byte[] downBuffer = new byte[iBufferSize];

                        while ((iByteSize = smRespStream.Read(downBuffer, 0, downBuffer.Length)) > 0)
                        {
                            saveFileStream.Write(downBuffer, 0, iByteSize);

                            DownValue = (int)(saveFileStream.Length / 1024);

                            DownloadChanged?.Invoke(DownValue, DownMax, (DownValue / DownMax * 100));
                        }
                    }
                }
            }
        }
    }
}
