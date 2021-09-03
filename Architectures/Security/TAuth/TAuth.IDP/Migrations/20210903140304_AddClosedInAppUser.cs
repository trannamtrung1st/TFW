using Microsoft.EntityFrameworkCore.Migrations;

namespace TAuth.IDP.Migrations
{
    public partial class AddClosedInAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Closed",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Closed",
                table: "AspNetUsers");
        }
    }
}
