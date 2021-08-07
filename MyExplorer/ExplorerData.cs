using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyExplorer
{
    public class ExplorerData
    {
        public string FolderPath { get; set; } = "";

        public List<string> FileList { get; set; } = null;

        public ExplorerData(string folderPath)
        {
            SetFolderInfo(folderPath);
        }

        private void SetFolderInfo(string folderPath)
        {
            FolderPath = folderPath;
            FileList = new List<string>();
            AddGoBack();
            AddFiles(folderPath, System.IO.Directory.GetDirectories);
            AddFiles(folderPath, System.IO.Directory.GetFiles);
        }

        private void AddGoBack()
        {
            FileList.Add(Common.MoveOneUpFolderString);
        }

        private void AddFiles(string folderPath, Func<string, string[]> func)
        {
            string[] files = func(folderPath);
            foreach (string file in files)
            {
                FileList.Add(System.IO.Path.GetFileName(file));
            }
        }

        public bool IsFolder(string fileName)
        {
            string file = FileList.Find(s => s == fileName);
            if (file == null)
            {
                return false;
            }

            string filePath = System.IO.Path.Combine(FolderPath, file);
            return System.IO.Directory.Exists(filePath);
        }

        public void Update(out bool isStateChanged)
        {
            SetFolderInfo(FolderPath);
            isStateChanged = true;
        }

        public void MoveFolderOneUp(out bool isStateChanged)
        {
            SetFolderInfo(System.IO.Path.GetDirectoryName(FolderPath));
            isStateChanged = true;
        }

        public void IntoFolder(string selectedName, out bool isStateChanged)
        {
            string selectedPath = System.IO.Path.Combine(FolderPath, selectedName);
            MoveFolder(selectedPath, out isStateChanged);
        }

        public void MoveFolder(string folderPath, out bool isStateChanged)
        {
            isStateChanged = false;
            if (!System.IO.Directory.Exists(folderPath))
            {
                return;
            }

            bool isDirectory = System.IO.File.GetAttributes(folderPath).HasFlag(System.IO.FileAttributes.Directory);
            if (isDirectory)
            {
                SetFolderInfo(folderPath);
                isStateChanged = true;
            }
        }
    }
}
