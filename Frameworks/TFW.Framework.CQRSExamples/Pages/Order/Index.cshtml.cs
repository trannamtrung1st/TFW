using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Pages.Order
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IOrderQuery _orderQuery;

        public IndexModel(IMediator mediator,
            IOrderQuery orderQuery)
        {
            _mediator = mediator;
            _orderQuery = orderQuery;
        }

        public IEnumerable<OrderListItem> OrderList { get; set; }

        public async Task OnGet()
        {
            OrderList = await _orderQuery.GetOrderListAsync();
        }
    }
}
