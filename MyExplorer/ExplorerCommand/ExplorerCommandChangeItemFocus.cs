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
            if (Explorer.IsItemInFileList("IsFocused"))
            {
                // FileList の Item にフォーカスが当たってる状態で Item でない ListView 領域をクリックするとここを通る
                // Item の Selected を解除
                Explorer.SelectedItem = null;
                isStateChanged = true;
            }
            else
            {
                // FileList にフォーカスを当てる
                SetFocusFileList(out isStateChanged);
            }
        }

        private void SetFocusFileList(out bool isStateChanged)
        {
            isStateChanged = false;
            if (Explorer.SelectedItem == null)
            {
                // 真に何も選択されていない場合、FileList フォーカスを当てる
                SetFocusFolderFileList();
                isStateChanged = true;
            }
            else if (Explorer.SelectedItem is string itemString)
            {
                // 選択されている状態でフォーカスが ListView から外れ、再度 ListView にフォーカス当たった場合にここを通る
                // この場合はファイルにフォーカスを当てなおす
                ListViewItem item = Explorer.GetListViewItem(itemString);
                if (item != null)
                {
                    SetFocusFile(item);
                    isStateChanged = true;
                }
            }
        }

        private void SetFocusFolderFileList()
        {
            Keyboard.Focus(Explorer.FolderFileList);
            Explorer.FolderFileList.Focus();
        }

        private void SetFocusFile(ListViewItem selected)
        {
            Keyboard.Focus(selected);
            selected.Focus();
        }
    }
}
