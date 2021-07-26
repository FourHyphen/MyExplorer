using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
