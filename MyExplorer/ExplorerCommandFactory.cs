using System;
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

            if (keyEventType == Keys.KeyEventType.Update)
            {
                return new ExplorerCommandUpdateFolder(explorer);
            }

            if (keyEventType == Keys.KeyEventType.DisplayFileMenuWindow)
            {
                return CreateDisplayFileMenuWindow(explorer);
            }

            if (explorer.IsFocusedItemInFileList())
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

            if (explorer.SelectedItem == null)
            {
                // ファイルリストのフォーカス外してる場合(1行が青くなってない場合)はエンターキーでも何もしない
                return new ExplorerCommandNone(explorer);
            }

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

            ExplorerFileInfo efi = (ExplorerFileInfo)focused.Content;
            if (efi.Type == Common.TypeFolderString)
            {
                return new ExplorerCommandForwardFolder(explorer);
            }
            else if (efi.Name != Common.MoveOneUpFolderString)
            {
                ExplorerCommandFileExecute command = new ExplorerCommandFileExecute(explorer);
                command.Init(efi);
                return command;
            }

            return new ExplorerCommandNone(explorer);
        }

        private static ExplorerCommand CreateDisplayFileMenuWindow(Explorer explorer)
        {
            if (DoDisplayFileMenuWindow(explorer, out ExplorerFileInfo efi))
            {
                ExplorerCommandDisplayFileMenuWindow command = new ExplorerCommandDisplayFileMenuWindow(explorer);
                command.Init(efi);
                return command;
            }
            else
            {
                return new ExplorerCommandNone(explorer);
            }
        }

        private static bool DoDisplayFileMenuWindow(Explorer explorer, out ExplorerFileInfo efi)
        {
            efi = null;
            if (explorer.SelectedItem == null)
            {
                // ファイルリストのフォーカス外してる場合(1行が青くなってない場合)
                return false;
            }

            if (explorer.SelectedItem is ExplorerFileInfo)
            {
                efi = (ExplorerFileInfo)explorer.SelectedItem;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 左クリックを想定
        /// </summary>
        /// <param name="explorer"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ExplorerCommand CreateLeftButtonEvent(Explorer explorer, dynamic obj)
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

        /// <summary>
        /// ダブルクリック時を想定
        /// </summary>
        /// <param name="explorer"></param>
        /// <returns></returns>
        public static ExplorerCommand CreateDoubleClickEvent(Explorer explorer)
        {
            return CreateEnterKeyEvent(explorer);
        }

        /// <summary>
        /// 右クリック時を想定
        /// </summary>
        /// <param name="explorer"></param>
        /// <param name="efi"></param>
        /// <returns></returns>
        public static ExplorerCommand CreateRightButtonEvent(Explorer explorer, ExplorerFileInfo efi)
        {
            if (efi != null)
            {
                ExplorerCommandDisplayFileMenuWindow command = new ExplorerCommandDisplayFileMenuWindow(explorer);
                command.Init(efi);
                return command;
            }

            return new ExplorerCommandNone(explorer);
        }
    }
}