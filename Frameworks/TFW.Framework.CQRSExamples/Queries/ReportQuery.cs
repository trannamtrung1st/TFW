using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.CQRSExamples.Entities.Query;
using TFW.Framework.CQRSExamples.Entities.Relational;
using TFW.Framework.CQRSExamples.Models.Query;
using TFW.Framework.i18n.Extensions;

namespace TFW.Framework.CQRSExamples.Queries
{
    public class ReportQuery : IReportQuery
    {
        private readonly QueryDbContext _queryDbContext;
        private readonly RelationalContext _relationalContext;

        public ReportQuery(QueryDbContext queryDbContext,
            RelationalContext relationalContext)
        {
            _queryDbContext = queryDbContext;
            _relationalContext = relationalContext;
        }

        public async Task<IEnumerable<CustomerReportListItem>> GetCustomerReportListAsync(
            int fromMonth, int fromYear, int toMonth, int toYear)
        {
            DateTime from = new DateTime(fromYear, fromMonth, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime to = new DateTime(toYear, toMonth, 1, 0, 0, 0, DateTimeKind.Utc).GetMonthEnd();

#if true
            var reportList = await _queryDbContext.CustomerReports
                .Where(o => o.MonthTime >= from && o.MonthTime <= to)
                .Select(o => new CustomerReportListItem
                {
                    Id = o.Id,
                    LastUpdatedTime = o.LastUpdatedTime,
                    MonthTime = o.MonthTime,
                    TotalRevenue = o.TotalRevenue,
                    CustomerId = o.CustomerId,
                    TotalOrder = o.TotalOrder
                }).ToArrayAsync();

            var customerIds = reportList.Select(o => o.CustomerId).ToArray();

            var customerNameMap = await _relationalContext.Customers.Where(o => customerIds.Contains(o.Id))
                .ToDictionaryAsync(o => o.Id, o => o.Name);

            foreach (var report in reportList)
                report.CustomerName = customerNameMap.GetValueOrDefault(report.CustomerId, "[Deleted]");
#else
            var totalOrderQuery = _relationalContext.Orders
                .Where(o => o.Time >= from && o.Time <= to)
                .GroupBy(o => new { o.CustomerId, CustomerName = o.Customer.Name, o.Time.Month, o.Time.Year }).Select(o => new
                {
                    CustomerId = o.Key.CustomerId,
                    CustomerName = o.Key.CustomerName,
                    Month = o.Key.Month,
                    Year = o.Key.Year,
                    TotalOrder = o.Count()
                });

            var totalRevenueQuery = _relationalContext.OrderItems
                .Where(o => o.Order.Time >= from && o.Order.Time <= to)
                .GroupBy(o => new { o.Order.CustomerId, o.Order.Time.Month, o.Order.Time.Year }).Select(o => new
                {
                    CustomerId = o.Key.CustomerId,
                    Month = o.Key.Month,
                    Year = o.Key.Year,
                    TotalRevenue = o.Sum(item => item.Quantity * item.UnitPrice)
                });

            var reportList = await (from totalOrder in totalOrderQuery
                                    join totalRevenue in totalRevenueQuery
                                    on new { totalOrder.CustomerId, totalOrder.Month, totalOrder.Year }
                                    equals new { totalRevenue.CustomerId, totalRevenue.Month, totalRevenue.Year }
                                    select new CustomerReportListItem
                                    {
                                        CustomerId = totalOrder.CustomerId,
                                        CustomerName = totalOrder.CustomerName,
                                        Id = "",
                                        LastUpdatedTime = DateTime.UtcNow,
                                        MonthTime = new DateTime(totalOrder.Year, totalOrder.Month, 1),
                                        TotalOrder = totalOrder.TotalOrder,
                                        TotalRevenue = totalRevenue.TotalRevenue
                                    }).ToArrayAsync();
#endif

            return reportList;
        }

        public async Task<IEnumerable<OrderReportListItem>> GetOrderReportListAsync(
            int fromMonth, int fromYear, int toMonth, int toYear)
        {
            DateTime from = new DateTime(fromYear, fromMonth, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime to = new DateTime(toYear, toMonth, 1, 0, 0, 0, DateTimeKind.Utc).GetMonthEnd();

            var list = await _queryDbContext.OrderReports
                .Where(o => o.MonthTime >= from && o.MonthTime <= to)
                .Select(o => new OrderReportListItem
                {
                    Id = o.Id,
                    LastUpdatedTime = o.LastUpdatedTime,
                    MonthTime = o.MonthTime,
                    TotalAmount = o.TotalAmount,
                    TotalCustomer = o.TotalCustomer,
                    TotalOrderCount = o.TotalOrderCount,
                }).ToArrayAsync();

            return list;
        }

        public async Task<IEnumerable<ProductReportListItem>> GetProductReportListAsync(
            int fromMonth, int fromYear, int toMonth, int toYear)
        {
            DateTime from = new DateTime(fromYear, fromMonth, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime to = new DateTime(toYear, toMonth, 1, 0, 0, 0, DateTimeKind.Utc).GetMonthEnd();

            var reportList = await _queryDbContext.ProductReports
                .Where(o => o.MonthTime >= from && o.MonthTime <= to)
                .Select(o => new ProductReportListItem
                {
                    Id = o.Id,
                    LastUpdatedTime = o.LastUpdatedTime,
                    MonthTime = o.MonthTime,
                    ProductId = o.ProductId,
                    TotalQuantity = o.TotalQuantity,
                    TotalRevenue = o.TotalRevenue
                }).ToArrayAsync();

            var productIds = reportList.Select(o => o.ProductId).ToArray();

            var productNameMap = await _relationalContext.Products.Where(o => productIds.Contains(o.Id))
                .ToDictionaryAsync(o => o.Id, o => o.Name);

            foreach (var report in reportList)
                report.ProductName = productNameMap.GetValueOrDefault(report.ProductId, "[Deleted]");

            return reportList;
        }
    }
}
