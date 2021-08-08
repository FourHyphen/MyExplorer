namespace MyExplorer
{
    public class ExplorerFileInfo
    {
        public string FullPath { get; private set; }

        public string Name { get; private set; }

        public string Date { get; private set; }

        public string Type { get; private set; }

        public string Size { get; private set; }

        public ExplorerFileInfo(string filePath)
        {
            Init(filePath);
        }

        private void Init(string filePath)
        {
            if (System.IO.Directory.Exists(filePath))
            {
                InitFolder(filePath);
            }
            else if (System.IO.File.Exists(filePath))
            {
                InitFile(filePath);
            }
            else if (filePath == Common.MoveOneUpFolderString)
            {
                InitBackFolderIcon();
            }
            else
            {
                InitNone();
            }
        }

        private void InitFolder(string folderPath)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folderPath);
            FullPath = di.FullName;
            Name = di.Name;
            Date = di.LastWriteTime.ToString("yyyyMMdd HH:mm:ss");
            Type = Common.TypeFolderString;
            Size = "";
        }

        private void InitFile(string filePath)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(filePath);
            FullPath = fi.FullName;
            Name = fi.Name;
            Date = fi.LastWriteTime.ToString("yyyyMMdd HH:mm:ss");
            Type = fi.Extension;
            Size = "WIP";
        }

        private void InitBackFolderIcon()
        {
            FullPath = "";
            Name = Common.MoveOneUpFolderString;
            Date = "";
            Type = "";
            Size = "";
        }

        private void InitNone()
        {
            FullPath = "";
            Name = "(不明)";
            Date = "";
            Type = "";
            Size = "";
        }
    }
}