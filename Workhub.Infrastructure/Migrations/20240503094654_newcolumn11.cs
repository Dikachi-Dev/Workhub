using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newcolumn11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("9a1cd1cc-1f14-4f94-960d-c4bc992fc185"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "ChatPosts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "AmountInDollars", "AmountInNaira", "IsEnabled" },
                values: new object[] { new Guid("33db2265-2192-4e70-a06c-eb677f2f75ad"), 1.0, 1300.0, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("33db2265-2192-4e70-a06c-eb677f2f75ad"));

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "ChatPosts");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "AmountInDollars", "AmountInNaira", "IsEnabled" },
                values: new object[] { new Guid("9a1cd1cc-1f14-4f94-960d-c4bc992fc185"), 1.0, 1300.0, false });
        }
    }
}
