using System;

namespace MyExplorer
{
    internal class FileExecuteRename : FileExecute
    {
        public FileExecuteRename(int index, string name, string filePath) : base(index, name, filePath) { }

        public override void Execute()
        {
            ShowRenameWindow();
            IsFileChanged = true;
        }

        private void ShowRenameWindow()
        {
            new FileRenameWindow(FilePath).ShowDialog();
        }
    }
}