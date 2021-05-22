using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TFW.Framework.CQRSExamples.Models.Command;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Pages.Product
{
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IProductCategoryQuery _productCategoryQuery;

        public CreateModel(IMediator mediator,
            IProductCategoryQuery productCategoryQuery)
        {
            _mediator = mediator;
            _productCategoryQuery = productCategoryQuery;
        }

        [BindProperty]
        public CreateProductCommand Command { get; set; }

        public IEnumerable<SelectListItem> ProductCategories { get; set; }

        public async Task OnGet()
        {
            ProductCategories = (await _productCategoryQuery.GetProductCategoryListOptionAsync())
                .Select(o => new SelectListItem
                {
                    Text = o.Name,
                    Value = o.Id
                }).ToArray();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var id = await _mediator.Send(Command);

            if (id == null) throw new Exception("Failed to create product");

            return RedirectToPage("/Product/Index");
        }
    }
}
