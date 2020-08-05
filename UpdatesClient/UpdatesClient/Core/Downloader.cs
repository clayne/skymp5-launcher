using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UpdatesClient.Core
{
    public class Downloader
    {
        public delegate void DownloaderStateHandler(double Value, double LenFile, double prDown);
        public event DownloaderStateHandler DownloadChanged;

        public delegate void DownloadedStateHandler(string DestinationFile, string Vers);
        public event DownloadedStateHandler DownloadComplete;

        private long iFileSize = 0;
        private double DownValue = 0;
        private double DownMax = 0;
        public int iBufferSize = 1024;

        public bool Downloading = false;
        public string sDestinationPath = $"\\";
        public string sInternetPath;
        public string sVers;

        public Downloader(string DestinationPath, string InternetPath, string Vers)
        {
            sDestinationPath = DestinationPath;
            sInternetPath = InternetPath;
            sVers = Vers;
            iBufferSize *= 10;
        }

        public async void StartAsync()
        {
            string path = Path.GetDirectoryName(sDestinationPath);
            if (path != null && path != "" && !Directory.Exists(path)) Directory.CreateDirectory(path);

            await Task.Run(() => StartDown());
        }

        private void StartDown()
        {
            try
            {
                Downloading = true;

                if (Directory.Exists(Path.GetDirectoryName(sDestinationPath))) Directory.CreateDirectory(Path.GetDirectoryName(sDestinationPath));

                DownloadFile();

                Downloading = false;
                
                DownloadComplete?.Invoke(sDestinationPath, sVers);
            }
            catch (Exception er) { Downloading = false; DownloadComplete?.Invoke(null, sVers); }
        }

        private void DownloadFile()
        {
            HttpWebRequest hwRq = (HttpWebRequest)HttpWebRequest.Create(new Uri(sInternetPath));
            hwRq.Timeout = 30000;
            hwRq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            hwRq.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:74.0) Gecko/20100101 Firefox/74.0";

            using (FileStream saveFileStream = new FileStream(sDestinationPath, FileMode.Create, FileAccess.Write))
            {
                using (HttpWebResponse hwRes = (HttpWebResponse)hwRq.GetResponse())
                {
                    using (Stream smRespStream = hwRes.GetResponseStream())
                    {
                        iFileSize = hwRes.ContentLength;

                        DownMax = (int)(iFileSize / 1024);

                        int iByteSize;
                        byte[] downBuffer = new byte[iBufferSize];

                        while ((iByteSize = smRespStream.Read(downBuffer, 0, downBuffer.Length)) > 0)
                        {
                            saveFileStream.Write(downBuffer, 0, iByteSize);
                            DownValue = (int)(saveFileStream.Length / 1024);
                            DownloadChanged?.Invoke(DownValue, DownMax, DownValue / DownMax * 100);
                        }
                    }
                }
            }
        }
    }
}
