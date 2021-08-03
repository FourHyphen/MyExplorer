using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace TestMyExplorer
{
    class MainWindowDriver
    {
        private dynamic MainWindow { get; }

        private IWPFDependencyObjectCollection<System.Windows.DependencyObject> Tree { get; set; }

        private FolderPathAdapter FolderPath { get; set; }

        private FileListAdapter FileList { get; set; }

        public MainWindowDriver(dynamic mainWindow)
        {
            MainWindow = mainWindow;
            FolderPath = new FolderPathAdapter("FolderPath");
            FileList = new FileListAdapter("FolderFileList");
            Tree = new WindowControl(mainWindow).LogicalTree();
        }

        public string GetFolderPath()
        {
            UpdateNowMainWindowStatus();
            return FolderPath.Text(Tree);
        }

        internal bool ContainFile(string fileName)
        {
            return FileList.Contains(fileName, Tree);
        }

        internal void OpenFolder(string folderPath)
        {
            MainWindow.OpenFolder(folderPath);
        }

        internal void EmurateKey(Key key, ModifierKeys modifier)
        {
            if (modifier == ModifierKeys.None)
            {
                MainWindow.InputKey(key, Key.None, modifier);
            }
            else
            {
                MainWindow.InputKey(Key.None, key, modifier);
            }
        }

        internal void FocusFile(string fileName)
        {
            MainWindow.FocusFile(fileName);
        }

        internal void FocusFolderPathArea()
        {
            MainWindow.FocusFolderPathArea();
        }

        internal void SetFolderPathText(string str)
        {
            MainWindow.SetFolderPathText(str);
        }

        private void UpdateNowMainWindowStatus()
        {
            Tree = new WindowControl(MainWindow).LogicalTree();    // 現在の画面状況を取得
        }
    }
}
