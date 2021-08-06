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

        public override void Execute()
        {
            string input = Explorer.FolderPath.Text;
            Explorer.Data.MoveFolder(input, out bool isStateChanged);
            if (isStateChanged)
            {
                IsDataChanged = true;
            }
        }
    }
}
