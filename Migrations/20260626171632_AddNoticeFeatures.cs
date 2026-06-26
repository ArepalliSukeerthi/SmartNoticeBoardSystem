using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartNoticeBoardSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddNoticeFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DownloadCount",
                table: "Notices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPinned",
                table: "Notices",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Notices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Notices",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadCount",
                table: "Notices");

            migrationBuilder.DropColumn(
                name: "IsPinned",
                table: "Notices");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Notices");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Notices");
        }
    }
}
