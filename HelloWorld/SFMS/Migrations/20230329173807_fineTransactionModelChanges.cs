using Microsoft.EntityFrameworkCore.Migrations;

namespace SFMS.Migrations
{
    public partial class fineTransactionModelChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FineAmount",
                table: "FineTransaction",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InTime",
                table: "FineTransaction",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutTime",
                table: "FineTransaction",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FineAmount",
                table: "FineTransaction");

            migrationBuilder.DropColumn(
                name: "InTime",
                table: "FineTransaction");

            migrationBuilder.DropColumn(
                name: "OutTime",
                table: "FineTransaction");
        }
    }
}
