using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatafix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("43047abd-f8a5-45cd-afa4-8cdf09608613"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "AmountInDollars", "AmountInNaira", "IsEnabled", "NokoKashId", "PayPalKey", "PayPalSecret" },
                values: new object[] { new Guid("2f47c3b3-89b1-4d0f-8c5c-7c212408fa12"), 1.0, 1300.0, false, "cPp6u5Ckq2nAybmk4", "", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("2f47c3b3-89b1-4d0f-8c5c-7c212408fa12"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "AmountInDollars", "AmountInNaira", "IsEnabled", "NokoKashId", "PayPalKey", "PayPalSecret" },
                values: new object[] { new Guid("43047abd-f8a5-45cd-afa4-8cdf09608613"), 1.0, 1300.0, false, "cPp6u5Ckq2nAybmk4", "", "" });
        }
    }
}
