using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.Background.Services
{
    public class DisposableService : IDisposable
    {
        public DisposableService(ChildDisposableService service)
        {
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing");
        }

        public void Process()
        {
            Console.WriteLine("Processing");
            BackgroundJob.Schedule(() => Process(), DateTimeOffset.UtcNow.AddSeconds(10));
        }
    }

    public class ChildDisposableService : IDisposable
    {
        public ChildDisposableService()
        {
            Console.WriteLine("New child");
        }

        public void Dispose()
        {
            Console.WriteLine("Child disposing");
        }
    }
}
