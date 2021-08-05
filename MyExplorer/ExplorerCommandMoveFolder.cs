using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer
{
    public class ExplorerCommandMoveFolder : ExplorerCommand
    {
        public ExplorerCommandMoveFolder(Explorer explorer) : base(explorer) { }

        public override void Execute(out bool isStateChanged)
        {
            string input = Explorer.FolderPath.Text;
            Explorer.Data.MoveFolder(input, out isStateChanged);
        }
    }
}
