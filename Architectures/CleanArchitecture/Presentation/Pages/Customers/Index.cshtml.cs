using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Customers.Queries.GetCustomersList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly IGetCustomersListQuery _query;

        public IndexModel(IGetCustomersListQuery query)
        {
            _query = query;
        }

        public IEnumerable<CustomerModel> Customers { get; set; }

        public async Task OnGetAsync()
        {
            Customers = await _query.ExecuteAsync();
        }
    }
}
