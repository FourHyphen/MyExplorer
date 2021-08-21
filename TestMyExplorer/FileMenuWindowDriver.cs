using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using Codeer.Friendly.Windows.Grasp;
using System;

namespace TestMyExplorer
{
    class FileMenuWindowDriver
    {
        private WindowsAppFriend App { get; }

        private WindowControl Window { get; set; }

        public FileMenuWindowDriver(WindowsAppFriend app)
        {
            App = app;
        }

        internal void Close()
        {
            UpdateWindow();
            Window.Close();
        }

        internal void Zip()
        {
            Execute(System.Windows.Input.Key.D3);
        }

        internal void PathCopy()
        {
            Execute(System.Windows.Input.Key.D4);
        }

        private void Execute(System.Windows.Input.Key key)
        {
            UpdateWindow();
            Window.AppVar.Dynamic().WindowKeyDowned(key);
        }

        private void UpdateWindow()
        {
            Window = WindowControl.IdentifyFromWindowText(App, "FileMenuWindow");
        }
    }
}
