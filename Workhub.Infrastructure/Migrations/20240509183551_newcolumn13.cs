using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newcolumn13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("33db2265-2192-4e70-a06c-eb677f2f75ad"));

            migrationBuilder.AddColumn<string>(
                name: "NokoKashId",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PayPalKey",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PayPalSecret",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "AmountInDollars", "AmountInNaira", "IsEnabled", "NokoKashId", "PayPalKey", "PayPalSecret" },
                values: new object[] { new Guid("a873389a-66c9-4dce-87cd-28db6d949cfa"), 1.0, 1300.0, false, "cPp6u5Ckq2nAybmk4", "", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("a873389a-66c9-4dce-87cd-28db6d949cfa"));

            migrationBuilder.DropColumn(
                name: "NokoKashId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "PayPalKey",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "PayPalSecret",
                table: "Subscriptions");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "AmountInDollars", "AmountInNaira", "IsEnabled" },
                values: new object[] { new Guid("33db2265-2192-4e70-a06c-eb677f2f75ad"), 1.0, 1300.0, false });
        }
    }
}
