using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newcolumn14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("a873389a-66c9-4dce-87cd-28db6d949cfa"));

            migrationBuilder.AddColumn<string>(
                name: "Image1ext",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image2ext",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "AmountInDollars", "AmountInNaira", "IsEnabled", "NokoKashId", "PayPalKey", "PayPalSecret" },
                values: new object[] { new Guid("d287ec31-f426-4e47-a8bf-09097510eb02"), 1.0, 1300.0, false, "cPp6u5Ckq2nAybmk4", "", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("d287ec31-f426-4e47-a8bf-09097510eb02"));

            migrationBuilder.DropColumn(
                name: "Image1ext",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Image2ext",
                table: "Profiles");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "AmountInDollars", "AmountInNaira", "IsEnabled", "NokoKashId", "PayPalKey", "PayPalSecret" },
                values: new object[] { new Guid("a873389a-66c9-4dce-87cd-28db6d949cfa"), 1.0, 1300.0, false, "cPp6u5Ckq2nAybmk4", "", "" });
        }
    }
}
