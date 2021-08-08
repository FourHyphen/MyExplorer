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

        public object SelectedItem { get; set; }

        public int FolderAreaWidth { get; private set; }

        public int FolderAreaHeight { get; private set; }

        public int FolderPathAreaWidth { get; private set; }

        public int FolderPathAreaHeight { get; private set; }

        public int FolderFileListAreaWidth { get; private set; }

        public int FolderFileListAreaHeight { get; private set; }

        public Explorer(string folderPath)
        {
            InitializeComponent();
            Init(folderPath);
        }

        private void Init(string folderPath)
        {
            Data = new ExplorerData(folderPath);
            DataContext = this;
        }

        public void SetPosition(MainWindow main)
        {
            int scrollBar = 10;                     // 10: 経験則
            int width = (int)main.ActualWidth - scrollBar;
            int height = (int)main.ActualHeight - scrollBar - 50;    // 50: 経験則

            SetWidth(width);
            SetHeight(height);
        }

        private void SetWidth(int width)
        {
            FolderPathAreaWidth = width;
            FolderFileListAreaWidth = width;
            FolderAreaWidth = width;
        }

        private void SetHeight(int height)
        {
            FolderPathAreaHeight = 40;
            FolderFileListAreaHeight = height - FolderPathAreaHeight;
            FolderAreaHeight = height;
        }

        public void Display(UIElementCollection collection)
        {
            collection.Clear();
            collection.Add(this);
        }

        public void DoKeyEvent(Keys.KeyEventType keyEventType)
        {
            ExplorerCommand ec = ExplorerCommandFactory.Create(this, keyEventType);
            DoEventCore(ec);
        }

        public void DoMouseEvent(dynamic obj)
        {
            ExplorerCommand ec = ExplorerCommandFactory.Create(this, obj);
            DoEventCore(ec);
        }

        private void DoEventCore(ExplorerCommand ec)
        {
            ec.Execute();
            if (ec.IsDataChanged)
            {
                NotifyPropertyChanged(nameof(Data));
            }
            if (ec.IsSelectedItemChanged)
            {
                NotifyPropertyChanged(nameof(SelectedItem));
            }
        }

        public void NotifyPropertyChanged(string name)
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
        /// テストで使用
        /// </summary>
        public void FocusFolderPathArea()
        {
            FolderPath.Focus();
            Keyboard.Focus(FolderPath);
        }

        /// <summary>
        /// テストで使用
        /// </summary>
        /// <param name="fileName"></param>
        public void FocusFile(string fileName)
        {
            ListViewItem selected = GetListViewItem(fileName);
            if (selected != null)
            {
                Keyboard.Focus(selected);
                selected.Focus();
            }
        }

        /// <summary>
        /// テストで使用
        /// </summary>
        /// <param name="str"></param>
        public void SetFolderPathText(string str)
        {
            FolderPath.Text = str;
        }
    }
}
