using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TFW.Framework.Common.Helpers
{
    public static class DirectoryHelper
    {
        public static bool IsSubDirectoryOf(this DirectoryInfo dir, DirectoryInfo another)
        {
            while (dir.FullName.StartsWith(another.FullName) && dir.Parent != null)
            {
                dir = dir.Parent;

                if (dir.FullName == another.FullName) 
                    return true;
            }

            return false;
        }

        public static string GetSolutionFolder()
        {
            return Directory.GetParent(
                Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
        }
    }
}
