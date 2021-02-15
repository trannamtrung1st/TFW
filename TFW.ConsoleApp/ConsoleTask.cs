using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TFW.ConsoleApp
{
    public interface IConsoleTask
    {
        IDictionary<string, Func<Task>> Tasks { get; }
        string Title { get; }
        string Description { get; }

        Task Start();
    }

    public abstract class ConsoleTask : IConsoleTask
    {
        public abstract IDictionary<string, Func<Task>> Tasks { get; }
        public abstract string Title { get; }
        public abstract string Description { get; }

        public abstract Task Start();

        public override string ToString()
        {
            return Title;
        }
    }
}
