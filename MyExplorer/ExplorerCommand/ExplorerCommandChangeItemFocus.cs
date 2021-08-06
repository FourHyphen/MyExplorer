using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyExplorer
{
    public class ExplorerCommandChangeItemFocus : ExplorerCommand
    {
        public ExplorerCommandChangeItemFocus(Explorer explorer) : base(explorer) { }

        public override void Execute(out bool isStateChanged)
        {
            isStateChanged = false;
            if (Explorer.IsItemInFileList("IsFocused"))
            {
                // Item の Selected を解除
                Explorer.SelectedItem = null;
                isStateChanged = true;
            }
            else
            {
                // FileList にフォーカスを当てる
                SetFocusFileList();
                isStateChanged = true;
            }
        }

        private void SetFocusFileList()
        {
            if (Explorer.SelectedItem == null)
            {
                // 真に何も選択されていない場合、FileList にキーボードフォーカスと論理フォーカスを当てる
                Explorer.FolderFileList.Focus();
                Keyboard.Focus(Explorer.FolderFileList);
            }

            if (Explorer.SelectedItem is string itemString)
            {
                // 選択されている状態でフォーカスが ListView から外れ、再度 ListView にフォーカス当たった場合にここを通る
                // この場合はファイルにフォーカスを当てなおす
                ListViewItem item = Explorer.GetListViewItem(itemString);
                if (item != null)
                {
                    SetFocusFile(item);
                }
            }
        }

        private void SetFocusFile(ListViewItem selected)
        {
            Keyboard.Focus(selected);
            selected.Focus();
        }
    }
}
