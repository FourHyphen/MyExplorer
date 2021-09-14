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
using System.Windows.Shapes;

namespace MyExplorer
{
    public partial class FileRenameWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string NowName { get; private set; }

        public string AfterName { get; private set; }

        private string NowPath { get; set; }

        public FileRenameWindow(string nowPath)
        {
            InitializeComponent();
            DataContext = this;
            Init(nowPath);
        }

        private void Init(string nowPath)
        {
            NowPath = nowPath;
            InitText();
            AfterNameTextBox.Focus();
        }

        private void InitText()
        {
            NowName = System.IO.Path.GetFileName(NowPath);
            AfterName = (string)NowName.Clone();
            NotifyPropertyChanged(nameof(NowName));
            NotifyPropertyChanged(nameof(AfterName));
        }

        private void OKButtonClicked(object sender, RoutedEventArgs e)
        {
            Rename();
            Close();
        }

        private void Rename()
        {
            if (Common.IsFolder(NowPath))
            {
                RenameFolder();
            }
            else
            {
                RenameFile();
            }
        }

        private void RenameFolder()
        {
            RenameCore("System.IO.Directory");
        }

        private void RenameFile()
        {
            RenameCore("System.IO.File");
        }

        private void RenameCore(string moveFuncClassName)
        {
            string dirPath = System.IO.Path.GetDirectoryName(NowPath);
            string toPath = System.IO.Path.Combine(dirPath, AfterName);

            MethodInfo mi = Type.GetType(moveFuncClassName).GetMethod("Move");
            mi.Invoke(null, new object[] { NowPath, toPath });
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NotifyPropertyChanged(string name)
        {
            var e = new PropertyChangedEventArgs(name);
            PropertyChanged?.Invoke(this, e);
        }
    }
}
