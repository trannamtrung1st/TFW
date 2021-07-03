using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAuth.ResourceAPI.Entities
{
    public class ResourceContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public const string DefaultConnStr = "Data Source=./ResourceContext.db";

        public ResourceContext()
        {
        }

        public ResourceContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<ResourceEntity> Resources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(DefaultConnStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ResourceEntity>(builder =>
            {
                builder.HasOne(o => o.User)
                    .WithMany(o => o.Resources)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
