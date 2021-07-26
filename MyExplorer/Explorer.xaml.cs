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
    public partial class Explorer : UserControl
    {
        public ExplorerData Data { get; private set; } = null;

        public int CanvasLeft { get; private set; }

        public int CanvasTop { get; private set; }

        public int CanvasWidth { get; private set; }

        public int CanvasHeight { get; private set; }

        public Explorer(string folderPath, int index)
        {
            InitializeComponent();
            Init(folderPath, index);
        }

        private void Init(string folderPath, int index)
        {
            Data = new ExplorerData(folderPath);
            DataContext = this;
            SetElementsName(index);
        }

        private void SetElementsName(int index)
        {
            string unique = index.ToString();
            FolderArea.Name = "FolderArea" + unique;
            Folder.Name = "Folder" + unique;
            FolderFileList.Name = "FileList" + unique;
        }

        public void SetLeft(int left)
        {
            CanvasLeft = left;
            Canvas.SetLeft(this, left);
        }

        public void SetTop(int top)
        {
            CanvasTop = top;
            Canvas.SetTop(this, top);
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
