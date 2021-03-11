using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFW.Framework.ConsoleApp.Options;

namespace TFW.Framework.ConsoleApp
{
    public interface IConsoleProgram
    {
        Task StartAsync();
    }

    public class OptionsProgram : IConsoleProgram
    {
        protected readonly List<IConsoleTask> taskList;
        public virtual List<IConsoleTask> Tasks => taskList;

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
                    await taskList[optIdx - 1].StartAsync();
                else Console.Clear();
            }
        }
    }
}
