using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Models.Command;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Pages.ProductCategory
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IProductCategoryQuery _productCategoryQuery;

        public IndexModel(IMediator mediator,
            IProductCategoryQuery productCategoryQuery)
        {
            _mediator = mediator;
            _productCategoryQuery = productCategoryQuery;
        }

        public IEnumerable<ProductCategoryListItem> ProductCategoryList { get; set; }

        public async Task OnGet()
        {
            ProductCategoryList = await _productCategoryQuery.GetProductCategoryListAsync();
        }

        [FromQuery]
        public string Id { get; set; }

        public async Task<IActionResult> OnGetDeleteAsync()
        {
            await _mediator.Send(new DeleteProductCategoryCommand { Id = Id });

            return RedirectToPage("/ProductCategory/Index");
        }
    }
}
