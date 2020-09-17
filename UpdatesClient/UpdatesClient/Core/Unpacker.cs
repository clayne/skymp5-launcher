using SevenZip;
using System.IO;
using System.IO.Compression;
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
                    return UnpackZip(file, extractTo);

                default:
                    return SevenZUnpack(file, extractTo, extractFromSub);
            }
        }
        private static bool UnpackZip(string file, string extractTo, string extractFromSub = "")
        {
            if (!File.Exists(file)) return false;

            string tmpFiles = $"{Settings.PathToSkyrimTmp}files\\";
            Delete(tmpFiles);
            Create(tmpFiles);

            ZipFile.ExtractToDirectory(file, tmpFiles);
            CopyToDir($"{tmpFiles}{extractFromSub}\\", extractTo);

            Delete(tmpFiles);
            File.Delete(file);

            return true;
        }
        private static bool SevenZUnpack(string file, string extractTo, string extractFromSub = "")
        {
            string pathToLib = Settings.PathToLocalTmp + "7z.dll";
            if (File.Exists(pathToLib) && File.Exists(file))
            {
                SevenZipLibraryManager.SetLibraryPath(pathToLib);

                string tmpFiles = $"{Settings.PathToSkyrimTmp}files\\";
                Delete(tmpFiles);
                Create(tmpFiles);

                using (SevenZipExtractor extractor = new SevenZipExtractor(file))
                {
                    for (var i = 0; i < extractor.ArchiveFileData.Count; i++)
                        extractor.ExtractFiles(tmpFiles, extractor.ArchiveFileData[i].Index);
                }
                CopyToDir($"{tmpFiles}{extractFromSub}\\", extractTo);

                Delete(tmpFiles);
                File.Delete(file);

                return true;
            }
            return false;
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
