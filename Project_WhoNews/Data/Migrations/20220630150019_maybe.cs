using Microsoft.EntityFrameworkCore.Migrations;

namespace Project_WhoNews.Data.Migrations
{
    public partial class maybe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "contentId",
                table: "archiveDb1",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_archiveDb1_contentId",
                table: "archiveDb1",
                column: "contentId");

            migrationBuilder.AddForeignKey(
                name: "FK_archiveDb1_Content_contentId",
                table: "archiveDb1",
                column: "contentId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_archiveDb1_Content_contentId",
                table: "archiveDb1");

            migrationBuilder.DropIndex(
                name: "IX_archiveDb1_contentId",
                table: "archiveDb1");

            migrationBuilder.DropColumn(
                name: "contentId",
                table: "archiveDb1");
        }
    }
}
