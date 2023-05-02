using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFMS.Migrations
{
    /// <inheritdoc />
    public partial class TeacherModelUpdateWithUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Teacher",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_UserId",
                table: "Teacher",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_AspNetUsers_UserId",
                table: "Teacher",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_AspNetUsers_UserId",
                table: "Teacher");

            migrationBuilder.DropIndex(
                name: "IX_Teacher_UserId",
                table: "Teacher");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Teacher");
        }
    }
}
