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
        private ExplorerList Explorers { get; set; } = new ExplorerList();

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
            Explorers.Add(folderPath);
            Explorers.SetPosition(this);
            Explorers.Display(FoldersArea.Children);
        }

        private void MainWindowKeyDown(object sender, KeyEventArgs e)
        {
            InputKey(e.Key, e.SystemKey, e.KeyboardDevice.Modifiers);
        }

        private void InputKey(Key key, Key systemKey, ModifierKeys modifier)
        {
            Keys.KeyEventType keyEventType = Keys.ToKeyEventType(key, systemKey, modifier);
            if (keyEventType != Keys.KeyEventType.Else)
            {
                Explorers.DoKeyEvent(keyEventType);
            }
        }

        /// <summary>
        /// テストでのみ使用
        /// </summary>
        /// <param name="fileName"></param>
        private void FocusFile(string fileName)
        {
            Explorers.FocusFile(fileName);
        }
    }
}
