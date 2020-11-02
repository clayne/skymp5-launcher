using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using System.IO;
using System.Linq;
using UpdatesClient.Modules.Configs;

namespace UpdatesClient.Core
{
    public class Unpacker
    {
        public static bool UnpackArchive(string file, string extractTo, string extractFromSub = "")
        {
            switch (Path.GetExtension(file))
            {
                case ".zip":
                    return UnpackZip(file, extractTo, extractFromSub);
                case ".7z":
                    return SevenZUnpack(file, extractTo, extractFromSub);

                default:
                    return false;
            }
        }
        private static bool UnpackZip(string file, string extractTo, string extractFromSub = "")
        {
            if (!File.Exists(file)) return false;

            string tmpFiles = $"{Settings.PathToSkyrimTmp}files\\";
            Delete(tmpFiles);
            Create(tmpFiles);

            using (var archive = ZipArchive.Open(file))
            {
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(tmpFiles, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }

            CopyToDir($"{tmpFiles}{extractFromSub}\\", extractTo);

            Delete(tmpFiles);
            File.Delete(file);

            return true;
        }
        private static bool SevenZUnpack(string file, string extractTo, string extractFromSub = "")
        {
            if (!File.Exists(file)) return false;
            
            string tmpFiles = $"{Settings.PathToSkyrimTmp}files\\";
            Delete(tmpFiles);
            Create(tmpFiles);

            using (var archive = SevenZipArchive.Open(file))
            {
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(extractTo, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }

            CopyToDir($"{tmpFiles}{extractFromSub}\\", extractTo);

            Delete(tmpFiles);
            File.Delete(file);

            return true;
        }

        private static bool ArchiveUnpack(string file, string extractTo, string extractFromSub = "")
        {
            if (!File.Exists(file)) return false;

            string tmpFiles = $"{Settings.PathToSkyrimTmp}files\\";
            Delete(tmpFiles);
            Create(tmpFiles);

            using (var archive = ArchiveFactory.Open(file))
            {
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(extractTo, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }

            CopyToDir($"{tmpFiles}{extractFromSub}\\", extractTo);

            Delete(tmpFiles);
            File.Delete(file);

            return true;
        }
        private static bool CopyToDir(string fromDir, string toDir)
        {
            foreach (DirectoryInfo dir in new DirectoryInfo(fromDir).GetDirectories())
            {
                Create($"{toDir}\\{dir.Name}");
                CopyToDir(dir.FullName, $"{toDir}\\{dir.Name}");
            }

            foreach (string file in Directory.GetFiles(fromDir))
            {
                string NameFile = file.Substring(file.LastIndexOf('\\'), file.Length - file.LastIndexOf('\\'));
                File.Copy(file, $"{toDir}\\{NameFile}", true);
            }
            return true;
        }

        private static void Create(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
        private static void Delete(string path)
        {
            if (Directory.Exists(path)) Directory.Delete(path, true);
        }
    }
}
