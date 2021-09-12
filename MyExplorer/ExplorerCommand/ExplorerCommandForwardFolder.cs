using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer
{
    public class ExplorerCommandForwardFolder : ExplorerCommand
    {
        public ExplorerCommandForwardFolder(Explorer explorer) : base(explorer) { }

        public override void Execute()
        {
            ExplorerFileInfo selectedItem = GetSelectedItemOfFolderFileList();
            if (selectedItem == null)
            {
                return;
            }

            Explorer.IntoFolder(selectedItem);
        }

        private ExplorerFileInfo GetSelectedItemOfFolderFileList()
        {
            object selected = Explorer.FolderFileList.SelectedItem;
            return selected is ExplorerFileInfo efi ? efi : null;
        }
    }
}
