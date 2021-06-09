using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.ConsoleApp
{
    public static class XConsole
    {
        public static string PromptLine(object message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }
    }
}
