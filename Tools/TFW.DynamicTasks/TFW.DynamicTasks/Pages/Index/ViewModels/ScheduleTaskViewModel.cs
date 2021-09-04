using System.ComponentModel.DataAnnotations;

namespace TFW.DynamicTasks.Pages.Index.ViewModels
{
    public class ScheduleTaskViewModel
    {
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
