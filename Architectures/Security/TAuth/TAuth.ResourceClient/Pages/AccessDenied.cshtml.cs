using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TAuth.ResourceClient.Pages
{
    public class AccessDeniedModel : PageModel
    {
        [FromQuery]
        public string ReturnUrl { get; set; }

        public void OnGet()
        {
        }
    }
}
