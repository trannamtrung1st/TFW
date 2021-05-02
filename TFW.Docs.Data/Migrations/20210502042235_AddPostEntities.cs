using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TFW.Docs.Data.Migrations
{
    public partial class AddPostEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRole_AppRole_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRole_AppUser_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.CreateTable(
                name: "PostCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeletedTime = table.Column<DateTime>(nullable: true),
                    DeletedUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    LastModifiedTime = table.Column<DateTime>(nullable: true),
                    LastModifiedUserId = table.Column<int>(nullable: true),
                    StartingPostId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeletedTime = table.Column<DateTime>(nullable: true),
                    DeletedUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    LastModifiedTime = table.Column<DateTime>(nullable: true),
                    LastModifiedUserId = table.Column<int>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_PostCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "PostCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Post_Post_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostCategoryLocalization",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    LastModifiedTime = table.Column<DateTime>(nullable: true),
                    LastModifiedUserId = table.Column<int>(nullable: true),
                    Lang = table.Column<string>(unicode: false, maxLength: 2, nullable: false),
                    Region = table.Column<string>(unicode: false, maxLength: 2, nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    EntityId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategoryLocalization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostCategoryLocalization_PostCategory_EntityId",
                        column: x => x.EntityId,
                        principalTable: "PostCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostLocalization",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    LastModifiedTime = table.Column<DateTime>(nullable: true),
                    LastModifiedUserId = table.Column<int>(nullable: true),
                    Lang = table.Column<string>(unicode: false, maxLength: 2, nullable: false),
                    Region = table.Column<string>(unicode: false, maxLength: 2, nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    EntityId = table.Column<int>(nullable: false),
                    Index = table.Column<string>(nullable: true),
                    Title = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLocalization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostLocalization_Post_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "56612923-ec7f-4c26-aff0-6617ba1e8af6");

            migrationBuilder.CreateIndex(
                name: "IX_Post_CategoryId",
                table: "Post",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_ParentId",
                table: "Post",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostCategory_StartingPostId",
                table: "PostCategory",
                column: "StartingPostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostCategoryLocalization_EntityId",
                table: "PostCategoryLocalization",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLocalization_EntityId",
                table: "PostLocalization",
                column: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategory_Post_StartingPostId",
                table: "PostCategory",
                column: "StartingPostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_PostCategory_CategoryId",
                table: "Post");

            migrationBuilder.DropTable(
                name: "PostCategoryLocalization");

            migrationBuilder.DropTable(
                name: "PostLocalization");

            migrationBuilder.DropTable(
                name: "PostCategory");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "cdb8e41f-56d7-400b-b633-3b98117d3b85");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRole_AppRole_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRole_AppUser_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
