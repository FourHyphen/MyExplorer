using Microsoft.WindowsAPICodePack.Dialogs;    // NuGet から WindowsAPICodePack-Shell をインストールする

namespace MyExplorer
{
    class DialogOpenFolder
    {
        public static string Show()
        {
            CommonOpenFileDialog cofd = CreateCommonOpenFileDialog();
            string folderPath = null;
            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                folderPath = cofd.FileName;
            }

            return folderPath;
        }

        private static CommonOpenFileDialog CreateCommonOpenFileDialog()
        {
            CommonOpenFileDialog cofd = new CommonOpenFileDialog();

            cofd.Title = "開くフォルダを選択してください";
            cofd.RestoreDirectory = true;
            cofd.IsFolderPicker = true;
            return cofd;
        }
    }
}
