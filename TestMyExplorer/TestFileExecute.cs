using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyExplorer;

namespace TestMyExplorer
{
    [TestClass]
    public class TestFileExecute
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
        public void TestFileExecuteZip()
        {
            // ファイルの圧縮テスト
            string filePath = Common.GetFilePathOfDependentEnvironment(@"./TestData/Folder1/text11.txt");
            string zipPath = filePath.Replace(System.IO.Path.GetExtension(filePath), ".zip");
            DeleteFile(zipPath);

            FileExecuteZip fez = new FileExecuteZip(3, "zip", filePath);
            fez.Execute();
            Assert.IsTrue(System.IO.File.Exists(zipPath));
            DeleteFile(zipPath);    // 後始末

            // フォルダの圧縮テスト
            string folderPath = Common.GetFilePathOfDependentEnvironment(@"./TestData/Folder1");
            zipPath = folderPath + ".zip";
            DeleteFile(zipPath);

            fez = new FileExecuteZip(3, "zip", folderPath);
            fez.Execute();
            Assert.IsTrue(System.IO.File.Exists(zipPath));
            DeleteFile(zipPath);    // 後始末
        }

        private void DeleteFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
