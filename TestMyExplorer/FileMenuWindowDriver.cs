using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;

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

        internal void Delete()
        {
            Execute(System.Windows.Input.Key.D5);
        }

        internal void Rename(string afterFileName)
        {
            UpdateWindow();

            // 本処理側が同期処理なのでこっちからは非同期で呼び出す
            var async = new Async();
            Window.AppVar.Dynamic().WindowKeyDowned(async, System.Windows.Input.Key.D6);

            // Rename ウィンドウ内テキストボックスに文字列を流し込む
            var renameWindow = WindowControl.IdentifyFromWindowText(App, "Rename");
            renameWindow.Dynamic().AfterNameTextBox.Text = afterFileName;

            // OK 押して rename 実行
            WPFButtonBase okButton = new WPFButtonBase(renameWindow.Dynamic().OKButton);
            okButton.EmulateClick();

            // 非同期実行なので実行終了を待つ
            async.WaitForCompletion();
        }

        private void Execute(System.Windows.Input.Key key)
        {
            // 本処理側が非同期処理なのでこっちからは同期処理で呼び出してOK
            UpdateWindow();
            Window.AppVar.Dynamic().WindowKeyDowned(key);
        }

        private void UpdateWindow()
        {
            Window = WindowControl.IdentifyFromWindowText(App, "FileMenuWindow");
        }
    }
}
