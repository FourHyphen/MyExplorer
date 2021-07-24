using System;
using System.Collections.Generic;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuOpenFolderClick(object sender, RoutedEventArgs e)
        {
            string folderPath = DialogOpenFolder.Show();
            if (folderPath != null)
            {
                OpenFolder(folderPath);
            }
        }

        private void OpenFolder(string folderPath)
        {
            // TODO: どのウィンドウにオープンするかを現在の状態から決定する
            OpenFolder(folderPath, 0, 0);
        }

        private void OpenFolder(string folderPath, int? row, int? col)
        {
            Explorer ex = new Explorer(folderPath);
            FoldersArea.Children.Add(ex);
        }
    }
}
