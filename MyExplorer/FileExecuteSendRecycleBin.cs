using Microsoft.VisualBasic.FileIO;

namespace MyExplorer
{
    public class FileExecuteSendRecycleBin : FileExecute
    {
        public FileExecuteSendRecycleBin(int index, string name, string filePath) : base(index, name, filePath) { }

        public override void Execute()
        {
            if (System.IO.Directory.Exists(FilePath))
            {
                SendRecycleBinOfFolder();
            }
            else
            {
                SendRecycleBinOfFile();
            }

            IsFileChanged = true;
        }

        private void SendRecycleBinOfFolder()
        {
            // 参照に Microsoft.VisualBasic を追加すること
            FileSystem.DeleteDirectory(FilePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
        }

        private void SendRecycleBinOfFile()
        {
            FileSystem.DeleteFile(FilePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
        }
    }
}