using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class FixRepairmanFormRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RepairmanFormDetails_RepairmanFormId",
                table: "RepairmanFormDetails");

            migrationBuilder.CreateIndex(
                name: "IX_RepairmanFormDetails_RepairmanFormId",
                table: "RepairmanFormDetails",
                column: "RepairmanFormId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RepairmanFormDetails_RepairmanFormId",
                table: "RepairmanFormDetails");

            migrationBuilder.CreateIndex(
                name: "IX_RepairmanFormDetails_RepairmanFormId",
                table: "RepairmanFormDetails",
                column: "RepairmanFormId");
        }
    }
}
