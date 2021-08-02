using System;
using System.Collections.Generic;
using System.Diagnostics;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMyExplorer
{
    [TestClass]
    public class TestFeature
    {
        // 必要なパッケージ
        //  -> Codeer.Friendly
        //  -> Codeer.Friendly.Windows         -> WindowsAppFriend()
        //  -> Codeer.Friendly.Windows.Grasp   -> WindowControl()
        //  -> RM.Friendly.WPFStandardControls -> 各種WPFコントロールを取得するために必要
        // 必要な作業
        //  -> MyExplorer プロジェクトを参照に追加

        private string AttachExeName = "MyExplorer.exe";
        private WindowsAppFriend TestApp;
        private Process TestProcess;
        private dynamic MainWindow;
        private MainWindowDriver Driver;

        private string BeforeEnvironment { get; set; }

        [TestInitialize]
        public void Init()
        {
            // MainWindowプロセスにattach
            string exePath = System.IO.Path.GetFullPath(AttachExeName);
            TestApp = new WindowsAppFriend(Process.Start(exePath));
            TestProcess = Process.GetProcessById(TestApp.ProcessId);
            MainWindow = TestApp.Type("System.Windows.Application").Current.MainWindow;

            Driver = new MainWindowDriver(MainWindow);

            BeforeEnvironment = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Common.GetEnvironmentDirPath();
        }

        [TestCleanup]
        public void Cleanup()
        {
            TestApp.Dispose();
            TestProcess.CloseMainWindow();
            TestProcess.Dispose();

            Environment.CurrentDirectory = BeforeEnvironment;
        }

        [TestMethod]
        public void OpenFolder()
        {
            string folderPath = Common.GetFilePathOfDependentEnvironment(@".\TestData\Folder1");
            Driver.OpenFolder(folderPath);
            Assert.IsTrue(Driver.GetFolderPath().Contains(@"\TestData\Folder1"));
            Assert.IsTrue(Driver.ContainFile("folder01"));
            Assert.IsTrue(Driver.ContainFile("text11.txt"));
            Assert.IsTrue(Driver.ContainFile("text12.txt"));

            string folderPath2 = Common.GetFilePathOfDependentEnvironment(@".\TestData\Folder2");
            Driver.OpenFolder(folderPath2);

            Assert.IsTrue(Driver.GetFolderPath().Contains(@"\TestData\Folder2"));
            Assert.IsTrue(Driver.ContainFile("folder02"));
            Assert.IsTrue(Driver.ContainFile("text21.txt"));
            Assert.IsTrue(Driver.ContainFile("text22.txt"));
        }

        [TestMethod]
        public void BackFolderPathByKey()
        {
            // 左キーによる現在フォルダから 1 階層上への移動
            System.Windows.Input.ModifierKeys modifierNone = System.Windows.Input.ModifierKeys.None;

            string folderPath = Common.GetFilePathOfDependentEnvironment(@".\TestData\Folder1");
            Driver.OpenFolder(folderPath);

            // フォーカスが TextBox に当たっている場合はフォルダ移動しない
            Driver.FocusFolderPathArea();
            Driver.EmurateKey(System.Windows.Input.Key.Left, modifierNone);
            Assert.IsTrue(Driver.GetFolderPath().Contains(@"\TestData\Folder1"));

            // 1 階層上に上る
            Driver.FocusFile("text11.txt");
            Driver.EmurateKey(System.Windows.Input.Key.Left, modifierNone);
            Assert.IsFalse(Driver.GetFolderPath().Contains(@"\TestData\Folder1"));
            Assert.IsTrue(Driver.ContainFile("Folder1"));
            Assert.IsTrue(Driver.ContainFile("Folder2"));
        }
    }
}
