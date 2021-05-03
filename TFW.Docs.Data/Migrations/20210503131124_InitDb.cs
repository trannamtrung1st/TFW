using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TFW.Docs.Data.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    LastModifiedTime = table.Column<DateTime>(nullable: true),
                    LastModifiedUserId = table.Column<int>(nullable: true),
                    DeletedTime = table.Column<DateTime>(nullable: true),
                    DeletedUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    FullName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 100, nullable: false),
                    ProviderDisplayName = table.Column<string>(maxLength: 256, nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    DefaultLocalizationId = table.Column<int>(nullable: true),
                    StartingPostId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategory", x => x.Id);
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
                    Region = table.Column<string>(unicode: false, maxLength: 2, nullable: false),
                    EntityId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 256, nullable: false),
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
                    Region = table.Column<string>(unicode: false, maxLength: 2, nullable: false),
                    EntityId = table.Column<int>(nullable: false),
                    PostIndex = table.Column<string>(maxLength: 256, nullable: false),
                    Title = table.Column<string>(maxLength: 256, nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLocalization", x => x.Id);
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
                    DefaultLocalizationId = table.Column<int>(nullable: true),
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
                        name: "FK_Post_PostLocalization_DefaultLocalizationId",
                        column: x => x.DefaultLocalizationId,
                        principalTable: "PostLocalization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Post_Post_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 1, "5bef6349-ca9c-4f9c-9fc1-aef6bfcb1260", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Post_CategoryId",
                table: "Post",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_DefaultLocalizationId",
                table: "Post",
                column: "DefaultLocalizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_ParentId",
                table: "Post",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostCategory_DefaultLocalizationId",
                table: "PostCategory",
                column: "DefaultLocalizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PostCategory_StartingPostId",
                table: "PostCategory",
                column: "StartingPostId");

            migrationBuilder.CreateIndex(
                name: "UI_EntityId_Lang_Region",
                table: "PostCategoryLocalization",
                columns: new[] { "EntityId", "Lang", "Region" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UI_EntityId_Lang_Region",
                table: "PostLocalization",
                columns: new[] { "EntityId", "Lang", "Region" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategory_PostCategoryLocalization_DefaultLocalizationId",
                table: "PostCategory",
                column: "DefaultLocalizationId",
                principalTable: "PostCategoryLocalization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategory_Post_StartingPostId",
                table: "PostCategory",
                column: "StartingPostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostLocalization_Post_EntityId",
                table: "PostLocalization",
                column: "EntityId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_PostCategory_CategoryId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCategoryLocalization_PostCategory_EntityId",
                table: "PostCategoryLocalization");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_PostLocalization_DefaultLocalizationId",
                table: "Post");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PostCategory");

            migrationBuilder.DropTable(
                name: "PostCategoryLocalization");

            migrationBuilder.DropTable(
                name: "PostLocalization");

            migrationBuilder.DropTable(
                name: "Post");
        }
    }
}
