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
        public void OpenTwoFolders()
        {
            string folderPath = Common.GetFilePathOfDependentEnvironment(@".\TestData\Folder1");
            Driver.OpenFolder(folderPath);
            string folderPath2 = Common.GetFilePathOfDependentEnvironment(@".\TestData\Folder2");
            Driver.OpenFolder(folderPath2);

            Assert.IsTrue(Driver.GetFolderPath(0).Contains(@"\TestData\Folder1"));
            Assert.IsTrue(Driver.ContainFile("text11.txt", 0));
            Assert.IsTrue(Driver.ContainFile("text12.txt", 0));
            Assert.IsTrue(Driver.ContainFolder("folder01", 0));

            Assert.IsTrue(Driver.GetFolderPath(1).Contains(@"\TestData\Folder2"));
            Assert.IsTrue(Driver.ContainFile("text21.txt", 1));
            Assert.IsTrue(Driver.ContainFile("text22.txt", 1));
            Assert.IsTrue(Driver.ContainFolder("folder02", 1));
        }

        [TestMethod]
        public void MoveFolderPathByKey()
        {
            // Alt + 左右 による現在フォルダの移動
            System.Windows.Input.ModifierKeys altKey = System.Windows.Input.ModifierKeys.Alt;

            string folderPath = Common.GetFilePathOfDependentEnvironment(@".\TestData\Folder1");
            Driver.OpenFolder(folderPath);
            Driver.EmurateKey(System.Windows.Input.Key.Left, altKey);

            Assert.IsFalse(Driver.GetFolderPath(0).Contains(@"\TestData\Folder1"));
            Assert.IsTrue(Driver.GetFolderPath(0).Contains(@"\TestData"));
            Assert.IsTrue(Driver.ContainFolder("Folder1", 0));
            Assert.IsTrue(Driver.ContainFolder("Folder2", 0));

            Driver.EmurateKey(System.Windows.Input.Key.Left, altKey);
            Assert.IsFalse(Driver.GetFolderPath(0).Contains(@"\TestData"));
            Assert.IsTrue(Driver.ContainFolder("TestData", 0));

            Driver.EmurateKey(System.Windows.Input.Key.Right, altKey);
            Assert.IsFalse(Driver.GetFolderPath(0).Contains(@"\TestData\Folder1"));
            Assert.IsTrue(Driver.GetFolderPath(0).Contains(@"\TestData"));

            Driver.EmurateKey(System.Windows.Input.Key.Right, altKey);
            Assert.IsTrue(Driver.GetFolderPath(0).Contains(@"\TestData\Folder1"));
        }
    }
}
