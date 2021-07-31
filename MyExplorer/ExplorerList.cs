using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyExplorer
{
    public class ExplorerList
    {
        private List<Explorer> Explorers { get; set; } = new List<Explorer>();

        public void Add(string folderPath)
        {
            Explorer explorer = new Explorer(folderPath, Explorers.Count);
            Explorers.Add(explorer);
        }

        public void SetPosition(MainWindow main)
        {
            int width = (int)main.ActualWidth;
            int height = (int)main.ActualHeight - 50;    // 50: 経験則
            int scrollBar = 10;                     // 10: 経験則
            if (Explorers.Count == 2)
            {
                width = width / 2 - scrollBar;
            }
            else if (Explorers.Count >= 3)
            {
                width = width / 2 - scrollBar;
                height = height / 2 - scrollBar;
            }

            foreach (Explorer explorer in Explorers)
            {
                explorer.SetWidth(width);
                explorer.SetHeight(height);
            }

            Explorers[0].SetLeft(0);
            Explorers[0].SetTop(0);
            if (Explorers.Count >= 2)
            {
                Explorers[1].SetLeft(Explorers[0].CanvasWidth + 1);
                Explorers[1].SetTop(0);
            }
            if (Explorers.Count >= 3)
            {
                Explorers[2].SetLeft(0);
                Explorers[2].SetTop(Explorers[0].CanvasHeight + 1);
            }
            if (Explorers.Count >= 4)
            {
                Explorers[3].SetLeft(Explorers[0].CanvasWidth + 1);
                Explorers[3].SetTop(Explorers[0].CanvasHeight + 1);
            }
        }

        public void DoKeyEvent(Keys.KeyEventType keyEventType)
        {
            // どのフォルダにフォーカスが当たっているかを特定
            Explorer explorer = GetNowFocusing();

            // 現在フォーカス中のフォルダに対してKeyEventを発行
            if (explorer != null)
            {
                explorer.DoKeyEvent(keyEventType);
            }
        }

        private Explorer GetNowFocusing()
        {
            if (Explorers.Count == 1)
            {
                return Explorers[0];
            }

            return Explorers.Find(t => t.NowFocusing());
        }

        public void Display(UIElementCollection collection)
        {
            collection.Clear();
            foreach (Explorer explorer in Explorers)
            {
                collection.Add(explorer);
            }
        }

        /// <summary>
        /// テストで使用
        /// </summary>
        /// <param name="fileName"></param>
        public void FocusFile(string fileName)
        {
            Explorer explorer = GetNowFocusing();
            explorer.FocusFile(fileName);
        }
    }
}
