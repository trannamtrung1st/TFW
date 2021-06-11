using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Sales
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public virtual void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Date)
                .IsRequired();

            builder.HasOne(p => p.Customer).WithMany().IsRequired();

            builder.HasOne(p => p.Employee).WithMany().IsRequired();

            builder.HasOne(p => p.Product).WithMany().IsRequired();

            builder.Property(p => p.TotalPrice)
                .IsRequired();
        }
    }
}
