using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyExplorer
{
    /// <remarks>
    /// Partial: ExplorerDisplay / ExplorerEvent / ExplorerProcessItemList
    /// </remarks>
    public partial class Explorer : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ExplorerData Data { get; private set; }

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

            SetPosition((int)main.ActualWidth, (int)main.ActualHeight);
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
