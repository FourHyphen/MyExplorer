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
            FileRenameWindow frw = new FileRenameWindow(System.IO.Path.GetFileName(FilePath));
            frw.Closed += Frw_Closed;
            frw.ShowDialog();
        }

        private void Frw_Closed(object sender, EventArgs e)
        {

        }
    }
}