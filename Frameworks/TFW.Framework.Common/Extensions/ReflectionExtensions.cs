using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TFW.Framework.Common.Extensions
{
    public static class ReflectionExtensions
    {
        public static T[] GetAllConstants<T>(this Type type)
        {
            var returnType = typeof(T);

            return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == returnType)
                .Select(x => (T)x.GetRawConstantValue())
                .ToArray();
        }

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

        public static T CreateInstance<T>(this Type type) where T : class
        {
            return Activator.CreateInstance(type) as T;
        }
    }
}
