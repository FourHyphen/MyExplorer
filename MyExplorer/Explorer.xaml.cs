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
                    SelectedItemChanged();
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
