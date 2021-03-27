using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.DI.Examples.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            LogToConsole(message);
        }

        public void LogToConsole(string message)
        {
            Console.WriteLine(message);
        }
    }
}
