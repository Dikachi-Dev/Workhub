using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newcolumn6seeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AmountInDollars = table.Column<double>(type: "float", nullable: false),
                    AmountInNaira = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "AmountInDollars", "AmountInNaira", "IsEnabled" },
                values: new object[] { new Guid("39b65e13-3fb4-47a6-8e74-f82ca485b406"), 1.0, 1300.0, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");
        }
    }
}
