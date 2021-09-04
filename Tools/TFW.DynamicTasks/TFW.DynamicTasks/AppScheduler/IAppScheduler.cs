using System.Threading.Tasks;

namespace TFW.DynamicTasks.AppScheduler
{
    public interface IAppScheduler
    {
        string ServiceType { get; }
        Task ScheduleAsync(string rawCode);
    }
}
