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
            FileExecutes.Add(new FileExecuteOpen("開く", filePath));
            FileExecutes.Add(new FileExecuteShowProgram("プログラムから開く", filePath));
            //FileExecutes.Add(new FileExecuteZip("zip圧縮", filePath);
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

        private void KeyDowned(object sender, KeyEventArgs e)
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
