using System;
using System.Diagnostics;
using System.Windows.Input;
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
        private WindowsAppFriend App;
        private MainWindowDriver Driver;

        private string BeforeEnvironment { get; set; }

        [TestInitialize]
        public void Init()
        {
            // MainWindowプロセスにattach
            string exePath = System.IO.Path.GetFullPath(AttachExeName);
            App = new WindowsAppFriend(Process.Start(exePath));
            Driver = new MainWindowDriver(App);

            BeforeEnvironment = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Common.GetEnvironmentDirPath();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Process exeMainProcess = Process.GetProcessById(App.ProcessId);
            App.Dispose();
            exeMainProcess.CloseMainWindow();
            exeMainProcess.Dispose();

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
            ModifierKeys modifierNone = ModifierKeys.None;

            string folderPath = Common.GetFilePathOfDependentEnvironment(@".\TestData\Folder1");
            Driver.OpenFolder(folderPath);

            // フォーカスが TextBox に当たっている場合はフォルダ移動しない
            Driver.FocusFolderPathArea();
            Driver.EmurateKey(Key.Left, modifierNone);
            Assert.IsTrue(Driver.GetFolderPath().Contains(@"\TestData\Folder1"));

            // 1 階層上に上る
            Driver.FocusFile("text11.txt");
            Driver.EmurateKey(Key.Left, modifierNone);
            Assert.IsFalse(Driver.GetFolderPath().Contains(@"\TestData\Folder1"));
            Assert.IsTrue(Driver.ContainFile("Folder1"));
            Assert.IsTrue(Driver.ContainFile("Folder2"));

            // (←)のエンターキー押下にて 1 階層上に移動する
            Driver.FocusFile(MyExplorer.Common.MoveOneUpFolderString);
            Driver.EmurateKey(Key.Enter, modifierNone);
            Assert.IsFalse(Driver.GetFolderPath().Contains(@"\TestData"));
            Assert.IsTrue(Driver.ContainFile("TestData"));
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
            Driver.EmurateKey(Key.Enter, ModifierKeys.None);
            Assert.IsTrue(Driver.GetFolderPath().Contains(@"\TestData\Folder2"));
            Assert.IsTrue(Driver.ContainFile("text21.txt"));

            // 存在しないフォルダへは移動せず、画面は変化しない
            Driver.SetFolderPathText("NOT_EXIST");
            Driver.EmurateKey(Key.Enter, ModifierKeys.None);
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
            Driver.EmurateKey(Key.F5, ModifierKeys.None);
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

            // ファイルが 1 つも存在しないフォルダの場合
            CreateFolder(testFolderPath);
            Driver.OpenFolder(testFolderPath);
            Assert.IsTrue(Driver.ContainFile(Common.GoBackString));

            string testFileName = "testEmpty.txt";
            string testFilePath = System.IO.Path.Combine(testFolderPath, testFileName);
            CreateFile(testFilePath);

            // フォルダ内にファイルが作成され、状態更新後でも 1 階層上に上るアイコンは表示される
            Driver.EmurateKey(Key.F5, ModifierKeys.None);
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

        [TestMethod]
        public void ForwardFolderByInputKey()
        {
            // キー入力によるフォルダの中への移動(1 階層潜る)
            string folderPath = Common.GetFilePathOfDependentEnvironment(@".\TestData");
            Driver.OpenFolder(folderPath);

            // フォルダのフォーカス中にフォルダを移動する
            Driver.FocusFile("Folder1");
            Driver.EmurateKey(Key.Right, ModifierKeys.None);
            Assert.IsTrue(Driver.GetFolderPath().Contains(@"TestData\Folder1"));

            // ファイルのフォーカス中はフォルダ移動しない
            Driver.FocusFile("test11.txt");
            Driver.EmurateKey(Key.Right, ModifierKeys.None);
            Assert.IsFalse(Driver.GetFolderPath().Contains(@"TestData\Folder1\"));
        }

        [TestMethod]
        public void DisplayFileMenuWindow()
        {
            // ファイルメニュー画面表示機能のテスト
            string folderPath = Common.GetFilePathOfDependentEnvironment(@".\TestData\Folder1");
            string fileName = "text11.txt";
            Driver.OpenFolder(folderPath);
            Driver.FocusFile(fileName);
            Driver.EmurateKey(Key.F10, ModifierKeys.Shift);

            new FileMenuWindowDriver(App).Close();
        }

        [TestMethod]
        public void ZipFromFileMenuWindow()
        {
            // ファイルメニューから zip 圧縮するテスト
            // 準備
            string folderPath = Common.GetFilePathOfDependentEnvironment(@".\TestData\Folder1");
            string zipName = "text11.zip";
            string zipPath = System.IO.Path.Combine(folderPath, zipName);
            DeleteFile(zipPath);

            // ファイルメニュー画面表示
            string fileName = "text11.txt";
            Driver.OpenFolder(folderPath);
            Driver.FocusFile(fileName);
            Driver.EmurateKey(Key.F10, ModifierKeys.Shift);
            Assert.IsFalse(Driver.ContainFile(zipName));

            new FileMenuWindowDriver(App).Zip();

            // ファイル確認
            Driver.EmurateKey(Key.F5, ModifierKeys.None);
            Assert.IsTrue(Driver.ContainFile(zipName));

            // 後始末
            DeleteFile(zipPath);
        }

        [TestMethod]
        public void CopyPathForClipboard()
        {
            // クリップボードにファイルパスを出力する
            // 準備
            System.Windows.Clipboard.SetText("");
            string folderPath = Common.GetFilePathOfDependentEnvironment(@".\TestData\Folder1");
            Driver.OpenFolder(folderPath);

            // ファイルのパスコピー
            CopyPathForClipboardCore(folderPath, "text11.txt");

            // フォルダのパスコピー
            CopyPathForClipboardCore(folderPath, "folder01");
        }

        private void CopyPathForClipboardCore(string folderPath, string fileName)
        {
            Driver.FocusFile(fileName);
            Driver.EmurateKey(Key.F10, ModifierKeys.Shift);

            new FileMenuWindowDriver(App).PathCopy();

            AssertAreEqualClipboard(System.IO.Path.Combine(folderPath, fileName));
        }

        private void AssertAreEqualClipboard(string expected)
        {
            string clipboard = System.Windows.Clipboard.GetText();
            Assert.AreEqual(expected: expected, actual: clipboard);
        }

        [TestMethod]
        public void DeleteFileFromFileMenuWindow()
        {
            // 準備: 削除するファイルを作成
            string guid = System.Guid.NewGuid().ToString("N");
            string folderPath = Common.GetFilePathOfDependentEnvironment(@".\TestData\Folder1");
            string testFileName = "deletetest_" + guid + ".txt";
            string testFilePath = System.IO.Path.Combine(folderPath, testFileName);
            DeleteFile(testFilePath);
            CreateFile(testFilePath);

            // 準備: 削除するフォルダを作成
            string testFolderName = "deletefolder_" + guid;
            string testFolderPath = System.IO.Path.Combine(folderPath, testFolderName);
            DeleteFolder(testFolderPath);
            CreateFolder(testFolderPath);

            Driver.OpenFolder(folderPath);

            // ファイルの削除
            TestDeleteCore(testFileName, testFilePath, System.IO.File.Exists);

            // フォルダの削除
            TestDeleteCore(testFolderName, testFolderPath, System.IO.Directory.Exists);
        }

        private void TestDeleteCore(string name, string path, Func<string, bool> func)
        {
            // 準備: ごみ箱に存在しないこと
            var bins = GetFileNamesInRecycleBin();
            Assert.IsFalse(bins.Contains(name));

            // 準備: ファイル/フォルダが存在すること
            Assert.IsTrue(Driver.ContainFile(name));
            Assert.IsTrue(func(path));

            // 削除実行
            Driver.FocusFile(name);
            Driver.EmurateKey(Key.F10, ModifierKeys.Shift);
            new FileMenuWindowDriver(App).Delete();

            // 削除されたことの確認
            Assert.IsFalse(Driver.ContainFile(name));
            Assert.IsFalse(func(path));

            // ごみ箱には存在することの確認
            bins = GetFileNamesInRecycleBin();
            Assert.IsTrue(bins.Contains(name));
        }

        private System.Collections.Generic.List<string> GetFileNamesInRecycleBin()
        {
            System.Collections.Generic.List<string> result = new System.Collections.Generic.List<string>();

            // 参考: http://nonsoft.la.coocan.jp/SoftSample/CS.NET/SampleRccList.html
            // 要作業(1): 参照に COM: Microsoft Shell Controls And Automation を追加 -> Shell32 と表示される
            // 要作業(2): 参照の Shell32 の "相互運用型の埋め込み" を False に変更する
            Shell32.ShellClass shl = new Shell32.ShellClass();
            Shell32.Folder fol = shl.NameSpace(10);

            foreach (Shell32.FolderItem folderItem in fol.Items())
            {
                // ごみ箱内のパスはもとのファイル名ではないので、ごみ箱内ファイルのもとのファイル名を取得
                // ex) folderItem.Path = C:\\$Recycle.Bin\\S-1-5-21-638183743-3077767593-1375336953-1001\\$R22YH79.txt
                string beforeFileName = fol.GetDetailsOf(folderItem, 0);    // 0 -> ファイル名 / 1 -> もとのフォルダパス / 2 -> 削除日 / etc....
                result.Add(beforeFileName);
            }

            return result;
        }
    }
}
