using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pgvector;

#nullable disable

namespace ShopxBase.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserBehaviors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserBehaviors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    SessionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BehaviorType = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    BrandId = table.Column<int>(type: "integer", nullable: true),
                    SearchQuery = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RatingScore = table.Column<int>(type: "integer", nullable: true),
                    DwellTimeSeconds = table.Column<int>(type: "integer", nullable: true),
                    SourcePage = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBehaviors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBehaviors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserBehaviors_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserBehaviors_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserBehaviors_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBehaviors_BehaviorType",
                table: "UserBehaviors",
                column: "BehaviorType");

            migrationBuilder.CreateIndex(
                name: "IX_UserBehaviors_BrandId",
                table: "UserBehaviors",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBehaviors_CategoryId",
                table: "UserBehaviors",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBehaviors_CreatedAt",
                table: "UserBehaviors",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserBehaviors_ProductId",
                table: "UserBehaviors",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBehaviors_SessionId",
                table: "UserBehaviors",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBehaviors_UserId",
                table: "UserBehaviors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBehaviors_UserId_BehaviorType",
                table: "UserBehaviors",
                columns: new[] { "UserId", "BehaviorType" });

            migrationBuilder.CreateIndex(
                name: "IX_UserBehaviors_UserId_ProductId",
                table: "UserBehaviors",
                columns: new[] { "UserId", "ProductId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBehaviors");
        }
    }
}
