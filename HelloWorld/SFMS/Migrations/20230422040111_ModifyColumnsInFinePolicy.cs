using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFMS.Migrations
{
    /// <inheritdoc />
    public partial class ModifyColumnsInFinePolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinePolicy_Batch_BathId",
                table: "FinePolicy");

            migrationBuilder.RenameColumn(
                name: "BathId",
                table: "FinePolicy",
                newName: "BatchId");

            migrationBuilder.RenameIndex(
                name: "IX_FinePolicy_BathId",
                table: "FinePolicy",
                newName: "IX_FinePolicy_BatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinePolicy_Batch_BatchId",
                table: "FinePolicy",
                column: "BatchId",
                principalTable: "Batch",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinePolicy_Batch_BatchId",
                table: "FinePolicy");

            migrationBuilder.RenameColumn(
                name: "BatchId",
                table: "FinePolicy",
                newName: "BathId");

            migrationBuilder.RenameIndex(
                name: "IX_FinePolicy_BatchId",
                table: "FinePolicy",
                newName: "IX_FinePolicy_BathId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinePolicy_Batch_BathId",
                table: "FinePolicy",
                column: "BathId",
                principalTable: "Batch",
                principalColumn: "Id");
        }
    }
}
