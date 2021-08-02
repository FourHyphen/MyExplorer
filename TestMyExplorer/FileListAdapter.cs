using System;
using System.Collections.Generic;
using RM.Friendly.WPFStandardControls;
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

        public bool Contains(string str, IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree)
        {
            string name = ElementName;
            WPFListView listBox = GetFileListElement(name, logicalTree);
            for (int i = 0; i < listBox.ItemCount; i++)
            {
                WPFListViewItem item = listBox.GetItem(i);
                string text = item.AppVar.ToString().Replace("System.Windows.Controls.ListViewItem: ", "");
                if (text == str)
                {
                    return true;
                }
            }

            return false;
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
