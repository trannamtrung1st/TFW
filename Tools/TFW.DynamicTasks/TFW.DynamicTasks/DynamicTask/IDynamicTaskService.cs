using System.Threading.Tasks;

namespace TFW.DynamicTasks.DynamicTask
{
    public interface IDynamicTaskService
    {
        string CurrentService { get; }
        Task ChangeServiceAsync(string serviceType);
        Task ScheduleTaskAsync(string rawCode);
    }
}
