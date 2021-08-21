using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyExplorer
{
    public partial class FileMenuWindow : Window
    {
        public List<FileExecute> FileExecutes { get; private set; }

        public FileMenuWindow(string filePath)
        {
            InitializeComponent();
            DataContext = this;

            InitMenu(filePath);
        }

        private void InitMenu(string filePath)
        {
            FileExecutes = new List<FileExecute>();
            FileExecutes.Add(new FileExecuteOpen(1, "開く", filePath));
            FileExecutes.Add(new FileExecuteShowProgram(2, "プログラムから開く", filePath));
            FileExecutes.Add(new FileExecuteZip(3, "zip圧縮", filePath));
            FileExecutes.Add(new FileExecuteSetClipboard(4, "パスコピー", filePath));
        }

        public void SetFocus()
        {
            FileMenuList.Focus();
            Keyboard.Focus(FileMenuList);
        }

        private void MouseLeftButtonDownClicked(object sender, MouseButtonEventArgs e)
        {
            ListViewItem selected = (ListViewItem)sender;
            if (selected.Content is FileExecute fe)
            {
                Execute(fe);
            }
        }

        private void WindowKeyDowned(object sender, KeyEventArgs e)
        {
            WindowKeyDowned(e.Key);
        }

        private void WindowKeyDowned(Key key)
        {
            int? num = ToInt(key);
            if (num != null)
            {
                Execute(FileExecutes[(int)num - 1]);    // 1 始まりを 0 始まりに変換
            }
        }

        private int? ToInt(Key key)
        {
            if (key == Key.NumPad1 || key == Key.D1)
            {
                return 1;
            }
            else if (key == Key.NumPad2 || key == Key.D2)
            {
                return 2;
            }
            else if (key == Key.NumPad3 || key == Key.D3)
            {
                return 3;
            }
            else if (key == Key.NumPad4 || key == Key.D4)
            {
                return 4;
            }
            else if (key == Key.NumPad5 || key == Key.D5)
            {
                return 5;
            }
            else if (key == Key.NumPad6 || key == Key.D6)
            {
                return 6;
            }
            else if (key == Key.NumPad7 || key == Key.D7)
            {
                return 7;
            }
            else if (key == Key.NumPad8|| key == Key.D8)
            {
                return 8;
            }
            else if (key == Key.NumPad9 || key == Key.D9)
            {
                return 9;
            }
            else
            {
                return null;
            }
        }

        private void FileMenuItemKeyDowned(object sender, KeyEventArgs e)
        {
            //KeyDowned(e.Key, e.SystemKey, e.KeyboardDevice.Modifiers);
            if (e.Key == Key.Enter)
            {
                Execute((FileExecute)FileMenuList.SelectedItem);
            }
        }

        private void Execute(FileExecute fe)
        {
            fe.Execute();
            Close();
        }
    }
}
