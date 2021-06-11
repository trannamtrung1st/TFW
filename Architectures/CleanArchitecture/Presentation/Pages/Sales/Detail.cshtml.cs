using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Sales.Queries.GetSaleDetail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Sales
{
    public class DetailModel : PageModel
    {
        private readonly IGetSaleDetailQuery _saleDetailQuery;

        public DetailModel(IGetSaleDetailQuery saleDetailQuery)
        {
            _saleDetailQuery = saleDetailQuery;
        }

        public SaleDetailModel Sale { get; set; }

        [FromRoute(Name = "id")]
        public int Id { get; set; }

        public async Task OnGetAsync()
        {
            Sale = await _saleDetailQuery.ExecuteAsync(Id);
        }
    }
}
