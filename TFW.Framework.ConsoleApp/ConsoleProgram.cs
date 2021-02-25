using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TFW.Framework.Common.Helpers;
using TFW.Framework.ConsoleApp.Options;

namespace TFW.Framework.ConsoleApp
{
    public interface IConsoleProgram
    {
        IList<IConsoleTask> Tasks { get; }
        ProgramOptions Options { get; set; }
        void AddFromAssemblies(params Assembly[] assemblies);
        void Start();
    }

    public class DefaultConsoleProgram : IConsoleProgram
    {
        protected readonly List<IConsoleTask> taskList;
        public virtual IList<IConsoleTask> Tasks => taskList;

        protected ProgramOptions options;
        public virtual ProgramOptions Options
        {
            get => options; set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                options = value;
            }
        }

        public DefaultConsoleProgram()
        {
            taskList = new List<IConsoleTask>();
            options = new ProgramOptions();
        }

        public virtual void AddFromAssemblies(params Assembly[] assemblies)
        {
            var taskTypes = ReflectionHelper.GetAllTypesAssignableTo(typeof(IConsoleTask), assemblies);

            var tasks = taskTypes.Select(o => o.CreateInstance<IConsoleTask>()).ToArray();

            taskList.AddRange(tasks);
        }

        public virtual void Start()
        {
            string line = null;

            while (line?.Trim().ToLower() != options.ExitOption)
            {
                var taskOptions = taskList.Select((o, i) => $"{i + 1}. {o}");

                line = XConsole.PromptLine($"Welcome to console app!\n" +
                    $"Choose 1 option\n" +
                    $"Or type '{options.ExitOption}' then enter to exit.\n" +
                    $"----------------------------------\n" +
                    string.Join("\n", taskOptions) + $"\n" +
                    $"----------------------------------\n" +
                    $"Input: ");
                int optIdx;

                if (int.TryParse(line, out optIdx) && optIdx <= taskList.Count)
                    taskList[optIdx - 1].Start().Wait();
                else Console.Clear();
            }
        }
    }
}
