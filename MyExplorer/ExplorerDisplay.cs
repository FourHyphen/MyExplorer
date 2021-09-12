using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer
{
    public class ExplorerDisplay
    {
        public int FolderAreaWidth { get; private set; }

        public int FolderAreaHeight { get; private set; }

        public int FolderPathAreaWidth { get; private set; }

        public int FolderPathAreaHeight { get; private set; }

        public int FolderFileListAreaWidth { get; private set; }

        public int FolderFileListAreaHeight { get; private set; }

        public ExplorerDisplay(MainWindow main)
        {
            SetPosition(main);
        }

        private void SetPosition(MainWindow main)
        {
            int scrollBar = 10;                     // 10: 経験則
            int width = (int)main.ActualWidth - scrollBar;
            int height = (int)main.ActualHeight - scrollBar - 50;    // 50: 経験則

            SetWidth(width);
            SetHeight(height);
        }

        private void SetWidth(int width)
        {
            FolderPathAreaWidth = width;
            FolderFileListAreaWidth = width;
            FolderAreaWidth = width;
        }

        private void SetHeight(int height)
        {
            FolderPathAreaHeight = 40;
            FolderFileListAreaHeight = height - FolderPathAreaHeight;
            FolderAreaHeight = height;
        }
    }
}
