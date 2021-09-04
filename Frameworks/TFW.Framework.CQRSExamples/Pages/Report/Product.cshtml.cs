using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Pages.Report
{
    public class ProductModel : PageModel
    {
        private readonly IReportQuery _productReportQuery;

        public ProductModel(IReportQuery productReportQuery)
        {
            _productReportQuery = productReportQuery;
        }

        public IEnumerable<ProductReportListItem> ProductReportList { get; set; }

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

            ProductReportList = await _productReportQuery.GetProductReportListAsync(
                FromMonth.Value, FromYear.Value, ToMonth.Value, ToYear.Value);
        }
    }
}
