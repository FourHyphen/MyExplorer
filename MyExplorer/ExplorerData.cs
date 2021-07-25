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
            FolderPath = folderPath;
            InitFileList(folderPath);
        }

        private void InitFileList(string folderPath)
        {
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
    }
}
