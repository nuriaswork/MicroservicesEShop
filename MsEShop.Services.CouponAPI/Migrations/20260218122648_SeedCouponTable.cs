using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MsEShop.Services.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedCouponTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "CreatedOn", "DiscountAmount", "LastUpdatedOn", "MinAmount" },
                values: new object[,]
                {
                    { 1, "10OFF", new DateTime(2026, 2, 18, 13, 26, 48, 323, DateTimeKind.Local).AddTicks(553), 10.0, new DateTime(2026, 2, 18, 13, 26, 48, 323, DateTimeKind.Local).AddTicks(614), 20 },
                    { 2, "20OFF", new DateTime(2026, 2, 18, 13, 26, 48, 323, DateTimeKind.Local).AddTicks(643), 20.0, new DateTime(2026, 2, 18, 13, 26, 48, 323, DateTimeKind.Local).AddTicks(644), 40 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
