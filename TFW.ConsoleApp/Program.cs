﻿using System;
using System.Collections.Generic;
using System.Linq;
using TFW.Framework.Common.Helpers;
using TFW.Framework.ConsoleApp;
using TFW.Framework.ConsoleApp.Options;

namespace TFW.ConsoleApp
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

            var assemblies = ReflectionHelper.GetAllAssemblies().ToArray();

            consoleProgram.AddFromAssemblies(assemblies);

            consoleProgram.Start();
        }

    }
}
