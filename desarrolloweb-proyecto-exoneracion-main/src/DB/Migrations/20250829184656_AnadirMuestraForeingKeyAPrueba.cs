using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AnadirMuestraForeingKeyAPrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdMuestra",
                table: "Prueba",
                type: "varchar(30)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Prueba_IdMuestra",
                table: "Prueba",
                column: "IdMuestra");

            migrationBuilder.AddForeignKey(
                name: "fk_prueba_muestra",
                table: "Prueba",
                column: "IdMuestra",
                principalTable: "Muestra",
                principalColumn: "MST_CODIGO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prueba_muestra",
                table: "Prueba");

            migrationBuilder.DropIndex(
                name: "IX_Prueba_IdMuestra",
                table: "Prueba");

            migrationBuilder.DropColumn(
                name: "IdMuestra",
                table: "Prueba");
        }
    }
}
