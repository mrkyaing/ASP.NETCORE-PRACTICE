using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFMS.Migrations
{
    /// <inheritdoc />
    public partial class TeacherEntityWithNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TeacherCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "Teacher",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Teacher",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LinkedinUrl",
                table: "Teacher",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterUrl",
                table: "Teacher",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Student",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FineTransaction",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FinePolicy",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Course",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Batch",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Attendance",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TeacherCourses");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "Teacher");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Teacher");

            migrationBuilder.DropColumn(
                name: "LinkedinUrl",
                table: "Teacher");

            migrationBuilder.DropColumn(
                name: "TwitterUrl",
                table: "Teacher");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FineTransaction");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FinePolicy");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Batch");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Attendance");
        }
    }
}
