using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyExplorer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuOpenFolderClick(object sender, RoutedEventArgs e)
        {
            string folderPath = DialogOpenFolder.Show();
            if (folderPath != null)
            {
                OpenFolder(folderPath);
            }
        }

        private List<Explorer> Explorers { get; set; } = new List<Explorer>();

        private void OpenFolder(string folderPath)
        {
            Explorer explorer = new Explorer(folderPath, Explorers.Count);
            Explorers.Add(explorer);
            DisplayExplorers();
            SetExplorersPosition();
        }

        private void DisplayExplorers()
        {
            FoldersArea.Children.Clear();
            foreach (Explorer explorer in Explorers)
            {
                FoldersArea.Children.Add(explorer);
            }
        }

        private void SetExplorersPosition()
        {
            int width = (int)ActualWidth;
            int height = (int)ActualHeight - 50;    // 50: 経験則
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

            Canvas.SetLeft(Explorers[0], 0);
            Canvas.SetTop(Explorers[0], 0);
            if (Explorers.Count == 2)
            {
                Canvas.SetLeft(Explorers[1], Explorers[0].CanvasWidth + 1);
                Canvas.SetTop(Explorers[1], 0);
            }
            else if (Explorers.Count == 3)
            {
                Canvas.SetLeft(Explorers[1], Explorers[0].CanvasWidth + 1);
                Canvas.SetTop(Explorers[1], 0);
                Canvas.SetLeft(Explorers[2], 0);
                Canvas.SetTop(Explorers[2], Explorers[0].CanvasHeight + 1);
            }
            else if (Explorers.Count == 4)
            {
                //Explorers[1].CanvasLeft = Explorers[0].CanvasWidth + 1;
                //Explorers[1].CanvasTop = 0;
                //Explorers[2].CanvasLeft = 0;
                //Explorers[2].CanvasTop = Explorers[0].CanvasHeight + 1;
                //Explorers[3].CanvasLeft = Explorers[0].CanvasWidth + 1;
                //Explorers[3].CanvasTop = Explorers[0].CanvasHeight + 1;
            }
        }
    }
}
