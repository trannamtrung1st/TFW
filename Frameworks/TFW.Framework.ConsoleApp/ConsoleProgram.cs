using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.ConsoleApp.Options;

namespace TFW.Framework.ConsoleApp
{
    public delegate void TaskErrorEventHandler(Exception ex, IConsoleTask consoleTask);

    public interface IConsoleProgram
    {
        Task StartAsync();
        event TaskErrorEventHandler TaskError;
    }

    public class OptionsProgram : IConsoleProgram
    {
        public event TaskErrorEventHandler TaskError;
        public virtual List<IConsoleTask> Tasks => taskList;
        public virtual ProgramOptions Options
        {
            get => options; set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                options = value;
            }
        }

        protected ProgramOptions options;
        protected readonly List<IConsoleTask> taskList;

        public OptionsProgram()
        {
            taskList = new List<IConsoleTask>();
            options = new ProgramOptions();
        }

        public virtual async Task StartAsync()
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
                {
                    var task = taskList[optIdx - 1];
                    try
                    {
                        await task.StartAsync();
                    }
                    catch (Exception ex)
                    {
                        TaskError?.Invoke(ex, task);
                    }
                }
                else Console.Clear();
            }
        }
    }
}
