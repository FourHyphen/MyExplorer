﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer
{
    public class ExplorerCommandForwardFolder : ExplorerCommand
    {
        public ExplorerCommandForwardFolder(Explorer explorer) : base(explorer) { }

        public override void Execute(out bool isStateChanged)
        {
            isStateChanged = false;

            string selectedName = GetFolderFileListSelected();
            if (selectedName != "")
            {
                Explorer.Data.IntoFolder(selectedName, out isStateChanged);
            }
        }

        private string GetFolderFileListSelected()
        {
            object selected = Explorer.FolderFileList.SelectedItem;
            return selected != null ? selected.ToString() : "";
        }
    }
}