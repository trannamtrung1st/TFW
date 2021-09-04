using System.ComponentModel.DataAnnotations;

namespace TFW.DynamicTasks.Pages.Index.ViewModels
{
    public class ChangeServiceViewModel
    {
        [Display(Name = "Service Type")]
        public string ServiceType { get; set; }
    }
}
