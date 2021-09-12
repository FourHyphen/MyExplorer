using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

        public ExplorerDisplay Display { get; private set; }

        public object SelectedItem { get; set; }

        public Explorer(string folderPath, MainWindow main)
        {
            InitializeComponent();
            Init(folderPath, main);
        }

        private void Init(string folderPath, MainWindow main)
        {
            DataContext = this;

            Data = new ExplorerData(folderPath);
            Display = new ExplorerDisplay((int)main.ActualWidth, (int)main.ActualHeight);

            main.FoldersArea.Children.Clear();
            main.FoldersArea.Children.Add(this);
        }

        public void MoveFolderOneUp()
        {
            Data.MoveFolderOneUp(out bool isStateChanged);
            if (isStateChanged)
            {
                DataChanged();
            }
        }

        public void IntoFolder(ExplorerFileInfo selectedItem)
        {
            Data.IntoFolder(selectedItem, out bool isStateChanged);
            if (isStateChanged)
            {
                DataChanged();
            }
        }

        public void MoveFolder(string input)
        {
            Data.MoveFolder(input, out bool isStateChanged);
            if (isStateChanged)
            {
                DataChanged();
            }
        }

        public void Update()
        {
            Data.Update(out bool isStateChanged);
            if (isStateChanged)
            {
                DataChanged();
            }
        }

        private void DataChanged()
        {
            NotifyPropertyChanged(nameof(Data));
        }

        public void SelectedItemChanged()
        {
            NotifyPropertyChanged(nameof(SelectedItem));
        }

        private void NotifyPropertyChanged(string name)
        {
            var e = new PropertyChangedEventArgs(name);
            PropertyChanged?.Invoke(this, e);
        }

        public bool IsFocusedItemInFileList()
        {
            ListViewItem item = GetListViewItemFocused();
            return item != null;
        }

        public ListViewItem GetListViewItemFocused()
        {
            // 参考: https://threeshark3.com/binding-listbox-focus/
            for (int i = 0; i < FolderFileList.Items.Count; i++)
            {
                var obj = GetItemInFolderFileList(i);
                if (obj is ListViewItem target)
                {
                    if (target.IsFocused)
                    {
                        return target;
                    }
                }
            }

            return null;
        }

        private object GetItemInFolderFileList(int index)
        {
            return FolderFileList.ItemContainerGenerator.ContainerFromIndex(index);
        }

        public ListViewItem GetListViewItem(string content)
        {
            for (int i = 0; i < FolderFileList.Items.Count; i++)
            {
                var obj = GetItemInFolderFileList(i);
                if (obj is ListViewItem target)
                {
                    if (target.Content is ExplorerFileInfo efi)
                    {
                        if (efi.Name == content)
                        {
                            return target;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// テストから直接呼び出す
        /// </summary>
        private void FocusFolderPathArea()
        {
            FolderPath.Focus();
            Keyboard.Focus(FolderPath);
        }

        /// <summary>
        /// テストから直接呼び出す
        /// </summary>
        /// <param name="fileName"></param>
        private void FocusFile(string fileName)
        {
            ListViewItem selected = GetListViewItem(fileName);
            if (selected != null)
            {
                SelectedItem = Data.FileList.Find(s => s.Name == fileName);
                Keyboard.Focus(selected);
                selected.Focus();
                NotifyPropertyChanged(nameof(Data));
                NotifyPropertyChanged(nameof(SelectedItem));
            }
        }
    }
}
