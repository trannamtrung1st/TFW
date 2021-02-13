using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;

namespace TFW.Data.Core
{
    public partial class DataContextCore : DataContext
    {
        public DataContextCore()
        {
        }

        public DataContextCore(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable(DataConsts.ConnStrVarName));
                //require Microsoft.EntityFrameworkCore.Proxies
                //.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(e => e.Id)
                    .IsUnicode(false)
                    .HasMaxLength(100);
            });
            modelBuilder.Entity<AppRole>(entity =>
            {
                entity.Property(e => e.Id)
                    .IsUnicode(false)
                    .HasMaxLength(100);
            });
            modelBuilder.Entity<Note>(entity =>
            {
                entity.Property(e => e.Title).IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.Content)
                    .IsUnicode();
                entity.HasOne(e => e.CreatedUser)
                    .WithMany(e => e.Notes)
                    .HasForeignKey(e => e.CreatedUserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Note_AppUser");
                entity.HasOne(e => e.Category)
                    .WithMany(e => e.Notes)
                    .HasForeignKey(e => e.CategoryName)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Note_Category");
            });
            modelBuilder.Entity<NoteCategory>(entity =>
            {
                entity.Property(e => e.Name).IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.Description)
                    .HasMaxLength(1000);
            });
        }
    }
}
