using RM.Friendly.WPFStandardControls;
using System.Reflection;
using System.Windows;

namespace TestMyExplorer
{
    class FolderPathAdapter : DisplayControl
    {
        public string ElementName { get; }

        public FolderPathAdapter(string elementName)
        {
            ElementName = elementName;
        }

        public string Text(IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree)
        {
            string name = ElementName;
            var text = logicalTree.ByType<System.Windows.Controls.TextBox>().ByName(name).Single();
            if (text == null)
            {
                Failure(MethodBase.GetCurrentMethod().Name, name);
            }

            string str = text.ToString().Replace("System.Windows.Controls.TextBox: ", "");
            str = str.Replace("System.Windows.Controls.TextBox", "");    // 文字列が空の場合の対応
            return str;
        }

        public void SetText(string str, IWPFDependencyObjectCollection<DependencyObject> logicalTree)
        {
            WPFTextBox textBox= new WPFTextBox(logicalTree.ByType<System.Windows.Controls.TextBox>().ByName(ElementName).Single());
            textBox.EmulateChangeText(str);
        }
    }
}
