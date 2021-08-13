using System.Diagnostics;

namespace MyExplorer
{
    public class FileExecuteShowProgram : FileExecute
    {
        public FileExecuteShowProgram(string name, string filePath) : base(name, filePath) { }

        public override void Execute()
        {
            // "プログラムから開く" の画面を出すコマンド
            // .\rundll32.exe shell32.dll OpenAs_RunDLL C:\MyDevelopment\GitHub\GitHub.txt
            using (Process p = Process.Start("rundll32.exe", "shell32.dll OpenAs_RunDLL " + FilePath))
            {
                p.WaitForExit();
            }
        }
    }
}