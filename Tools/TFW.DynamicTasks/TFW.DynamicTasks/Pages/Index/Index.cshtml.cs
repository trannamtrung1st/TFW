using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TFW.DynamicTasks.DynamicTask;
using TFW.DynamicTasks.Pages.Index.ViewModels;

namespace TFW.DynamicTasks.Pages.Index
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IDynamicTaskService _dynamicTaskService;

        public IndexModel(ILogger<IndexModel> logger,
            IDynamicTaskService dynamicTaskService)
        {
            _logger = logger;
            _dynamicTaskService = dynamicTaskService;
        }

        public async Task OnGetAsync()
        {
            await InitAsync();
        }

        #region ChangeService
        [BindProperty]
        public ChangeServiceViewModel ChangeServiceViewModel { get; set; }

        public async Task OnPostChangeServiceAsync()
        {
            await _dynamicTaskService.ChangeServiceAsync(ChangeServiceViewModel.ServiceType);

            await InitAsync();
        }
        #endregion

        #region ScheduleTask
        [BindProperty]
        public ScheduleTaskViewModel ScheduleTaskViewModel { get; set; }

        public async Task OnPostScheduleTaskAsync()
        {
            await _dynamicTaskService.ScheduleTaskAsync(ScheduleTaskViewModel.Code);

            await InitAsync();

            ScheduleTaskViewModel.Message = "Successfully!";
        }
        #endregion

        private Task InitAsync()
        {
            ChangeServiceViewModel = new ChangeServiceViewModel
            {
                ServiceType = _dynamicTaskService.CurrentService
            };

            ScheduleTaskViewModel = new ScheduleTaskViewModel
            {
                Code = ""
            };

            return Task.CompletedTask;
        }
    }
}
