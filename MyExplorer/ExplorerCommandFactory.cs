using System.Windows.Controls;

namespace MyExplorer
{
    public class ExplorerCommandFactory
    {
        /// <summary>
        /// キーボードイベント
        /// </summary>
        /// <param name="explorer"></param>
        /// <param name="keyEventType"></param>
        /// <returns></returns>
        public static ExplorerCommand Create(Explorer explorer, Keys.KeyEventType keyEventType)
        {
            if (keyEventType == Keys.KeyEventType.EnterKey)
            {
                return CreateEnterKeyEvent(explorer);
            }
            else if (keyEventType == Keys.KeyEventType.Update)
            {
                return new ExplorerCommandUpdateFolder(explorer);
            }
            else if (explorer.IsFocusedItemInFileList())
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

        private static ExplorerCommand CreateEnterKeyEvent(Explorer explorer)
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

                ListViewItem focused = explorer.GetListViewItemFocused();
                if (focused == null)
                {
                    return new ExplorerCommandNone(explorer);
                }
                if (explorer.Data.IsFolder((string)focused.Content))
                {
                    return new ExplorerCommandForwardFolder(explorer);
                }
            }

            return new ExplorerCommandNone(explorer);
        }

        /// <summary>
        /// マウスイベントを想定
        /// </summary>
        /// <param name="explorer"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ExplorerCommand Create(Explorer explorer, dynamic obj)
        {
            if (DoMouseDownFileList(obj))
            {
                return new ExplorerCommandChangeItemFocus(explorer);
            }

            return new ExplorerCommandNone(explorer);
        }

        private static bool DoMouseDownFileList(dynamic obj)
        {
            if (obj is ScrollViewer sv)
            {
                // Item が選択されている場合は false 判定になる
                return (sv.TemplatedParent is ListView);
            }

            return false;
        }
    }
}