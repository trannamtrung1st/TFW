using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TAuth.ResourceClient.Pages
{
    [Authorize(Roles = "Administrator")]
    public class AdminModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
