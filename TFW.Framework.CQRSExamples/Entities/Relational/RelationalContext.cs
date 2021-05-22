using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.CQRSExamples.Entities.Relational
{
    public class RelationalContext : DbContext
    {
        public RelationalContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<OrderEntity> Orders { get; set; }
        public virtual DbSet<OrderItemEntity> OrderItems { get; set; }
        public virtual DbSet<ProductEntity> Products { get; set; }
        public virtual DbSet<ProductCategoryEntity> ProductCategories { get; set; }
        public virtual DbSet<CustomerEntity> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItemEntity>(eBuilder =>
            {
                eBuilder.HasKey(e => new { e.OrderId, e.ProductId });

                eBuilder.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId);

                eBuilder.HasOne(e => e.Order)
                    .WithMany(e => e.OrderItems)
                    .HasForeignKey(e => e.OrderId);
            });

            modelBuilder.Entity<ProductEntity>(eBuilder =>
            {
                eBuilder.HasOne(e => e.Category)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.CategoryId);
            });

            modelBuilder.Entity<OrderEntity>(eBuilder =>
            {
                eBuilder.HasOne(e => e.Customer)
                    .WithMany()
                    .HasForeignKey(e => e.CustomerId);
            });
        }

    }
}
