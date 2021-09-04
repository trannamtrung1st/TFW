using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Threading.Tasks;

namespace TFW.DynamicTasks.AppScheduler.Quartz
{
    public class QuartzAppScheduler : IAppScheduler
    {
        public string ServiceType => BackgroundServiceTypes.Quartz;

        public Task ScheduleAsync(string rawCode)
        {
            return CSharpScript.EvaluateAsync<object>(rawCode);
        }
    }
}
