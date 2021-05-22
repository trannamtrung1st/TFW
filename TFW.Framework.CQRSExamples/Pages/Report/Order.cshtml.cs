using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TFW.Framework.CQRSExamples.Models.Query;
using TFW.Framework.i18n.Extensions;

namespace TFW.Framework.CQRSExamples.Pages.Report
{
    public class OrderModel : PageModel
    {
        private readonly IReportQuery _orderReportQuery;

        public OrderModel(IReportQuery orderReportQuery)
        {
            _orderReportQuery = orderReportQuery;
        }

        public IEnumerable<OrderReportListItem> OrderReportList { get; set; }

        [FromQuery]
        public int? FromMonth { get; set; }

        [FromQuery]
        public int? FromYear { get; set; }

        [FromQuery]
        public int? ToMonth { get; set; }

        [FromQuery]
        public int? ToYear { get; set; }

        public async Task OnGet()
        {
            if (FromMonth == null || FromYear == null || ToMonth == null || ToYear == null) return;

            OrderReportList = await _orderReportQuery.GetOrderReportListAsync(
                FromMonth.Value, FromYear.Value, ToMonth.Value, ToYear.Value);
        }
    }
}
