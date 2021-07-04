using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TAuth.ResourceClient.Exceptions;
using TAuth.ResourceClient.Models.Resource;
using TAuth.ResourceClient.Services;

namespace TAuth.ResourceClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IResourceService _resourceService;

        public IndexModel(ILogger<IndexModel> logger,
            IResourceService resourceService)
        {
            _logger = logger;
            _resourceService = resourceService;
        }

        public IEnumerable<ResourceListItemModel> ResourceList { get; set; }

        public async Task<IActionResult> OnGet()
        {
            await DebugIdentity();

            try
            {
                ResourceList = await _resourceService.GetAsync();
            }
            catch (HttpException ex)
            {
                if (ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                    || ex.Response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return Forbid();
                }

                throw ex;
            }

            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync([FromQuery] int id)
        {
            await _resourceService.DeleteAsync(id);

            return RedirectToPage("/Index");
        }

        private async Task DebugIdentity()
        {
            var idToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            Debug.WriteLine($"IdToken: {idToken}");

            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim: {claim.Type} - {claim.Value}");
            }
        }
    }
}
