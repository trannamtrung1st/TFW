using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Models.Command;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Pages.Product
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IProductQuery _productQuery;

        public IndexModel(IMediator mediator,
            IProductQuery productQuery)
        {
            _mediator = mediator;
            _productQuery = productQuery;
        }

        public IEnumerable<ProductListItem> ProductList { get; set; }

        public async Task OnGet()
        {
            ProductList = await _productQuery.GetProductListAsync();
        }

        [FromQuery]
        public string Id { get; set; }

        public async Task<IActionResult> OnGetDeleteAsync()
        {
            await _mediator.Send(new DeleteProductCommand { Id = Id });

            return RedirectToPage("/Product/Index");
        }
    }
}
