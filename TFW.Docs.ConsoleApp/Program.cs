using System;
using System.Collections.Generic;
using System.Linq;
using TFW.Framework.Common.Helpers;
using TFW.Framework.ConsoleApp;
using TFW.Framework.ConsoleApp.Options;

namespace TFW.Docs.ConsoleApp
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

            var assemblies = ReflectionHelper.GetAllAssemblies(searchPattern: "TFW.Docs.*.dll").ToArray();

            consoleProgram.Tasks.AddRange(ConsoleTaskHelper.FindFromAssemblies(assemblies));

            consoleProgram.TaskError += ConsoleProgram_TaskError;

            consoleProgram.StartAsync().Wait();
        }

        private static void ConsoleProgram_TaskError(Exception ex, IConsoleTask consoleTask)
        {
            Console.WriteLine(ex);
        }
    }
}
