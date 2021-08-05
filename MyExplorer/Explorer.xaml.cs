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

        public object SelectedItem { get; private set; }

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
            bool isStateChanged = false;
            if (keyEventType == Keys.KeyEventType.EnterKey)
            {
                DoEnterKeyEvent(out isStateChanged);
            }
            else if (keyEventType == Keys.KeyEventType.Update)
            {
                UpdateFolder(out isStateChanged);
            }
            else if (IsItemInFileList("IsFocused"))
            {
                if (keyEventType == Keys.KeyEventType.FolderBack)
                {
                    BackFolder(out isStateChanged);
                }
                else if (keyEventType == Keys.KeyEventType.FolderForward)
                {
                    ForwardFolder(out isStateChanged);
                }
            }

            if (isStateChanged)
            {
                NotifyDataChanged();
            }
        }

        private void DoEnterKeyEvent(out bool isStateChanged)
        {
            isStateChanged = false;
            if (FolderPath.IsFocused)
            {
                MoveFolder(out isStateChanged);
            }
            else
            {
                ListViewItem top = GetListViewItem(Common.MoveOneUpFolderString);
                if (top.IsFocused)
                {
                    BackFolder(out isStateChanged);
                }
            }
        }

        private void MoveFolder(out bool isStateChanged)
        {
            string input = FolderPath.Text;
            Data.MoveFolder(input, out isStateChanged);
        }

        private void UpdateFolder(out bool isStateChanged)
        {
            Data.Update(out isStateChanged);
        }

        private bool IsItemInFileList(string itemPropertyName)
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

        private void BackFolder(out bool isStateChanged)
        {
            Data.MoveFolderOneUp(out isStateChanged);
        }

        private void ForwardFolder(out bool isStateChanged)
        {
            isStateChanged = false;

            string selectedName = GetFolderFileListSelected();
            if (selectedName != "")
            {
                Data.IntoFolder(selectedName, out isStateChanged);
            }
        }

        private string GetFolderFileListSelected()
        {
            object selected = FolderFileList.SelectedItem;
            return selected != null ? selected.ToString() : "";
        }

        public void DoMouseEvent(dynamic obj)
        {
            if (DoMouseDownFileList(obj))
            {
                DoMouseEventFileList();
            }
        }

        private bool DoMouseDownFileList(dynamic obj)
        {
            if (obj is ScrollViewer sv)
            {
                // Item が選択されている場合は false 判定になる
                return (sv.TemplatedParent is ListView);
            }

            return false;
        }

        private void DoMouseEventFileList()
        {
            if (IsItemInFileList("IsFocused"))
            {
                // Item の Selected を解除
                SelectedItem = null;
            }
            else
            {
                // FileList にフォーカスを当てる
                SetFocusFileList();
            }

            NotifySelectedItemChanged();
        }

        private void SetFocusFileList()
        {
            if (SelectedItem == null)
            {
                // 真に何も選択されていない場合、FileList にキーボードフォーカスと論理フォーカスを当てる
                FolderFileList.Focus();
                Keyboard.Focus(FolderFileList);
            }

            if (SelectedItem is string itemString)
            {
                // 選択されている状態でフォーカスが ListView から外れ、再度 ListView にフォーカス当たった場合にここを通る
                // この場合はファイルにフォーカスを当てなおす
                ListViewItem item = GetListViewItem(itemString);
                if (item != null)
                {
                    SetFocusFile(item);
                }
            }
        }

        private ListViewItem GetListViewItem(string content)
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

        private void SetFocusFile(ListViewItem selected)
        {
            Keyboard.Focus(selected);
            selected.Focus();
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
                SetFocusFile(selected);
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
