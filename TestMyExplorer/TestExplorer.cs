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
            string folderPath = Common.GetFilePathOfDependentEnvironment(@"./TestData/Folder1");
            Explorer explorer = new Explorer(folderPath);
            Assert.IsTrue(explorer.Data.FolderPath.Contains(@"TestData\Folder1"));

            // [0] -> 1 階層上に上るアイコン
            Assert.IsTrue(System.IO.Path.GetFileName(explorer.Data.FileList[1]) == "folder01");
            Assert.IsTrue(System.IO.Path.GetFileName(explorer.Data.FileList[2]) == "text11.txt");
            Assert.IsTrue(System.IO.Path.GetFileName(explorer.Data.FileList[3]) == "text12.txt");
        }

        [TestMethod]
        public void ForwardFolder()
        {
            string folderPath = Common.GetFilePathOfDependentEnvironment(@"./TestData/");
            ExplorerData explorer = new ExplorerData(folderPath);
            explorer.IntoFolder("Folder1", out bool isStateChanged);
            Assert.IsTrue(isStateChanged);
            Assert.IsTrue(explorer.FolderPath.Contains(@"TestData\Folder1"));
            Assert.AreEqual(expected: 4, actual: explorer.FileList.Count);

            explorer.IntoFolder("folder01", out isStateChanged);
            Assert.IsTrue(isStateChanged);
            Assert.IsTrue(explorer.FolderPath.Contains(@"TestData\Folder1\folder01"));
            Assert.AreEqual(expected: 1, actual: explorer.FileList.Count);

            // 存在しないフォルダには入れない、状態変化しない
            explorer.IntoFolder("NOT_EXIST", out isStateChanged);
            Assert.IsFalse(isStateChanged);
            Assert.IsTrue(explorer.FolderPath.Contains(@"TestData\Folder1\folder01"));
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
    }
}
