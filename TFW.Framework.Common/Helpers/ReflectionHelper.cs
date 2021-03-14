using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace TFW.Framework.Common.Helpers
{
    public static class ReflectionHelper
    {
        public static MethodInfo GetInstanceMethod(this Type type, string methodName, bool isPublic = true,
            bool nonPublic = false)
        {
            var flag = BindingFlags.Instance;

            if (isPublic)
                flag = flag | BindingFlags.Public;

            if (nonPublic)
                flag = flag | BindingFlags.NonPublic;

            return type.GetMethod(methodName, flag);
        }

        public static TOut InvokeGeneric<TOut>(this MethodInfo genMethodInfo,
            object subject, Type[] genArgs, params object[] args)
        {
            genMethodInfo = genMethodInfo.MakeGenericMethod(genArgs);

            return (TOut)genMethodInfo.Invoke(subject, args);
        }

        public static string GetNameWithoutGenericParameters(this Type type)
        {
            return type.Name.Split('`')[0];
        }

        public static IEnumerable<PropertyInfo> GetPublicProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public);
        }

        public static IEnumerable<Type> GetTypesOfNamespace(string nameSpace, Assembly assembly = null, bool includeSubns = false)
        {
            assembly = assembly ?? Assembly.GetEntryAssembly();

            return assembly.GetTypes()
                .Where(type => includeSubns ? type.Namespace == nameSpace || type.Namespace.StartsWith(nameSpace + ".") :
                    type.Namespace == nameSpace);
        }

        public static List<Assembly> GetAllAssemblies(string rootPath = null,
            string searchPattern = "*.dll", IEnumerable<string> excludedRelativeDirPaths = null,
            SearchOption searchOption = SearchOption.AllDirectories)
        {
            if (string.IsNullOrEmpty(rootPath))
                rootPath = GetEntryAssemblyLocation();
            string rootFolderPath = Path.GetDirectoryName(rootPath);

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .ToDictionary(o => o.FullName);
            
            List<Assembly> allAssemblies = new List<Assembly>();

            var excludedDirs = excludedRelativeDirPaths?.Select(dirPath => new DirectoryInfo(
                Path.Combine(Directory.GetParent(rootPath).FullName, dirPath))).ToArray() ?? new DirectoryInfo[] { };

            var targetAssemblies = Directory.EnumerateFiles(rootFolderPath, searchPattern, searchOption)
                .Where(file => !excludedDirs.Any(dir => Directory.GetParent(file).IsSubDirectoryOf(dir)))
                .Select(dll => new
                {
                    Dll = dll,
                    Name = SafelyGetAssemblyName(dll)
                }).Where(assObj => assObj.Name != null).ToArray();

            foreach (var assObj in targetAssemblies)
            {
                if (loadedAssemblies.ContainsKey(assObj.Name.FullName))
                {
                    allAssemblies.Add(loadedAssemblies[assObj.Name.FullName]);
                    continue;
                }

                try
                {
                    var assembly = Assembly.Load(assObj.Name);
                    allAssemblies.Add(assembly);
                }
                catch (FileNotFoundException)
                {
                    var assembly = Assembly.LoadFile(assObj.Dll);
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

        public static AssemblyName SafelyGetAssemblyName(string assFile)
        {
            try
            {
                return AssemblyName.GetAssemblyName(assFile);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static T CreateInstance<T>(this Type type) where T : class
        {
            return Activator.CreateInstance(type) as T;
        }

        public static string GetEntryAssemblyLocation()
        {
            return Assembly.GetEntryAssembly().Location;
        }
    }
}
