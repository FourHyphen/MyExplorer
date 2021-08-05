using System;
using System.Windows.Controls;

namespace MyExplorer
{
    public class ExplorerCommandFactory
    {
        public static ExplorerCommand Create(Explorer explorer, Keys.KeyEventType keyEventType)
        {
            if (keyEventType == Keys.KeyEventType.EnterKey)
            {
                return DoEnterKeyEvent(explorer);
            }
            else if (keyEventType == Keys.KeyEventType.Update)
            {
                return new ExplorerCommandUpdateFolder(explorer);
            }
            else if (explorer.IsItemInFileList("IsFocused"))
            {
                if (keyEventType == Keys.KeyEventType.FolderBack)
                {
                    return new ExplorerCommandBackFolder(explorer);
                }
                else if (keyEventType == Keys.KeyEventType.FolderForward)
                {
                    return new ExplorerCommandForwardFolder(explorer);
                }
            }

            return new ExplorerCommandNone(explorer);
        }

        private static ExplorerCommand DoEnterKeyEvent(Explorer explorer)
        {
            if (explorer.FolderPath.IsFocused)
            {
                return new ExplorerCommandMoveFolder(explorer);
            }
            else
            {
                ListViewItem top = explorer.GetListViewItem(Common.MoveOneUpFolderString);
                if (top.IsFocused)
                {
                    return new ExplorerCommandBackFolder(explorer);
                }
            }

            return new ExplorerCommandNone(explorer);
        }
    }
}