using Shell32;
using System;

namespace MyExplorer
{
    public abstract class FileExecute
    {
        public int Index { get; }

        public string Name { get; }

        protected string FilePath { get; set; }

        public FileExecute(int index, string name, string filePath)
        {
            Index = index;
            Name = name;
            FilePath = filePath;
        }

        public abstract void Execute();
    }
}