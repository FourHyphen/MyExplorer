using Shell32;
using System;

namespace MyExplorer
{
    public abstract class FileExecute
    {
        public string Name { get; }

        protected string FilePath { get; set; }

        public FileExecute(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
        }

        public abstract void Execute();
    }
}