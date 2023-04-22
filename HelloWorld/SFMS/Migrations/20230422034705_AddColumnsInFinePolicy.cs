using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFMS.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsInFinePolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherCourses_Course_CourseId",
                table: "TeacherCourses");

            migrationBuilder.DropIndex(
                name: "IX_TeacherCourses_CourseId",
                table: "TeacherCourses");

            migrationBuilder.AlterColumn<string>(
                name: "CourseId",
                table: "TeacherCourses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BathId",
                table: "FinePolicy",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnable",
                table: "FinePolicy",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CourseId",
                table: "Course",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinePolicy_BathId",
                table: "FinePolicy",
                column: "BathId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_CourseId",
                table: "Course",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_TeacherCourses_CourseId",
                table: "Course",
                column: "CourseId",
                principalTable: "TeacherCourses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FinePolicy_Batch_BathId",
                table: "FinePolicy",
                column: "BathId",
                principalTable: "Batch",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_TeacherCourses_CourseId",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_FinePolicy_Batch_BathId",
                table: "FinePolicy");

            migrationBuilder.DropIndex(
                name: "IX_FinePolicy_BathId",
                table: "FinePolicy");

            migrationBuilder.DropIndex(
                name: "IX_Course_CourseId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "BathId",
                table: "FinePolicy");

            migrationBuilder.DropColumn(
                name: "IsEnable",
                table: "FinePolicy");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Course");

            migrationBuilder.AlterColumn<string>(
                name: "CourseId",
                table: "TeacherCourses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherCourses_CourseId",
                table: "TeacherCourses",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherCourses_Course_CourseId",
                table: "TeacherCourses",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");
        }
    }
}
