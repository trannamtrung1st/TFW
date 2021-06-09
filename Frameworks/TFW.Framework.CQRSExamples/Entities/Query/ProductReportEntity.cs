using System;

namespace TFW.Framework.CQRSExamples.Entities.Query
{
    public class ProductReportEntity
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public int TotalQuantity { get; set; }
        public double TotalRevenue { get; set; }
        public DateTime MonthTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
