using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newcolumn7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("39b65e13-3fb4-47a6-8e74-f82ca485b406"));

            migrationBuilder.AddColumn<string>(
                name: "BuyerAddeess",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SellerAddress",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "AmountInDollars", "AmountInNaira", "IsEnabled" },
                values: new object[] { new Guid("05ee914f-968e-431d-8a9f-068f40c862af"), 1.0, 1300.0, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("05ee914f-968e-431d-8a9f-068f40c862af"));

            migrationBuilder.DropColumn(
                name: "BuyerAddeess",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "SellerAddress",
                table: "Jobs");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "AmountInDollars", "AmountInNaira", "IsEnabled" },
                values: new object[] { new Guid("39b65e13-3fb4-47a6-8e74-f82ca485b406"), 1.0, 1300.0, false });
        }
    }
}
