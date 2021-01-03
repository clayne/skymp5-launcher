using SharpCompress.Archives;
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
            if (!File.Exists(file)) return false;

            string tmpFiles = $"{Settings.PathToSkyrimTmp}files\\";
            Delete(tmpFiles);
            Create(tmpFiles);

            using (IArchive archive = ArchiveFactory.Open(file))
            {
                foreach (IArchiveEntry entry in archive.Entries.Where(entry => !entry.IsDirectory))
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
                string pathToDestFile = $"{toDir}\\{NameFile}";

                if (File.GetAttributes(pathToDestFile) != FileAttributes.Normal)
                    File.SetAttributes(pathToDestFile, FileAttributes.Normal);
                
                File.Copy(file, pathToDestFile, true);
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
