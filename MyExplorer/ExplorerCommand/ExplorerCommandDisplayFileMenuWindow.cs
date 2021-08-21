using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyExplorer
{
    public class ExplorerCommandDisplayFileMenuWindow : ExplorerCommand
    {
        public ExplorerCommandDisplayFileMenuWindow(Explorer explorer) : base(explorer) { }

        private ExplorerFileInfo ExplorerFileInfo { get; set; }

        public void Init(ExplorerFileInfo efi)
        {
            ExplorerFileInfo = efi;
        }

        public override void Execute()
        {
            if (ExplorerFileInfo.Name == Common.MoveOneUpFolderString)
            {
                return;
            }

            FileMenuWindow few = new FileMenuWindow(ExplorerFileInfo.FullPath);
            few.Owner = GetParentWindow(Explorer);    // MainWindow を親に設定
            few.Show();
            few.SetFocus();
            Explorer.IsEnabled = false;    // FileMenuWindow に最初からキーボードフォーカスを当てるための無効化
            few.Closed += Few_Closed;
        }

        private void Few_Closed(object sender, EventArgs e)
        {
            Explorer.IsEnabled = true;    // 無効化からの復帰

            // ウィンドウ閉じただけだとフォーカスは宙ぶらりんになったので明示的に指定
            Explorer.FolderFileList.Focus();
            System.Windows.Input.Keyboard.Focus(Explorer.FolderFileList);
        }

        private Window GetParentWindow(FrameworkElement elem)
        {
            DependencyObject parent = elem.Parent;
            if (parent is Window w)
            {
                return w;
            }
            else if (parent == null)
            {
                return null;
            }
            else if (parent is FrameworkElement p)
            {
                return GetParentWindow(p);
            }
            else
            {
                return null;
            }
        }
    }
}
