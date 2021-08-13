using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer
{
    public class FileExecuteOpen : FileExecute
    {
        public FileExecuteOpen(string name, string filePath) : base(name, filePath) { }

        public override void Execute()
        {
            // 参照に Microsoft Shell Controls And Automation を追加することで Shell32 を参照できる
            Shell32.Shell shell = (Shell32.Shell)Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"));
            shell.Open(FilePath);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(shell);
        }
    }
}
