using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer
{
    public class ExplorerCommandUpdateFolder : ExplorerCommand
    {
        public ExplorerCommandUpdateFolder(Explorer explorer) : base(explorer) { }

        public override void Execute()
        {
            Explorer.Data.Update(out bool isStateChanged);
            if (isStateChanged)
            {
                IsDataChanged = true;
            }
        }
    }
}
