using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer
{
    public abstract class ExplorerCommand
    {
        protected Explorer Explorer { get; set; }

        public bool IsDataChanged { get; protected set; } = false;

        public bool IsSelectedItemChanged { get; protected set; } = false;

        public ExplorerCommand(Explorer explorer)
        {
            Explorer = explorer;
        }

        public abstract void Execute();
    }
}
