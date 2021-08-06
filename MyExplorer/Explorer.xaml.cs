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
            ec.Execute(out bool isStateChanged);

            if (isStateChanged)
            {
                NotifyDataChanged();
            }
        }

        public void DoMouseEvent(dynamic obj)
        {
            ExplorerCommand ec = ExplorerCommandFactory.Create(this, obj);
            ec.Execute(out bool isStateChanged);

            if (isStateChanged)
            {
                NotifySelectedItemChanged();
            }
        }

        private void NotifyDataChanged()
        {
            NotifyPropertyChanged(nameof(Data));
        }

        private void NotifySelectedItemChanged()
        {
            NotifyPropertyChanged(nameof(SelectedItem));
        }

        public void NotifyPropertyChanged(string name)
        {
            var e = new PropertyChangedEventArgs(name);
            PropertyChanged?.Invoke(this, e);
        }

        public bool IsItemInFileList(string itemPropertyName)
        {
            // 参考: https://threeshark3.com/binding-listbox-focus/
            for (int i = 0; i < FolderFileList.Items.Count; i++)
            {
                var obj = GetItemInFolderFileList(i);
                if (obj is ListViewItem target)
                {
                    PropertyInfo pi = typeof(ListViewItem).GetProperty(itemPropertyName);
                    if ((bool)pi.GetValue(target))
                    {
                        return true;
                    }
                }
            }

            return false;
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
                    if ((string)target.Content == content)
                    {
                        return target;
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
