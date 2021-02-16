﻿using System;
using System.Collections.Generic;
using System.Linq;
using TFW.Framework.Common;
using TFW.Framework.ConsoleApp;

namespace TFW.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            var assemblies = ReflectionHelper.GetAllAssemblies();
            var taskTypes = ReflectionHelper.GetAllTypesAssignableTo(typeof(IConsoleTask), assemblies);
            var tasks = taskTypes.Select(o => ReflectionHelper.CreateInstance<IConsoleTask>(o)).ToArray();
            program.Start(tasks);
        }

        public IConsoleTask[] Tasks { get; set; }

        public void Start(params IConsoleTask[] tasks)
        {
            Tasks = tasks;
            string line = null;
            while (line?.Trim().ToLower() != ExitOption)
            {
                var options = tasks.Select((o, i) => $"{i + 1}. {o}");
                Console.Write($"Welcome to console app!\n" +
                    $"Choose 1 option\n" +
                    $"Or type 'exit' then enter to exit.\n" +
                    $"----------------------------------\n" +
                    string.Join("\n", options) + $"\n" +
                    $"----------------------------------\n" +
                    $"Input: ");
                line = Console.ReadLine();
                int optIdx;
                if (int.TryParse(line, out optIdx))
                {
                    Console.Clear();
                    tasks[optIdx - 1].Start().Wait();
                }
                Console.Clear();
            }
        }

        public const string ExitOption = "exit";
    }
}
