using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class QuitarTipoMuestraDePrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prueba_tipo",
                table: "Prueba");

            migrationBuilder.DropIndex(
                name: "IX_Prueba_tipo_muestra_asociada",
                table: "Prueba");

            migrationBuilder.DropIndex(
                name: "uk_prueba",
                table: "Prueba");

            migrationBuilder.DropColumn(
                name: "tipo_muestra_asociada",
                table: "Prueba");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "tipo_muestra_asociada",
                table: "Prueba",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_Prueba_tipo_muestra_asociada",
                table: "Prueba",
                column: "tipo_muestra_asociada");

            migrationBuilder.CreateIndex(
                name: "uk_prueba",
                table: "Prueba",
                columns: new[] { "nombre_prueba", "tipo_muestra_asociada" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_prueba_tipo",
                table: "Prueba",
                column: "tipo_muestra_asociada",
                principalTable: "Tipo_Muestra",
                principalColumn: "tpmst_id");
        }
    }
}
