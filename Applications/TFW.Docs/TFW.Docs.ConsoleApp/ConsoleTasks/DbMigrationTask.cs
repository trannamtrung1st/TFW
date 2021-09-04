using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TFW.Framework.Common.Extensions;
using TFW.Framework.Common.Helpers;
using TFW.Framework.ConsoleApp;

namespace TFW.Docs.ConsoleApp.ConsoleTasks
{
    public class DbMigrationTask : DefaultConsoleTask
    {
        public IDictionary<string, Func<Task>> Tasks => new Dictionary<string, Func<Task>>()
        {
            { $"{AddMigrationOpt}", AddMigration },
            { $"{UpdateDatabaseOpt}", UpdateDatabase },
            { $"{DropDatabaseOpt}", DropDatabase },
        };

        public override string Title => "Database migration tasks";

        public override string Description => $"Options:\n" +
            $"{AddMigrationOpt}. {nameof(AddMigration)}\n" +
            $"{UpdateDatabaseOpt}. {nameof(UpdateDatabase)}\n" +
            $"{DropDatabaseOpt}. {nameof(DropDatabase)}\n" +
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

            Process process = new Process().Build(fileName: "cmd.exe",
                arguments: $"/C dotnet ef migrations add {migrationName} --project={destPrj} --startup-project={startupPrj}",
                workingDir: solutionFolder);

            process.Start();
            process.WaitForExit();

            Console.ReadLine();
            Console.Clear();

            return Task.CompletedTask;
        }

        private Task UpdateDatabase()
        {
            Console.Clear();

            var migrationName = XConsole.PromptLine("Migration name: ");

            var solutionFolder = XConsole.PromptLine("Solution folder: ");
            if (string.IsNullOrWhiteSpace(solutionFolder))
                solutionFolder = DirectoryHelper.GetSolutionFolder();

            var destPrj = XConsole.PromptLine("Destination project: ");
            if (string.IsNullOrWhiteSpace(destPrj))
                destPrj = DefaultDestinationProject;

            var startupPrj = XConsole.PromptLine("Startup project: ");
            if (string.IsNullOrWhiteSpace(startupPrj))
                startupPrj = DefaultStartupProject;

            Process process = new Process().Build(fileName: "cmd.exe",
                arguments: $"/C dotnet ef database update {migrationName} --project={destPrj} --startup-project={startupPrj}",
                workingDir: solutionFolder);

            process.Start();
            process.WaitForExit();

            Console.ReadLine();
            Console.Clear();

            return Task.CompletedTask;
        }

        private Task DropDatabase()
        {
            Console.Clear();

            var solutionFolder = XConsole.PromptLine("Solution folder: ");
            if (string.IsNullOrWhiteSpace(solutionFolder))
                solutionFolder = DirectoryHelper.GetSolutionFolder();

            var destPrj = XConsole.PromptLine("Destination project: ");
            if (string.IsNullOrWhiteSpace(destPrj))
                destPrj = DefaultDestinationProject;

            var startupPrj = XConsole.PromptLine("Startup project: ");
            if (string.IsNullOrWhiteSpace(startupPrj))
                startupPrj = DefaultStartupProject;

            Process process = new Process().Build(fileName: "cmd.exe",
                arguments: $"/C dotnet ef database drop --project={destPrj} --startup-project={startupPrj}",
                workingDir: solutionFolder);

            process.Start();
            process.WaitForExit();

            Console.ReadLine();
            Console.Clear();

            return Task.CompletedTask;
        }

        public override async Task StartAsync()
        {
            Console.Clear();

            var opt = XConsole.PromptLine(Description);

            if (Tasks.ContainsKey(opt))
                await Tasks[opt]();

            XConsole.PromptLine("\nPress enter to exit task");

            Console.Clear();
        }

        public const string AddMigrationOpt = "1";
        public const string UpdateDatabaseOpt = "2";
        public const string DropDatabaseOpt = "3";
        private const string DefaultDestinationProject = "TFW.Docs.Data";
        private const string DefaultStartupProject = "TFW.Docs.WebApi";
    }
}
