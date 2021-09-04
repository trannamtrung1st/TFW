using System.Linq;
using System.Reflection;
using TFW.Framework.Common.Extensions;
using TFW.Framework.Common.Helpers;

namespace TFW.Framework.ConsoleApp
{
    public static class ConsoleTaskHelper
    {
        public static IConsoleTask[] FindFromAssemblies(params Assembly[] assemblies)
        {
            var taskTypes = ReflectionHelper.GetAllTypesAssignableTo(typeof(IConsoleTask), assemblies);

            var tasks = taskTypes.Select(o => o.CreateInstance<IConsoleTask>()).ToArray();

            return tasks;
        }
    }
}
