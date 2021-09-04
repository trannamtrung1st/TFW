using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Pages.Report
{
    public class CustomerModel : PageModel
    {
        private readonly IReportQuery _customerReportQuery;

        public CustomerModel(IReportQuery customerReportQuery)
        {
            _customerReportQuery = customerReportQuery;
        }

        public IEnumerable<CustomerReportListItem> CustomerReportList { get; set; }

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

            CustomerReportList = await _customerReportQuery.GetCustomerReportListAsync(
                FromMonth.Value, FromYear.Value, ToMonth.Value, ToYear.Value);
        }
    }
}
