using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Entities.Query;
using TFW.Framework.i18n.Extensions;

namespace TFW.Framework.CQRSExamples.Models.Notification
{
    public class CreateOrderEvent : INotification
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime Time { get; set; }

        public virtual CustomerModel Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public class CustomerModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public DateTime CreatedTime { get; set; }
        }

        public class OrderItem
        {
            public string ProductId { get; set; }
            public string OrderId { get; set; }
            public int Quantity { get; set; }
            public double UnitPrice { get; set; }
        }
    }

    public class CreateOrderEventHandler : INotificationHandler<CreateOrderEvent>
    {
        private readonly QueryDbContext _queryDbContext;

        public CreateOrderEventHandler(QueryDbContext queryDbContext)
        {
            _queryDbContext = queryDbContext;
        }

        // [WARNING] Synchronization, Eventual consistency
        public async Task Handle(CreateOrderEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await UpdateOrderReport(notification);
                await UpdateProductReport(notification);
                await UpdateCustomerReport(notification);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task UpdateOrderReport(CreateOrderEvent notification)
        {
            var monthTime = notification.Time.GetMonthStart();
            var report = await _queryDbContext.OrderReports.FirstOrDefaultAsync(o => o.MonthTime == monthTime);
            var isUpdate = report != null;

            if (report == null)
                report = new OrderReportEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    MonthTime = monthTime,
                    TotalAmount = 0,
                    TotalCustomer = 0,
                    TotalOrderCount = 0
                };

            report.TotalAmount += notification.OrderItems.Sum(o => o.Quantity * o.UnitPrice);
            report.TotalOrderCount += 1;
            report.LastUpdatedTime = DateTime.UtcNow;

            if (!await _queryDbContext.CustomerReports.AnyAsync(o => o.MonthTime == monthTime
                && o.CustomerId == notification.CustomerId))
                report.TotalCustomer += 1;

            if (!isUpdate)
                _queryDbContext.Add(report);

            await _queryDbContext.SaveChangesAsync();
        }

        private async Task UpdateCustomerReport(CreateOrderEvent notification)
        {
            var monthTime = notification.Time.GetMonthStart();
            var report = await _queryDbContext.CustomerReports.FirstOrDefaultAsync(o =>
                o.MonthTime == monthTime && o.CustomerId == notification.CustomerId);
            var isUpdate = report != null;

            if (report == null)
                report = new CustomerReportEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    MonthTime = monthTime,
                    TotalOrder = 0,
                    TotalRevenue = 0,
                    CustomerId = notification.CustomerId,
                };

            report.TotalOrder += 1;
            report.TotalRevenue += notification.OrderItems.Sum(o => o.Quantity * o.UnitPrice);
            report.LastUpdatedTime = DateTime.UtcNow;

            if (!isUpdate)
                _queryDbContext.Add(report);

            await _queryDbContext.SaveChangesAsync();
        }

        private async Task UpdateProductReport(CreateOrderEvent notification)
        {
            var monthTime = notification.Time.GetMonthStart();

            foreach (var item in notification.OrderItems)
            {
                var report = await _queryDbContext.ProductReports.FirstOrDefaultAsync(o =>
                o.MonthTime == monthTime && o.ProductId == item.ProductId);

                var isUpdate = report != null;

                if (report == null)
                    report = new ProductReportEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = item.ProductId,
                        MonthTime = monthTime,
                        TotalRevenue = 0,
                        TotalQuantity = 0
                    };

                report.TotalRevenue += item.Quantity * item.UnitPrice;
                report.TotalQuantity += item.Quantity;
                report.LastUpdatedTime = DateTime.UtcNow;

                if (!isUpdate)
                    _queryDbContext.Add(report);
            }

            await _queryDbContext.SaveChangesAsync();
        }
    }
}
