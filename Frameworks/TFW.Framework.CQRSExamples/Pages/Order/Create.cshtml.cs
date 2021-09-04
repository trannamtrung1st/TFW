using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Models.Command;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Pages.Order
{
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IProductQuery _productQuery;

        public CreateModel(IMediator mediator,
            IProductQuery productQuery)
        {
            _mediator = mediator;
            _productQuery = productQuery;
        }

        [BindProperty]
        public CreateOrderCommand Command { get; set; }

        public IEnumerable<ProductListItem> ProductList { get; set; }

        public async Task OnGet()
        {
            ProductList = await _productQuery.GetProductListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Command.OrderItems = Command.OrderItems.Where(o => o.Quantity > 0).ToArray();

            var id = await _mediator.Send(Command);

            if (id == null) throw new Exception("Failed to create order");

            return RedirectToPage("/Order/Index");
        }
    }
}
