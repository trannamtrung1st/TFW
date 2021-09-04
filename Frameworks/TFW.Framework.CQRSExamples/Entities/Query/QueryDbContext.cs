using Microsoft.EntityFrameworkCore;

namespace TFW.Framework.CQRSExamples.Entities.Query
{
    public class QueryDbContext : DbContext
    {
        public QueryDbContext(DbContextOptions<QueryDbContext> options) : base(options)
        {
        }

        public virtual DbSet<OrderReportEntity> OrderReports { get; set; }
        public virtual DbSet<ProductReportEntity> ProductReports { get; set; }
        public virtual DbSet<CustomerReportEntity> CustomerReports { get; set; }
    }
}
