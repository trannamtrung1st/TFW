﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TFW.Framework.CQRSExamples.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public string SessionId { get; set; }

        public void OnGet()
        {
            SessionId = HttpContext.Session.Id;
        }
    }
}
