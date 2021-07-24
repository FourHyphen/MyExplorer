﻿using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using System;
using System.Collections.Generic;

namespace TestMyExplorer
{
    class MainWindowDriver
    {
        private dynamic MainWindow { get; }

        private IWPFDependencyObjectCollection<System.Windows.DependencyObject> Tree { get; set; }

        private TextBoxAdapter FolderPath { get; set; }

        private ListBoxAdapter FileList { get; set; }

        public MainWindowDriver(dynamic mainWindow)
        {
            MainWindow = mainWindow;
            FolderPath = new TextBoxAdapter("Folder");
            FileList = new ListBoxAdapter("FileList");
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
            return FileList.Contains(fileName, Tree);
        }

        public string GetFolderPath(int? row, int? col)
        {
            UpdateNowMainWindowStatus();
            return FolderPath.Text(Tree);
        }

        internal bool ContainFolder(string folderName, int? row, int? col)
        {
            return FileList.Contains(folderName, Tree);
        }
    }
}