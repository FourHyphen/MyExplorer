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

        private void KeyDowned(object sender, KeyEventArgs e)
        {
            KeyDowned(e.Key, e.SystemKey, e.KeyboardDevice.Modifiers);
        }

        private void KeyDowned(Key key, Key systemKey, ModifierKeys modifier)
        {
            Keys.KeyEventType keyEventType = Keys.ToKeyEventType(key, systemKey, modifier);
            if (keyEventType != Keys.KeyEventType.Else)
            {
                DoKeyEvent(keyEventType);
            }
        }

        public void DoKeyEvent(Keys.KeyEventType keyEventType)
        {
            ExplorerCommand ec = ExplorerCommandFactory.Create(this, keyEventType);
            DoEventCore(ec);
        }

        private void MouseLeftButtonDownClicked(object sender, MouseButtonEventArgs e)
        {
            MouseLeftButtonDownClicked(e.GetPosition((UIElement)sender));
        }

        private void MouseLeftButtonDownClicked(Point p)
        {
            HitTestResult result = VisualTreeHelper.HitTest(this, p);
            dynamic obj = result.VisualHit;
            DoLeftMouseEvent(obj);
        }

        public void DoLeftMouseEvent(dynamic obj)
        {
            ExplorerCommand ec = ExplorerCommandFactory.CreateLeftButtonEvent(this, obj);
            DoEventCore(ec);
        }

        private void MouseDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            DoMouseDoubleClickEvent();
        }

        public void DoMouseDoubleClickEvent()
        {
            ExplorerCommand ec = ExplorerCommandFactory.CreateDoubleClickEvent(this);
            DoEventCore(ec);
        }

        private void FolderFileListItemMouseRightButtonClicked(object sender, MouseButtonEventArgs e)
        {
            // ファイル選択中に別ファイルに対して右クリックした際、選択中ファイルを変える
            if (sender is ListViewItem lvi && lvi.Content is ExplorerFileInfo eventItem)
            {
                ExplorerFileInfo now = (ExplorerFileInfo)FolderFileList.SelectedItem;
                // TODO: バグ: ファイル無選択の場合、now が null になって null 参照例外起こす
                if (eventItem.Name != now.Name)
                {
                    SelectedItem = eventItem;
                    NotifyPropertyChanged(nameof(SelectedItem));
                }
            }

            FolderFileListItemMouseRightButtonClicked((ExplorerFileInfo)FolderFileList.SelectedItem);
        }

        private void FolderFileListItemMouseRightButtonClicked(ExplorerFileInfo efi)
        {
            DoRightMouseEvent(efi);
        }

        private void DoRightMouseEvent(ExplorerFileInfo efi)
        {
            ExplorerCommand ec = ExplorerCommandFactory.CreateRightButtonEvent(this, efi);
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

        private void NotifyPropertyChanged(string name)
        {
            var e = new PropertyChangedEventArgs(name);
            PropertyChanged?.Invoke(this, e);
        }

        public void DataChanged()
        {
            NotifyPropertyChanged(nameof(Data));
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
