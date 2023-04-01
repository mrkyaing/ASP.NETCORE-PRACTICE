using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SFMS.Migrations
{
    public partial class fineTransactionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FineTransaction",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDte = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    IP = table.Column<string>(nullable: true),
                    FinedDate = table.Column<DateTime>(nullable: false),
                    FinePolicyId = table.Column<string>(nullable: true),
                    StudentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FineTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FineTransaction_FinePolicy_FinePolicyId",
                        column: x => x.FinePolicyId,
                        principalTable: "FinePolicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FineTransaction_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FineTransaction_FinePolicyId",
                table: "FineTransaction",
                column: "FinePolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_FineTransaction_StudentId",
                table: "FineTransaction",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FineTransaction");
        }
    }
}
