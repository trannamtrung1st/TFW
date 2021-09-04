using System.IO;

namespace TFW.Framework.Common.Helpers
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
