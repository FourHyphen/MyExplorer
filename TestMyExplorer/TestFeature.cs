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

        [TestMethod]
        public void MoveFolderPathByInputPath()
        {
            // パス入力後のエンターキーによるフォルダ移動
            string folderPath = Common.GetFilePathOfDependentEnvironment(@".\TestData\Folder1");
            Driver.OpenFolder(folderPath);

            // パス入力する＝フォーカスが TextBox に当たっている
            string toPath = Common.GetFilePathOfDependentEnvironment(@".\TestData\Folder2");
            Driver.FocusFolderPathArea();
            Driver.SetFolderPathText(toPath);

            // エンターキー
            Driver.EmurateKey(System.Windows.Input.Key.Enter, System.Windows.Input.ModifierKeys.None);
            Assert.IsTrue(Driver.GetFolderPath().Contains(@"\TestData\Folder2"));
            Assert.IsTrue(Driver.ContainFile("text21.txt"));

            // 存在しないフォルダへは移動せず、画面は変化しない
            Driver.SetFolderPathText("NOT_EXIST");
            Driver.EmurateKey(System.Windows.Input.Key.Enter, System.Windows.Input.ModifierKeys.None);
            Assert.IsTrue(Driver.GetFolderPath().Contains(@"NOT_EXIST"));
            Assert.IsTrue(Driver.ContainFile("text21.txt"));
        }

        [TestMethod]
        public void UpdateFolderInfo()
        {
            // F5 キーによるフォルダの状態更新
            string folderPath = Common.GetFilePathOfDependentEnvironment(@".\TestData");
            string testFileName = @"testfile.txt";
            string testFilePath = System.IO.Path.Combine(folderPath, testFileName);

            // 準備
            DeleteFile(testFilePath);

            Driver.OpenFolder(folderPath);
            Assert.IsFalse(Driver.ContainFile(testFileName));

            // ファイルを作成したばかりでは画面に反映されない
            CreateFile(testFilePath);
            Assert.IsFalse(Driver.ContainFile(testFileName));

            // F5 キーによって状態更新され、作成したファイルが表示される
            Driver.EmurateKey(System.Windows.Input.Key.F5, System.Windows.Input.ModifierKeys.None);
            Assert.IsTrue(Driver.ContainFile(testFileName));

            // 後始末
            DeleteFile(testFilePath);
        }

        private void CreateFile(string filePath)
        {
            System.IO.File.WriteAllText(filePath, "test");
        }

        private void DeleteFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        [TestMethod]
        public void ContainIconThatBackFolder()
        {
            // どのフォルダを表示しても、ファイルリストの先頭に 1 階層上に上るアイコンを表示する
            string folderPath = Common.GetFilePathOfDependentEnvironment(@".\TestData");
            string testFolderName = @"FolderEmpty";
            string testFolderPath = System.IO.Path.Combine(folderPath, testFolderName);

            // 準備
            DeleteFolder(testFolderPath);
            CreateFolder(testFolderPath);

            // ファイルが 1 つも存在しないフォルダの場合
            CreateFolder(testFolderPath);
            Driver.OpenFolder(testFolderPath);
            Assert.IsTrue(Driver.ContainFile(Common.GoBackString));

            string testFileName = "testEmpty.txt";
            string testFilePath = System.IO.Path.Combine(testFolderPath, testFileName);
            CreateFile(testFilePath);

            // フォルダ内にファイルが作成され、状態更新後でも 1 階層上に上るアイコンは表示される
            Driver.EmurateKey(System.Windows.Input.Key.F5, System.Windows.Input.ModifierKeys.None);
            Assert.IsTrue(Driver.ContainFile(Common.GoBackString));
            Assert.IsTrue(Driver.ContainFile(testFileName));

            // 後始末
            DeleteFolder(testFolderPath);
        }

        private void DeleteFolder(string folderPath)
        {
            if (System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.Delete(folderPath, true);
            }
        }

        private void CreateFolder(string folderPath)
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }
    }
}
