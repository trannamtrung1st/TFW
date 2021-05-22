using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.CQRSExamples.Models.Query
{
    public interface IOrderQuery
    {
        Task<IEnumerable<OrderListItem>> GetOrderListAsync();
        Task<OrderDetail> GetOrderDetailAsync(string id);
    }

    public class OrderDetail
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double TotalAmount => OrderItems.Sum(o => o.TotalAmount);
        public DateTimeOffset Time { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalAmount => Quantity * UnitPrice;
    }

    public class OrderListItem
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double TotalAmount { get; set; }
        public DateTimeOffset Time { get; set; }
    }

}
