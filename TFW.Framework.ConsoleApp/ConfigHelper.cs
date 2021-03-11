using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TFW.Framework.Common.Helpers;

namespace TFW.Framework.ConsoleApp
{
    public static class ConfigHelper
    {
        public static IConsoleTask[] FindFromAssemblies(params Assembly[] assemblies)
        {
            var taskTypes = ReflectionHelper.GetAllTypesAssignableTo(typeof(IConsoleTask), assemblies);

            var tasks = taskTypes.Select(o => o.CreateInstance<IConsoleTask>()).ToArray();

            return tasks;
        }
    }
}
