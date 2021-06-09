using System;

namespace TFW.Framework.CQRSExamples.Entities.Query
{
    public class OrderReportEntity
    {
        public string Id { get; set; }
        public int TotalCustomer { get; set; }
        public int TotalOrderCount { get; set; }
        public double TotalAmount { get; set; }
        public DateTime MonthTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
