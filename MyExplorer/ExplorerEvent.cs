using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MyExplorer
{
    public partial class Explorer
    {
        private void KeyDowned(object sender, KeyEventArgs e)
        {
            KeyDowned(e.Key, e.SystemKey, e.KeyboardDevice.Modifiers);
        }

        private void KeyDowned(Key key, Key systemKey, ModifierKeys modifier)
        {
            Keys.KeyEventType keyEventType = Keys.ToKeyEventType(key, systemKey, modifier);
            if (keyEventType != Keys.KeyEventType.Else)
            {
                DoKeyEvent(keyEventType);
            }
        }

        private void DoKeyEvent(Keys.KeyEventType keyEventType)
        {
            ExplorerCommand ec = ExplorerCommandFactory.Create(this, keyEventType);
            DoEventCore(ec);
        }

        private void MouseLeftButtonDownClicked(object sender, MouseButtonEventArgs e)
        {
            MouseLeftButtonDownClicked(e.GetPosition((UIElement)sender));
        }

        private void MouseLeftButtonDownClicked(Point p)
        {
            HitTestResult result = VisualTreeHelper.HitTest(this, p);
            dynamic obj = result.VisualHit;
            DoLeftMouseEvent(obj);
        }

        private void DoLeftMouseEvent(dynamic obj)
        {
            ExplorerCommand ec = ExplorerCommandFactory.CreateLeftButtonEvent(this, obj);
            DoEventCore(ec);
        }

        private void MouseDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            DoMouseDoubleClickEvent();
        }

        private void DoMouseDoubleClickEvent()
        {
            ExplorerCommand ec = ExplorerCommandFactory.CreateDoubleClickEvent(this);
            DoEventCore(ec);
        }

        private void MouseRightButtonClickedOfFolderFileListItem(object sender, MouseButtonEventArgs e)
        {
            MouseRightButtonClickedOfFolderFileListItem(sender);
        }

        private void MouseRightButtonClickedOfFolderFileListItem(object sender)
        {
            MouseRightButtonClickedPrepare(sender);
            FolderFileListItemMouseRightButtonClicked((ExplorerFileInfo)FolderFileList.SelectedItem);
        }

        private void MouseRightButtonClickedPrepare(object sender)
        {
            // ファイル選択中に別ファイルに対して右クリックした際、選択中ファイルを変える
            if (sender is ListViewItem lvi && lvi.Content is ExplorerFileInfo eventItem)
            {
                ExplorerFileInfo now = (ExplorerFileInfo)FolderFileList.SelectedItem;
                // TODO: バグ: ファイル無選択の場合、now が null になって null 参照例外起こす
                if (eventItem.Name != now.Name)
                {
                    SelectedItem = eventItem;
                    SelectedItemChanged();
                }
            }
        }

        private void FolderFileListItemMouseRightButtonClicked(ExplorerFileInfo efi)
        {
            DoRightMouseEvent(efi);
        }

        private void DoRightMouseEvent(ExplorerFileInfo efi)
        {
            ExplorerCommand ec = ExplorerCommandFactory.CreateRightButtonEvent(this, efi);
            DoEventCore(ec);
        }

        private void DoEventCore(ExplorerCommand ec)
        {
            ec.Execute();
        }
    }
}
