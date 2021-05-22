using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.CQRSExamples.Models.Query
{
    public interface IReportQuery
    {
        Task<IEnumerable<OrderReportListItem>> GetOrderReportListAsync(
            int fromMonth, int fromYear, int toMonth, int toYear);

        Task<IEnumerable<ProductReportListItem>> GetProductReportListAsync(
            int fromMonth, int fromYear, int toMonth, int toYear);

        Task<IEnumerable<CustomerReportListItem>> GetCustomerReportListAsync(
            int fromMonth, int fromYear, int toMonth, int toYear);
    }

    public class OrderReportListItem
    {
        public string Id { get; set; }
        public int TotalCustomer { get; set; }
        public int TotalOrderCount { get; set; }
        public double TotalAmount { get; set; }
        public DateTime MonthTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }

    public class ProductReportListItem
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantity { get; set; }
        public double TotalRevenue { get; set; }
        public DateTime MonthTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }

    public class CustomerReportListItem
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime MonthTime { get; set; }
        public int TotalOrder { get; set; }
        public double TotalRevenue { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
