using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task OnGet()
        {
            ResourceList = await _resourceService.GetAsync();
        }

        public async Task<IActionResult> OnGetDeleteAsync([FromQuery] int id)
        {
            await _resourceService.DeleteAsync(id);

            return RedirectToPage("/Index");
        }
    }
}
