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
    public partial class Explorer : UserControl
    {
        public ExplorerData ExplorerData { get; set; }

        public Explorer(string folderPath, int row, int col)
        {
            InitializeComponent();
            Init(folderPath, row, col);
        }

        private void Init(string folderPath, int row, int col)
        {
            ExplorerData = new ExplorerData(folderPath);
            DataContext = ExplorerData;
            SetElementsName(row, col);
        }

        private void SetElementsName(int row, int col)
        {
            string unique = row.ToString() + col.ToString();
            Folder.Name = "Folder" + unique;
            FileList.Name = "FileList" + unique;
        }
    }
}
