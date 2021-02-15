using System;
using System.Collections.Generic;
using System.Linq;

namespace TFW.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.Start(
                new DbMigrationTask());
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
