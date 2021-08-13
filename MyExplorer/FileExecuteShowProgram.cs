namespace MyExplorer
{
    public class FileExecuteShowProgram : FileExecute
    {
        public FileExecuteShowProgram(string name, string filePath) : base(name, filePath) { }

        public override void Execute()
        {
            // "プログラムから開く" の画面を出すコマンド
            // .\rundll32.exe shell32.dll OpenAs_RunDLL C:\MyDevelopment\GitHub\GitHub.txt
            //FileExecute fe2 = new FileExecute("プログラムから開く", "rundll32.exe", "shell32.dll OpenAs_RunDLL " + filePath);
        }
    }
}