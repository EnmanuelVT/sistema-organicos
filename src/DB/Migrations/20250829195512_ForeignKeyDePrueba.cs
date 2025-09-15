using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyDePrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parametro_Norma_Tipo_Muestra_IdTipoMuestraNavigationTpmstId",
                table: "Parametro_Norma");

            migrationBuilder.DropForeignKey(
                name: "fk_parametro_prueba",
                table: "Parametro_Norma");

            migrationBuilder.DropIndex(
                name: "IX_Parametro_Norma_IdTipoMuestraNavigationTpmstId",
                table: "Parametro_Norma");

            migrationBuilder.DropColumn(
                name: "IdTipoMuestraNavigationTpmstId",
                table: "Parametro_Norma");

            migrationBuilder.RenameColumn(
                name: "IdParametroNorma",
                table: "Prueba",
                newName: "id_parametro_norma");

            migrationBuilder.AddForeignKey(
                name: "fk_prueba_parametro",
                table: "Parametro_Norma",
                column: "id_prueba",
                principalTable: "Prueba",
                principalColumn: "id_prueba");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prueba_parametro",
                table: "Parametro_Norma");

            migrationBuilder.RenameColumn(
                name: "id_parametro_norma",
                table: "Prueba",
                newName: "IdParametroNorma");

            migrationBuilder.AddColumn<byte>(
                name: "IdTipoMuestraNavigationTpmstId",
                table: "Parametro_Norma",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_Parametro_Norma_IdTipoMuestraNavigationTpmstId",
                table: "Parametro_Norma",
                column: "IdTipoMuestraNavigationTpmstId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parametro_Norma_Tipo_Muestra_IdTipoMuestraNavigationTpmstId",
                table: "Parametro_Norma",
                column: "IdTipoMuestraNavigationTpmstId",
                principalTable: "Tipo_Muestra",
                principalColumn: "TPMST_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_parametro_prueba",
                table: "Parametro_Norma",
                column: "id_prueba",
                principalTable: "Prueba",
                principalColumn: "id_prueba");
        }
    }
}
