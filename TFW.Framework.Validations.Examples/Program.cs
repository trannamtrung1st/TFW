using System;
using System.Reflection;
using TFW.Framework.ConsoleApp;
using TFW.Framework.ConsoleApp.Options;

namespace TFW.Framework.Validations.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var consoleProgram = new OptionsProgram()
            {
                Options = new ProgramOptions
                {
                    ExitOption = "exit"
                }
            };

            consoleProgram.Tasks.AddRange(ConfigHelper.FindFromAssemblies(Assembly.GetEntryAssembly()));

            consoleProgram.StartAsync().Wait();
        }
    }
}
