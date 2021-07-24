using System;
using System.Collections.Generic;
using RM.Friendly.WPFStandardControls;
using System.Reflection;

namespace TestMyExplorer
{
    class TextBoxAdapter : DisplayControl
    {
        public string TextBoxName { get; }

        public TextBoxAdapter(string textBoxName)
        {
            TextBoxName = textBoxName;
        }

        public string Text(int row, int col, IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree)
        {
            string textBoxName = TextBoxName + row.ToString() + col.ToString();
            var text = logicalTree.ByType<System.Windows.Controls.TextBox>().ByName(textBoxName).Single();
            if (text == null)
            {
                Failure(MethodBase.GetCurrentMethod().Name, textBoxName);
            }

            string str = text.ToString().Replace("System.Windows.Controls.TextBox: ", "");
            str = str.Replace("System.Windows.Controls.TextBox", "");    // 文字列が空の場合の対応
            return str;
        }
    }
}
