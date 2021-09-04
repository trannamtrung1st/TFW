using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.DynamicTasks.AppScheduler;

namespace TFW.DynamicTasks.DynamicTask
{
    public class DynamicTaskService : IDynamicTaskService
    {
        private IAppScheduler _currentScheduler;
        private readonly IEnumerable<IAppScheduler> _appSchedulers;

        public DynamicTaskService(IEnumerable<IAppScheduler> appSchedulers)
        {
            _appSchedulers = appSchedulers;
            _currentScheduler = appSchedulers.FirstOrDefault();
        }

        public string CurrentService => _currentScheduler.ServiceType;

        public Task ChangeServiceAsync(string serviceType)
        {
            _currentScheduler = _appSchedulers.FirstOrDefault(s => s.ServiceType == serviceType);
            return Task.CompletedTask;
        }

        public Task ScheduleTaskAsync(string rawCode)
        {
            return _currentScheduler.ScheduleAsync(rawCode);
        }
    }
}
