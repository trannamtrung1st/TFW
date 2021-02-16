using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TFW.Framework.ConsoleApp;

namespace TFW.ConsoleApp.ConsoleTasks
{
    public class DbMigrationTask : DefaultConsoleTask
    {
        public override IDictionary<string, Func<Task>> Tasks => new Dictionary<string, Func<Task>>()
        {
            { $"{AddMigrationOpt}", AddMigration }
        };

        public override string Title => "Database migration tasks";

        public override string Description => $"Options:\n" +
            $"{AddMigrationOpt}. {nameof(AddMigration)}\n" +
            $"-----------------------------------------\n" +
            $"Input: ";

        private Task AddMigration()
        {
            Console.Clear();
            Console.Write("Solution folder: ");
            var solutionFolder = Console.ReadLine();
 
            if (string.IsNullOrWhiteSpace(solutionFolder))
                solutionFolder = Directory.GetParent(
                    System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

            Console.Write("Migration name: ");
            var migrationName = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(migrationName))
            {
                Console.Write("Invalid migration name");
                return Task.CompletedTask;
            }

            Console.Write("Destination project: ");
            var destPrj = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(destPrj))
                destPrj = DefaultDestinationProject;

            Console.Write("Startup project: ");
            var startupPrj = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(startupPrj))
                startupPrj = DefaultStartupProject;

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WorkingDirectory = solutionFolder;
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"/C dotnet ef migrations add {migrationName} --project={destPrj} --startup-project={startupPrj}";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            
            return Task.CompletedTask;
        }

        public override async Task Start()
        {
            Console.Write(Description);
            var opt = Console.ReadLine();
        
            switch (opt)
            {
                case AddMigrationOpt:
                    await AddMigration();
                    break;
            }
            
            Console.WriteLine();
            Console.WriteLine("Press enter to exit task");
            Console.ReadLine();
        }

        public const string AddMigrationOpt = "1";
        private const string DefaultDestinationProject = "TFW.Data";
        private const string DefaultStartupProject = "TFW.WebAPI";
    }
}
