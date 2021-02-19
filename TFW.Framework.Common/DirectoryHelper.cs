using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TFW.Framework.Common
{
    public static class DirectoryHelper
    {
        public static string GetSolutionFolder()
        {
            return Directory.GetParent(
                Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
        }
    }
}
