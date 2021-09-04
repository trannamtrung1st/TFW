using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace TFW.Framework.Web.Handlers
{
    public interface IActionFilterHandler
    {
        void OnActionExecuted(ActionExecutedContext context, object filter);
        void OnActionExecuting(ActionExecutingContext context, object filter);
        Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next, object filter);
    }
}
