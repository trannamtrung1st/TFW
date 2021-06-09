using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.CQRSExamples.Entities.Query
{
    public class CustomerReportEntity
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public DateTime MonthTime { get; set; }
        public int TotalOrder { get; set; }
        public double TotalRevenue { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
