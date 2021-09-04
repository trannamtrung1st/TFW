using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Entities.Relational;
using TFW.Framework.CQRSExamples.Models.Query;

namespace TFW.Framework.CQRSExamples.Queries
{
    public class OrderQuery : IOrderQuery
    {
        private readonly RelationalContext _relationalContext;

        public OrderQuery(RelationalContext relationalContext)
        {
            _relationalContext = relationalContext;
        }

        public async Task<OrderDetail> GetOrderDetailAsync(string id)
        {
            var entity = await _relationalContext.Orders
                .Select(o => new OrderDetail
                {
                    Address = o.Address,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.Name,
                    Id = o.Id,
                    Phone = o.Phone,
                    Time = o.Time,
                    OrderItems = o.OrderItems.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.Product.Name,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    })
                }).FirstOrDefaultAsync(o => o.Id == id);

            return entity;
        }

        public async Task<IEnumerable<OrderListItem>> GetOrderListAsync()
        {
            var list = await _relationalContext.Orders.Select(order => new OrderListItem
            {
                Id = order.Id,
                Time = order.Time,
                Address = order.Address,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.Name,
                Phone = order.Phone,
                TotalAmount = order.OrderItems.Sum(item => item.Quantity * item.UnitPrice)
            }).ToArrayAsync();

            return list;
        }
    }
}
