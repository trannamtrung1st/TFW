using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Threading.Tasks;

namespace TFW.DynamicTasks.AppScheduler.Hangfire
{
    public class HangfireAppScheduler : IAppScheduler
    {
        public string ServiceType => BackgroundServiceTypes.Hangfire;

        public Task ScheduleAsync(string rawCode)
        {
            return CSharpScript.EvaluateAsync<object>(rawCode);
        }
    }
}
