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

            Assert.IsTrue(GetPrivateField(explorer, "FolderArea").Name == "FolderArea0");
            Assert.IsTrue(GetPrivateField(explorer, "Folder").Name == "Folder0");
            Assert.IsTrue(GetPrivateField(explorer, "FolderFileList").Name == "FileList0");

            Assert.IsTrue(System.IO.Path.GetFileName(explorer.Data.FileList[0]) == "folder01");
            Assert.IsTrue(System.IO.Path.GetFileName(explorer.Data.FileList[1]) == "text11.txt");
            Assert.IsTrue(System.IO.Path.GetFileName(explorer.Data.FileList[2]) == "text12.txt");
        }

        private dynamic GetPrivateField(Explorer explorer, string fieldName)
        {
            PrivateObject po = new PrivateObject(explorer);
            return po.GetField(fieldName);
        }
    }
}
