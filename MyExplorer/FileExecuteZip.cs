using System.IO.Compression;

namespace MyExplorer
{
    public class FileExecuteZip : FileExecute
    {
        public FileExecuteZip(int index, string name, string filePath) : base(index, name, filePath) { }

        public override void Execute()
        {
            if (System.IO.Directory.Exists(FilePath))
            {
                ZipFolder();
            }
            else
            {
                ZipFile();
            }
        }

        private void ZipFolder()
        {
            string zipName = System.IO.Path.GetFileName(FilePath) + ".zip";
            string zipPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(FilePath), zipName);

            // System.IO.Compression.FileStream の参照が必要
            System.IO.Compression.ZipFile.CreateFromDirectory(FilePath, zipPath);
        }

        private void ZipFile()
        {
            string ext = System.IO.Path.GetExtension(FilePath);
            string zipName = System.IO.Path.GetFileName(FilePath).Replace(ext, ".zip");
            string zipPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(FilePath), zipName);

            // System.IO.Compression および System.IO.Compression.FileStream の参照が必要
            using (ZipArchive archive = System.IO.Compression.ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                // CreateEntryFromFile() の呼び出しには using System.IO.Compression が必要
                archive.CreateEntryFromFile(FilePath, System.IO.Path.GetFileName(FilePath));
            }
        }
    }
}