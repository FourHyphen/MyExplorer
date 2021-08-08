using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using RM.Friendly.WPFStandardControls;
using System;
using System.Reflection;

namespace TestMyExplorer
{
    class FileListAdapter : DisplayControl
    {
        private string ElementName { get; }

        public FileListAdapter(string elementName)
        {
            ElementName = elementName;
        }

        public bool Contains(string fileName,
                             dynamic mainWindow,
                             WindowsAppFriend app,
                             IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree)
        {
            WPFListView listBox = GetFileListElement(ElementName, logicalTree);

            for (int i = 0; i < listBox.ItemCount; i++)
            {
                // Friendly 参考: https://qiita.com/advent-calendar/2014/friendly
                dynamic val = app.Null();
                WindowsAppExpander.LoadAssembly(app, GetType().Assembly);
                app.Type(GetType()).GetExplorerFileInfo(mainWindow, i, out val);

                //var tmp = (ExplorerFileInfo)val;    テストのためだけにクラスをシリアライズ化しない
                if ((string)val.Name == fileName)
                {
                    return true;
                }
            }

            return false;
        }

        private static void GetExplorerFileInfo(MyExplorer.MainWindow main, int index, out dynamic val)
        {
            val = main.Explorer.Data.FileList[index];
        }

        private WPFListView GetFileListElement(string elementName, IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree)
        {
            try
            {
                return new WPFListView(logicalTree.ByType<System.Windows.Controls.ListView>().ByName<System.Windows.Controls.ListView>(elementName).Single());
            }
            catch (Exception)
            {
                Failure(MethodBase.GetCurrentMethod().Name, elementName);
                return null;
            }
        }
    }
}
