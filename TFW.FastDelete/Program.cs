using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TFW.FastDelete
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> files = new List<string>();
            string inpFile;
            while (true)
            {
                inpFile = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(inpFile))
                    files.Add(inpFile);
                else break;
            }

            Console.WriteLine("============================");

            files = files.Select(o => new DirectoryInfo(o).FullName).ToList();
            foreach (var file in files)
                Console.WriteLine(file);

            Console.WriteLine("Are you sure?");
            var options = Console.ReadLine();
            if (options.Equals("y", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (var file in files)
                    DeleteAllFiles(file);
            }
        }

        static void DeleteAllFiles(string dir)
        {
            var files = Directory.GetFiles(dir);
            foreach (var f in files)
            {
                Console.WriteLine(f);
                File.Delete(f);
            }
            var folders = Directory.GetDirectories(dir);

            foreach (var folder in folders)
                DeleteAllFiles(folder);
        }
    }
}
