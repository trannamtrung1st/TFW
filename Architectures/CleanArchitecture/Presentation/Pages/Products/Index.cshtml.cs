using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Products.Queries.GetProductsList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IGetProductsListQuery _query;

        public IndexModel(IGetProductsListQuery query)
        {
            _query = query;
        }

        public IEnumerable<ProductModel> Products { get; set; }

        public async Task OnGetAsync()
        {
            Products = await _query.ExecuteAsync();
        }
    }
}
