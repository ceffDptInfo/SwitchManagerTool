using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedSwitchDb5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_SwitchDB_SwitchDBId",
                table: "Equipment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipment",
                table: "Equipment");

            migrationBuilder.RenameTable(
                name: "Equipment",
                newName: "Equipments");

            migrationBuilder.RenameIndex(
                name: "IX_Equipment_SwitchDBId",
                table: "Equipments",
                newName: "IX_Equipments_SwitchDBId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipments",
                table: "Equipments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_SwitchDB_SwitchDBId",
                table: "Equipments",
                column: "SwitchDBId",
                principalTable: "SwitchDB",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_SwitchDB_SwitchDBId",
                table: "Equipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipments",
                table: "Equipments");

            migrationBuilder.RenameTable(
                name: "Equipments",
                newName: "Equipment");

            migrationBuilder.RenameIndex(
                name: "IX_Equipments_SwitchDBId",
                table: "Equipment",
                newName: "IX_Equipment_SwitchDBId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipment",
                table: "Equipment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_SwitchDB_SwitchDBId",
                table: "Equipment",
                column: "SwitchDBId",
                principalTable: "SwitchDB",
                principalColumn: "Id");
        }
    }
}
