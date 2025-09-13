using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class CambiarNombreAForeignKeyIdMuestra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdMuestra",
                table: "Prueba",
                newName: "id_muestra");

            migrationBuilder.RenameIndex(
                name: "IX_Prueba_IdMuestra",
                table: "Prueba",
                newName: "IX_Prueba_id_muestra");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id_muestra",
                table: "Prueba",
                newName: "IdMuestra");

            migrationBuilder.RenameIndex(
                name: "IX_Prueba_id_muestra",
                table: "Prueba",
                newName: "IX_Prueba_IdMuestra");
        }
    }
}
