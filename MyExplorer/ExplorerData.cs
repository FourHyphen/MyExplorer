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
            AddFiles(folderPath, System.IO.Directory.GetDirectories);
            AddFiles(folderPath, System.IO.Directory.GetFiles);
        }

        private void AddFiles(string folderPath, Func<string, string[]> func)
        {
            string[] files = func(folderPath);
            foreach (string file in files)
            {
                FileList.Add(System.IO.Path.GetFileName(file));
            }
        }

        public void MoveFolderOneUp()
        {
            SetFolderInfo(System.IO.Path.GetDirectoryName(FolderPath));
        }

        public void IntoFolder(string selectedName)
        {
            string selectedPath = System.IO.Path.Combine(FolderPath, selectedName);
            bool isDirectory = System.IO.File.GetAttributes(selectedPath).HasFlag(System.IO.FileAttributes.Directory);
            if (isDirectory)
            {
                SetFolderInfo(selectedPath);
            }
        }
    }
}
