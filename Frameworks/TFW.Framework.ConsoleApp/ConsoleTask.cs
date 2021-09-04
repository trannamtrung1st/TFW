using System.Threading.Tasks;

namespace TFW.Framework.ConsoleApp
{
    public interface IConsoleTask
    {
        string Title { get; }
        string Description { get; }

        Task StartAsync();
    }

    public abstract class DefaultConsoleTask : IConsoleTask
    {
        public abstract string Title { get; }
        public abstract string Description { get; }

        public abstract Task StartAsync();

        public override string ToString()
        {
            return Title;
        }
    }
}
