using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newcolumn9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("05ee914f-968e-431d-8a9f-068f40c862af"));

            migrationBuilder.CreateTable(
                name: "SubHistorys",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubscriberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubHistorys", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "AmountInDollars", "AmountInNaira", "IsEnabled" },
                values: new object[] { new Guid("737334c4-9b1e-4681-97c8-3d3754e4e350"), 1.0, 1300.0, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubHistorys");

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("737334c4-9b1e-4681-97c8-3d3754e4e350"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "AmountInDollars", "AmountInNaira", "IsEnabled" },
                values: new object[] { new Guid("05ee914f-968e-431d-8a9f-068f40c862af"), 1.0, 1300.0, false });
        }
    }
}
