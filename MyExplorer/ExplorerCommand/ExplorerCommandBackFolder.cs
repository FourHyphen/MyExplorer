using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer
{
    public class ExplorerCommandBackFolder : ExplorerCommand
    {
        public ExplorerCommandBackFolder(Explorer explorer) : base(explorer) { }

        public override void Execute(out bool isStateChanged)
        {
            Explorer.Data.MoveFolderOneUp(out isStateChanged);
        }
    }
}
