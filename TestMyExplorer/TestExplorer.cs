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
            Explorer explorer = new Explorer(folderPath, 0);
            Assert.IsTrue(explorer.Data.FolderPath.Contains(@"TestData\Folder1"));

            Assert.IsTrue(GetPrivateMember(explorer, "FolderArea").Name == "FolderArea0");
            Assert.IsTrue(GetPrivateMember(explorer, "Folder").Name == "Folder0");
            Assert.IsTrue(GetPrivateMember(explorer, "FolderFileList").Name == "FileList0");

            Assert.IsTrue(System.IO.Path.GetFileName(explorer.Data.FileList[0]) == "folder01");
            Assert.IsTrue(System.IO.Path.GetFileName(explorer.Data.FileList[1]) == "text11.txt");
            Assert.IsTrue(System.IO.Path.GetFileName(explorer.Data.FileList[2]) == "text12.txt");
        }

        [TestMethod]
        public void CreateExplorerList()
        {
            string folderPath0 = Common.GetFilePathOfDependentEnvironment(@"./TestData/Folder1");
            string folderPath1 = Common.GetFilePathOfDependentEnvironment(@"./TestData/Folder2");

            ExplorerList explorerList = new ExplorerList();
            explorerList.Add(folderPath0);
            explorerList.Add(folderPath1);

            dynamic explorers = GetPrivateMember(explorerList, "Explorers");
            Assert.IsTrue(explorers[0].Data.FolderPath.Contains(@"TestData\Folder1"));
            Assert.IsTrue(explorers[1].Data.FolderPath.Contains(@"TestData\Folder2"));
        }

        private dynamic GetPrivateMember(dynamic instance, string memberName)
        {
            PrivateObject po = new PrivateObject(instance);
            return po.GetFieldOrProperty(memberName);
        }
    }
}
