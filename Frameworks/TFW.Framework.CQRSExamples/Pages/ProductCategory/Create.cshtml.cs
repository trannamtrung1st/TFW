using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Models.Command;

namespace TFW.Framework.CQRSExamples.Pages.ProductCategory
{
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;

        public CreateModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public CreateProductCategoryCommand Command { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var id = await _mediator.Send(Command);

            if (id == null) throw new Exception("Failed to create product category");

            return RedirectToPage("/ProductCategory/Index");
        }
    }
}
