using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using System.Windows.Input;

namespace TestMyExplorer
{
    class MainWindowDriver
    {
        private dynamic MainWindow { get; }

        private WindowsAppFriend App { get; }

        private IWPFDependencyObjectCollection<System.Windows.DependencyObject> Tree { get; set; }

        private FolderPathAdapter FolderPath { get; set; }

        private FileListAdapter FileList { get; set; }

        public MainWindowDriver(WindowsAppFriend app)
        {
            MainWindow = app.Type("System.Windows.Application").Current.MainWindow;
            App = app;
            FolderPath = new FolderPathAdapter("FolderPath");
            FileList = new FileListAdapter("FolderFileList");
            Tree = new WindowControl(MainWindow).LogicalTree();
        }

        public string GetFolderPath()
        {
            UpdateNowMainWindowStatus();
            return FolderPath.Text(Tree);
        }

        internal bool ContainFile(string fileName)
        {
            UpdateNowMainWindowStatus();
            return FileList.Contains(fileName, MainWindow, App, Tree);
        }

        internal void OpenFolder(string folderPath)
        {
            MainWindow.OpenFolder(folderPath);
            UpdateNowMainWindowStatus();
        }

        internal void EmurateKey(Key key, ModifierKeys modifier)
        {
            if (modifier == ModifierKeys.None)
            {
                // MainWindow.FoldersArea とすると Canvas 型のメソッドを探してしまう
                MainWindow.Explorer.KeyDowned(key, Key.None, modifier);
            }
            else
            {
                MainWindow.Explorer.KeyDowned(Key.None, key, modifier);
            }
        }

        internal void FocusFile(string fileName)
        {
            MainWindow.Explorer.FocusFile(fileName);
            UpdateNowMainWindowStatus();
        }

        internal void FocusFolderPathArea()
        {
            MainWindow.Explorer.FocusFolderPathArea();
            UpdateNowMainWindowStatus();
        }

        internal void SetFolderPathText(string str)
        {
            FolderPath.SetText(str, Tree);
            UpdateNowMainWindowStatus();
        }

        private void UpdateNowMainWindowStatus()
        {
            Tree = new WindowControl(MainWindow).LogicalTree();    // 現在の画面状況を取得
        }
    }
}
