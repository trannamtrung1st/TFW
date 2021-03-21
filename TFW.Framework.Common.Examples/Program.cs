using System;
using System.IO;
using TFW.Framework.Common.Helpers;

namespace TFW.Framework.Common.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            using var watcher = new FileSystemWatcher(
                @"T:\Workspace\Personal\TFW\TFW.WebAPI", "appsettings.json")
                .IncludeSubdirs(false)
                .NotifyFilter(NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size);

            watcher.Changed += Watcher_Changed;

            watcher.Start();

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("appsettings.json changed");
        }
    }
}
