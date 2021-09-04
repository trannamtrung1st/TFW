using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Pages.Order
{
    public class DetailModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IOrderQuery _orderQuery;

        public DetailModel(IMediator mediator,
            IOrderQuery orderQuery)
        {
            _mediator = mediator;
            _orderQuery = orderQuery;
        }

        [FromRoute]
        public string Id { get; set; }

        public OrderDetail Detail { get; set; }

        public async Task OnGetAsync()
        {
            Detail = await _orderQuery.GetOrderDetailAsync(Id);
        }
    }
}
