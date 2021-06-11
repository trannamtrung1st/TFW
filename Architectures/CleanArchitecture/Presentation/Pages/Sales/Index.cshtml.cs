using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Sales.Queries.GetSalesList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Sales
{
    public class IndexModel : PageModel
    {
        private readonly IGetSalesListQuery _query;

        public IndexModel(IGetSalesListQuery query)
        {
            _query = query;
        }

        public IEnumerable<SalesListItemModel> Sales { get; set; }

        public async Task OnGetAsync()
        {
            Sales = await _query.ExecuteAsync();
        }
    }
}
