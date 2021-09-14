using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer
{
    public static class Common
    {
        public static readonly string MoveOneUpFolderString = "(←)";

        public static readonly string TypeFolderString = "Folder";

        /// <summary>
        /// 存在するファイル/フォルダのパスがフォルダかを返す
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFolder(string path)
        {
            return System.IO.Directory.Exists(path);
        }
    }
}
