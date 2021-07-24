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

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string name)
        {
            var e = new PropertyChangedEventArgs(name);
            PropertyChanged?.Invoke(this, e);
        }

        public ExplorerData(string folderPath)
        {
            FolderPath = folderPath;
        }

        //private void InitFileList(string folderPath)
        //{
        //    string[] files = System.IO.Directory.GetFiles(folderPath);
        //    foreach (string file in files)
        //    {
        //        FileList.Items.Add(System.IO.Path.GetFileName(file));
        //    }
        //}
    }
}
