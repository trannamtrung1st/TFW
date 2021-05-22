﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TFW.Framework.CQRSExamples.Entities.Query;

namespace TFW.Framework.CQRSExamples.Migrations.QueryDb
{
    [DbContext(typeof(QueryDbContext))]
    partial class QueryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.15");

            modelBuilder.Entity("TFW.Framework.CQRSExamples.Entities.Query.CustomerReportEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("CustomerId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdatedTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("MonthTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalOrder")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TotalRevenue")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("CustomerReports");
                });

            modelBuilder.Entity("TFW.Framework.CQRSExamples.Entities.Query.OrderReportEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdatedTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("MonthTime")
                        .HasColumnType("TEXT");

                    b.Property<double>("TotalAmount")
                        .HasColumnType("REAL");

                    b.Property<int>("TotalCustomer")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalOrderCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("OrderReports");
                });

            modelBuilder.Entity("TFW.Framework.CQRSExamples.Entities.Query.ProductReportEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdatedTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("MonthTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TotalRevenue")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("ProductReports");
                });
#pragma warning restore 612, 618
        }
    }
}
