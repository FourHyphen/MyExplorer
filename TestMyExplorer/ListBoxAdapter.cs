using System;
using System.Collections.Generic;
using RM.Friendly.WPFStandardControls;
using System.Reflection;

namespace TestMyExplorer
{
    class ListBoxAdapter : DisplayControl
    {
        private string ListBoxName { get; }

        public ListBoxAdapter(string listBoxName)
        {
            ListBoxName = listBoxName;
        }

        public bool Contains(string str, int row, int col, IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree)
        {
            string listBoxName = ListBoxName + row.ToString() + col.ToString();
            WPFListBox listBox = GetListBox(listBoxName, logicalTree);
            for (int i = 0; i < listBox.ItemCount; i++)
            {
                WPFListBoxItem item = listBox.GetItem(i);
                string text = item.AppVar.ToString().Replace("System.Windows.Controls.ListBoxItem: ", "");
                if (text == str)
                {
                    return true;
                }
            }

            return false;
        }

        private WPFListBox GetListBox(string listBoxName, IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree)
        {
            try
            {
                return new WPFListBox(logicalTree.ByType<System.Windows.Controls.ListBox>().ByName<System.Windows.Controls.ListBox>(listBoxName).Single());
            }
            catch (Exception)
            {
                Failure(MethodBase.GetCurrentMethod().Name, listBoxName);
                return null;
            }
        }
    }
}
