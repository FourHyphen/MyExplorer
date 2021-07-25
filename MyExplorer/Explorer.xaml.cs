using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyExplorer
{
    public partial class Explorer : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string name)
        {
            var e = new PropertyChangedEventArgs(name);
            PropertyChanged?.Invoke(this, e);
        }

        public int CanvasWidth { get; private set; }

        public int CanvasHeight { get; private set; }

        public string FolderPath { get; set; } = "";

        private List<string> _FileList = null;
        public List<string> FileList
        {
            get => _FileList;
            set
            {
                _FileList = value;
                NotifyPropertyChanged(nameof(FileList));
            }
        }

        public Explorer(string folderPath, int index)
        {
            InitializeComponent();
            Init(folderPath, index);
        }

        private void Init(string folderPath, int index)
        {
            DataContext = this;
            InitFolderInfo(folderPath);
            SetElementsName(index);
        }

        private void InitFolderInfo(string folderPath)
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

        private void SetElementsName(int index)
        {
            string unique = index.ToString();
            FolderArea.Name = "FolderArea" + unique;
            Folder.Name = "Folder" + unique;
            FolderFileList.Name = "FileList" + unique;
        }

        public void SetWidth(int width)
        {
            FolderArea.Width = width;
            Folder.Width = width;
            FolderFileList.Width = width;
            CanvasWidth = width;
        }

        public void SetHeight(int height)
        {
            FolderArea.Height = height;
            Folder.Height = 40;
            FolderFileList.Height = height - Folder.Height;
            CanvasHeight = height;
        }
    }
}
