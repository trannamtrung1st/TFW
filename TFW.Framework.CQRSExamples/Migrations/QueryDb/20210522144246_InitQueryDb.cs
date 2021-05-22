using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TFW.Framework.CQRSExamples.Migrations.QueryDb
{
    public partial class InitQueryDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerReports",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CustomerId = table.Column<string>(nullable: true),
                    MonthTime = table.Column<DateTime>(nullable: false),
                    TotalOrder = table.Column<int>(nullable: false),
                    TotalRevenue = table.Column<double>(nullable: false),
                    LastUpdatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderReports",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TotalCustomer = table.Column<int>(nullable: false),
                    TotalOrderCount = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<double>(nullable: false),
                    MonthTime = table.Column<DateTime>(nullable: false),
                    LastUpdatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductReports",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ProductId = table.Column<string>(nullable: true),
                    TotalQuantity = table.Column<int>(nullable: false),
                    TotalRevenue = table.Column<double>(nullable: false),
                    MonthTime = table.Column<DateTime>(nullable: false),
                    LastUpdatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReports", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerReports");

            migrationBuilder.DropTable(
                name: "OrderReports");

            migrationBuilder.DropTable(
                name: "ProductReports");
        }
    }
}
