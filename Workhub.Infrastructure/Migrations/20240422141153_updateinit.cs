using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "publicId",
                table: "Profiles",
                newName: "ProfileImage");

            migrationBuilder.AddColumn<string>(
                name: "Image1",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image2",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image1",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Image2",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "ProfileImage",
                table: "Profiles",
                newName: "publicId");
        }
    }
}
