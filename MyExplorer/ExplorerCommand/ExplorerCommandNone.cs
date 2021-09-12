using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer
{
    public class ExplorerCommandNone : ExplorerCommand
    {
        public ExplorerCommandNone(Explorer explorer) : base(explorer) { }

        public override void Execute()
        {
            // nothing
        }
    }
}
