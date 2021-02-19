using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TFW.Framework.Common;
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

            var migrationName = XConsole.PromptLine("Migration name: ");
            if (string.IsNullOrWhiteSpace(migrationName))
            {
                Console.Write("Invalid migration name");
                return Task.CompletedTask;
            }

            var solutionFolder = XConsole.PromptLine("Solution folder: ");
            if (string.IsNullOrWhiteSpace(solutionFolder))
                solutionFolder = DirectoryHelper.GetSolutionFolder();

            var destPrj = XConsole.PromptLine("Destination project: ");
            if (string.IsNullOrWhiteSpace(destPrj))
                destPrj = DefaultDestinationProject;

            var startupPrj = XConsole.PromptLine("Startup project: ");
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
            var opt = XConsole.PromptLine(Description);
        
            switch (opt)
            {
                case AddMigrationOpt:
                    await AddMigration();
                    break;
            }
            
            XConsole.PromptLine("\nPress enter to exit task");
        }

        public const string AddMigrationOpt = "1";
        private const string DefaultDestinationProject = "TFW.Data.Core";
        private const string DefaultStartupProject = "TFW.WebAPI";
    }
}
