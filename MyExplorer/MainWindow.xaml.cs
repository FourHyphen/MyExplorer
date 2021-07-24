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
        private int Already = -1;

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
            if (Already < 0)
            {
                OpenFolder(folderPath, 0, 0);
            }
            else if (Already == 0)
            {
                OpenFolder(folderPath, 0, 1);
            }
            else if (Already == 1)
            {
                OpenFolder(folderPath, 1, 0);
            }
            else if (Already == 2)
            {
                OpenFolder(folderPath, 1, 1);
            }

            Already++;
        }

        private void OpenFolder(string folderPath, int row, int col)
        {
            Explorer ex = new Explorer(folderPath, row, col);
            FoldersArea.Children.Add(ex);
        }
    }
}
