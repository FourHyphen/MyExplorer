using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyExplorer
{
    public class ExplorerData : INotifyPropertyChanged
    {
        private string _FolderPath = "";
        public string FolderPath
        {
            get => _FolderPath;
            set
            {
                _FolderPath = value;
                NotifyPropertyChanged(nameof(FolderPath));
            }
        }

        private List<string> _FileList = null;
        public List<string> FileList
        {
            get => _FileList;
            set
            {
                _FileList = value;
                //NotifyPropertyChanged(nameof(FileList));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string name)
        {
            var e = new PropertyChangedEventArgs(name);
            PropertyChanged?.Invoke(this, e);
        }

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
