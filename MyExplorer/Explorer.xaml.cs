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

        public ExplorerData Data { get; private set; }

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

        public bool NowFocusing()
        {
            foreach (UIElement elem in FolderArea.Children)
            {
                // TextBox は IsFocused() が true / false になる
                if (elem.IsFocused)
                {
                    return true;
                }

                // ListBox の判定
                //if (elem.GetType().FullName.Contains("ListBox"))
                if (elem is ListBox)
                {
                    if (IsFocusedListBox((ListBox)elem))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsFocusedListBox(ListBox listBox)
        {
            // ListBox 自体は focus を持たないため、中の ListBoxItem が focus を持っているかを判定する
            // 参考: https://threeshark3.com/binding-listbox-focus/
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                var obj = listBox.ItemContainerGenerator.ContainerFromIndex(i);
                if (obj is ListBoxItem target)
                {
                    if (target.IsFocused)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void DoKeyEvent(Keys.KeyEventType keyEventType)
        {
            if (keyEventType == Keys.KeyEventType.FolderBack)
            {
                BackFolder();
            }
            else if (keyEventType == Keys.KeyEventType.FolderForward)
            {
                ForwardFolder();
            }

            NotifyExplorerDataChanged();
        }

        private void BackFolder()
        {
            Data.MoveFolderOneUp();
        }

        private void ForwardFolder()
        {
            string selectedName = GetFolderFileListSelected();
            if (selectedName != "")
            {
                Data.IntoFolder(selectedName);
            }
        }

        private string GetFolderFileListSelected()
        {
            object selected = FolderFileList.SelectedItem;
            if (selected != null)
            {
                return selected.ToString();
            }

            return "";
        }

        private void NotifyExplorerDataChanged()
        {
            NotifyPropertyChanged(nameof(Data));
        }

        public void NotifyPropertyChanged(string name)
        {
            var e = new PropertyChangedEventArgs(name);
            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// テストでのみ使用
        /// </summary>
        /// <param name="fileName"></param>
        public void FocusFile(string fileName)
        {
            int index = Data.FileList.FindIndex(f => f == fileName);
            FolderFileList.SelectedIndex = index;
            FolderFileList.Focus();
        }
    }
}
