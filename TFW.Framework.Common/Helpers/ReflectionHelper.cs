using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TFW.Framework.Common.Helpers
{
    public static class ReflectionHelper
    {
        public static string GetNameWithoutGenericParameters(this Type type)
        {
            return type.Name.Split('`')[0];
        }

        public static IEnumerable<PropertyInfo> GetPublicProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public);
        }

        public static IEnumerable<Type> GetClassesOfNamespace(string nameSpace, Assembly assembly = null, bool includeSubns = false)
        {
            assembly = assembly ?? Assembly.GetEntryAssembly();

            return assembly.GetTypes()
                .Where(type => includeSubns ? type.Namespace == nameSpace || type.Namespace.StartsWith(nameSpace + ".") :
                    type.Namespace == nameSpace);
        }

        public static List<Assembly> GetAllAssemblies(string path = null,
            string searchPattern = "*.dll", SearchOption searchOption = SearchOption.AllDirectories)
        {
            if (string.IsNullOrEmpty(path))
                path = GetEntryAssemblyLocation();

            List<Assembly> allAssemblies = new List<Assembly>();
            string folderPath = Path.GetDirectoryName(path);
            var allDlls = Directory.GetFiles(folderPath, searchPattern, searchOption);

            foreach (string dll in allDlls)
            {
                try
                {
                    var assembly = Assembly.Load(AssemblyName.GetAssemblyName(dll));
                    allAssemblies.Add(assembly);
                }
                catch (FileNotFoundException)
                {
                    var assembly = Assembly.LoadFile(dll);
                    allAssemblies.Add(assembly);
                }
                catch (FileLoadException) { }
                catch (BadImageFormatException) { }
            }

            return allAssemblies;
        }

        public static IEnumerable<Type> GetAllTypesAssignableTo(Type baseType, IEnumerable<Assembly> assemblies,
            bool baseTypeExcluded = true, bool isAbstract = false, bool isInterface = false)
        {
            var types = assemblies.SelectMany(o => o.GetTypes()).Where(o => baseType.IsAssignableFrom(o)
                && (!baseTypeExcluded || o != baseType) && o.IsAbstract == isAbstract
                && o.IsInterface == isInterface);

            return types;
        }

        public static T CreateInstance<T>(Type type) where T : class
        {
            return Activator.CreateInstance(type) as T;
        }

        public static string GetEntryAssemblyLocation()
        {
            return Assembly.GetEntryAssembly().Location;
        }
    }
}
