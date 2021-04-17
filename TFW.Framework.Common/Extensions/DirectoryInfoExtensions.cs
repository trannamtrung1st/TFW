using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TFW.Framework.Common.Extensions
{
    public static class DirectoryInfoExtensions
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
    }
}
