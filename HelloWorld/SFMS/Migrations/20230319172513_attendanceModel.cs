using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SFMS.Migrations
{
    public partial class attendanceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendance",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDte = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    IP = table.Column<string>(nullable: true),
                    AttendaceDate = table.Column<DateTime>(nullable: false),
                    InTime = table.Column<string>(nullable: true),
                    OutTime = table.Column<string>(nullable: true),
                    IsLate = table.Column<bool>(nullable: false),
                    IsLeave = table.Column<bool>(nullable: false),
                    StudentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendance_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_AttendaceDate",
                table: "Attendance",
                column: "AttendaceDate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_StudentId",
                table: "Attendance",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendance");
        }
    }
}
