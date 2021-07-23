using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using System;
using System.Collections.Generic;

namespace TestMyExplorer
{
    class MainWindowDriver
    {
        private dynamic MainWindow { get; }

        private IWPFDependencyObjectCollection<System.Windows.DependencyObject> Tree { get; set; }

        public MainWindowDriver(dynamic mainWindow)
        {
            MainWindow = mainWindow;
            Tree = new WindowControl(mainWindow).LogicalTree();
        }

        private void UpdateNowMainWindowStatus()
        {
            Tree = new WindowControl(MainWindow).LogicalTree();    // 現在の画面状況を取得
        }

        internal void OpenFolder(string folderPath, int? row, int? col)
        {
            MainWindow.OpenFolder(folderPath, row, col);
        }

        internal bool ContainFile(string fileName, int? row, int? col)
        {
            throw new NotImplementedException();
        }

        internal string NowPath(int? row, int? col)
        {
            throw new NotImplementedException();
        }
    }
}
