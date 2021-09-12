using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyExplorer;

namespace TestMyExplorer
{
    [TestClass]
    public class TestExplorer
    {
        private string BeforeEnvironment { get; set; }

        [TestInitialize]
        public void Init()
        {
            BeforeEnvironment = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Common.GetEnvironmentDirPath();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Environment.CurrentDirectory = BeforeEnvironment;
        }

        [TestMethod]
        public void CreateExplorer()
        {
            string folderPath = Common.GetFilePathOfDependentEnvironment(@"./TestData/Folder2");
            Explorer explorer = new Explorer(folderPath);
            Assert.IsTrue(explorer.Data.FolderPath.Contains(@"TestData\Folder2"));

            // [0] -> 1 階層上に上るアイコン
            Assert.IsTrue(System.IO.Path.GetFileName(explorer.Data.FileList[1].Name) == "folder02");
            Assert.IsTrue(System.IO.Path.GetFileName(explorer.Data.FileList[2].Name) == "text21.txt");
            Assert.IsTrue(System.IO.Path.GetFileName(explorer.Data.FileList[3].Name) == "text22.txt");
        }

        [TestMethod]
        public void ForwardFolder()
        {
            string folderPath = Common.GetFilePathOfDependentEnvironment(@"./TestData/");
            ExplorerFileInfo efi = new ExplorerFileInfo(System.IO.Path.Combine(folderPath, "Folder2"));

            ExplorerData explorer = new ExplorerData(folderPath);
            explorer.IntoFolder(efi, out bool isStateChanged);
            Assert.IsTrue(isStateChanged);
            Assert.IsTrue(explorer.FolderPath.Contains(@"TestData\Folder2"));
            Assert.AreEqual(expected: 4, actual: explorer.FileList.Count);

            efi = new ExplorerFileInfo(System.IO.Path.Combine(efi.FullPath, "folder02"));
            explorer.IntoFolder(efi, out isStateChanged);
            Assert.IsTrue(isStateChanged);
            Assert.IsTrue(explorer.FolderPath.Contains(@"TestData\Folder2\folder02"));
            Assert.AreEqual(expected: 1, actual: explorer.FileList.Count);
        }

        [TestMethod]
        public void MoveOneUpFolder()
        {
            // 1 階層上に上るテスト
            // 1 階層上に上るアイコンを実行したとき、←キー押下時が該当
            string folderPath = Common.GetFilePathOfDependentEnvironment(@"./TestData/Folder1/folder01");
            ExplorerData explorer = new ExplorerData(folderPath);
            explorer.MoveFolderOneUp(out bool tmp);
            Assert.IsFalse(explorer.FolderPath.Contains(@"folder01"));
            Assert.IsTrue(explorer.FolderPath.Contains(@"TestData\Folder1"));
        }

        [TestMethod]
        public void IsFolder()
        {
            string folderPath = Common.GetFilePathOfDependentEnvironment(@"./TestData/Folder1");
            ExplorerData explorer = new ExplorerData(folderPath);
            Assert.IsTrue(explorer.IsFolder("folder01"));
            Assert.IsFalse(explorer.IsFolder("text11.txt"));
            Assert.IsFalse(explorer.IsFolder("NOT_EXIST"));
        }

        [TestMethod]
        public void GetFileMenu()
        {
            string filePath = Common.GetFilePathOfDependentEnvironment(@"./TestData/Folder1/text11.txt");

            FileMenuWindow fmw = new FileMenuWindow(filePath);
            Assert.IsTrue(fmw.FileExecutes.Find(s => s.Name.Contains("開く")) != null);
            Assert.IsTrue(fmw.FileExecutes.Find(s => s.Name.Contains("プログラムから開く")) != null);
        }

        [TestMethod]
        public void CreateExplorerCommand()
        {
            // UIElement の現状を取得するのは不可能なので、見た目以外の入力で決定する Command のクラスを確認する
            string folderPath = Common.GetFilePathOfDependentEnvironment(@"./TestData/Folder1");
            Explorer explorer = new Explorer(folderPath);
            ExplorerCommand ec;

            // エンターキー
            explorer.SelectedItem = null;
            ec = ExplorerCommandFactory.Create(explorer, Keys.KeyEventType.EnterKey);
            Assert.IsTrue(ec is ExplorerCommandNone);

            // F5 キー
            ec = ExplorerCommandFactory.Create(explorer, Keys.KeyEventType.Update);
            Assert.IsTrue(ec is ExplorerCommandUpdateFolder);

            // 右クリック
            ec = ExplorerCommandFactory.CreateRightButtonEvent(explorer, null);
            Assert.IsTrue(ec is ExplorerCommandNone);

            ExplorerFileInfo efi = new ExplorerFileInfo(System.IO.Path.Combine(folderPath, "test11.txt"));
            ec = ExplorerCommandFactory.CreateRightButtonEvent(explorer, efi);
            Assert.IsTrue(ec is ExplorerCommandDisplayFileMenuWindow);
        }
    }
}
