using System.IO;

namespace TFW.Framework.DI.Examples.Loggers
{
    public class FileLogger : ILogger
    {
        public string FilePath { get; set; }

        public FileLogger(string filePath)
        {
            FilePath = filePath;
        }

        public void LogToFile(params string[] messages)
        {
            File.AppendAllLines(FilePath, messages);
        }

        public void Log(string message)
        {
            LogToFile(message);
        }
    }
}
