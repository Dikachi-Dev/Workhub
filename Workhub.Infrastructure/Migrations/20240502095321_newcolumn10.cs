using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newcolumn10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("737334c4-9b1e-4681-97c8-3d3754e4e350"));

            migrationBuilder.AddColumn<bool>(
                name: "IsRated",
                table: "Jobs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "AmountInDollars", "AmountInNaira", "IsEnabled" },
                values: new object[] { new Guid("9a1cd1cc-1f14-4f94-960d-c4bc992fc185"), 1.0, 1300.0, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("9a1cd1cc-1f14-4f94-960d-c4bc992fc185"));

            migrationBuilder.DropColumn(
                name: "IsRated",
                table: "Jobs");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "AmountInDollars", "AmountInNaira", "IsEnabled" },
                values: new object[] { new Guid("737334c4-9b1e-4681-97c8-3d3754e4e350"), 1.0, 1300.0, false });
        }
    }
}
