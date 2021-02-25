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
            var consoleProgram = new DefaultConsoleProgram()
            {
                Options = new ProgramOptions
                {
                    ExitOption = "exit"
                }
            };

            consoleProgram.AddFromAssemblies(Assembly.GetEntryAssembly());

            consoleProgram.Start();
        }

    }
}
