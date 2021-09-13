using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace MyExplorer
{
    public partial class FileRenameWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string NowName { get; private set; }

        public string AfterName { get; private set; }

        public FileRenameWindow(string nowName)
        {
            InitializeComponent();
            Init(nowName);
        }

        private void Init(string nowName)
        {
            NowName = nowName;
            NotifyPropertyChanged(nameof(NowName));
        }

        private void OKButtonClicked(object sender, RoutedEventArgs e)
        {
            Close();
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
