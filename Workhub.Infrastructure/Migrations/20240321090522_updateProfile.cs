using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reply_ChatPosts_ChatPostId",
                table: "Reply");

            migrationBuilder.DropColumn(
                name: "LGA",
                table: "Profiles");

            migrationBuilder.AlterColumn<string>(
                name: "ChatPostId",
                table: "Reply",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileId",
                table: "Jobs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ProfileId",
                table: "Jobs",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Profiles_ProfileId",
                table: "Jobs",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reply_ChatPosts_ChatPostId",
                table: "Reply",
                column: "ChatPostId",
                principalTable: "ChatPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Profiles_ProfileId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Reply_ChatPosts_ChatPostId",
                table: "Reply");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ProfileId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Jobs");

            migrationBuilder.AlterColumn<string>(
                name: "ChatPostId",
                table: "Reply",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "LGA",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Reply_ChatPosts_ChatPostId",
                table: "Reply",
                column: "ChatPostId",
                principalTable: "ChatPosts",
                principalColumn: "Id");
        }
    }
}
