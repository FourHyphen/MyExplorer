namespace MyExplorer
{
    public class FileExecuteSetClipboard : FileExecute
    {
        public FileExecuteSetClipboard(int index, string name, string filePath) : base(index, name, filePath) { }

        public override void Execute()
        {
            System.Windows.Clipboard.SetText(FilePath);
        }
    }
}