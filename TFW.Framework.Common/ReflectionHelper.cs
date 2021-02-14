using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TFW.Framework.Common
{
    public static class ReflectionHelper
    {
        public static List<Assembly> GetAllAssemblies(string path = null,
            string searchPattern = "*.dll", SearchOption searchOption = SearchOption.AllDirectories)
        {
            if (string.IsNullOrEmpty(path))
                path = GetExecutingAssemblyLocation();
            List<Assembly> allAssemblies = new List<Assembly>();
            string folderPath = Path.GetDirectoryName(path);

            var allDlls = Directory.GetFiles(folderPath, searchPattern, searchOption);
            foreach (string dll in allDlls)
            {
                try
                {
                    allAssemblies.Add(Assembly.LoadFile(dll));
                }
                catch (FileLoadException) { }
                catch (BadImageFormatException) { }
            }
            return allAssemblies;
        }

        public static string GetExecutingAssemblyLocation()
        {
            return Assembly.GetExecutingAssembly().Location;
        }
    }
}
