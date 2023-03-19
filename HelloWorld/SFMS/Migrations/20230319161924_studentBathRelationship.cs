using Microsoft.EntityFrameworkCore.Migrations;

namespace SFMS.Migrations
{
    public partial class studentBathRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BathId",
                table: "Student",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_BathId",
                table: "Student",
                column: "BathId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Batch_BathId",
                table: "Student",
                column: "BathId",
                principalTable: "Batch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Batch_BathId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_BathId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "BathId",
                table: "Student");
        }
    }
}
