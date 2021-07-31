﻿using Codeer.Friendly.Windows.Grasp;
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

        private TextBoxAdapter FolderPath { get; set; }

        private ListBoxAdapter FileList { get; set; }

        public MainWindowDriver(dynamic mainWindow)
        {
            MainWindow = mainWindow;
            FolderPath = new TextBoxAdapter("Folder");
            FileList = new ListBoxAdapter("FileList");
            Tree = new WindowControl(mainWindow).LogicalTree();
        }

        public string GetFolderPath(int index)
        {
            UpdateNowMainWindowStatus();
            return FolderPath.Text(index, Tree);
        }

        internal bool ContainFile(string fileName, int index)
        {
            return FileList.Contains(fileName, index, Tree);
        }

        internal bool ContainFolder(string folderName, int index)
        {
            return FileList.Contains(folderName, index, Tree);
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

        private void UpdateNowMainWindowStatus()
        {
            Tree = new WindowControl(MainWindow).LogicalTree();    // 現在の画面状況を取得
        }
    }
}
