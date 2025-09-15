using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class HacerPruebaParametroUnoAUno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_parnorma_prueba",
                table: "Parametro_Norma");

            migrationBuilder.DropForeignKey(
                name: "fk_prueba_muestra",
                table: "Prueba");

            migrationBuilder.DropIndex(
                name: "IX_Prueba_id_muestra",
                table: "Prueba");

            migrationBuilder.AddColumn<string>(
                name: "IdMuestraNavigationMstCodigo",
                table: "Prueba",
                type: "varchar(30)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IdParametroNorma",
                table: "Prueba",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "IdTipoMuestraNavigationTpmstId",
                table: "Parametro_Norma",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "tpmst_id",
                table: "Parametro_Norma",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_Prueba_IdMuestraNavigationMstCodigo",
                table: "Prueba",
                column: "IdMuestraNavigationMstCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_Parametro_Norma_id_prueba",
                table: "Parametro_Norma",
                column: "id_prueba",
                unique: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Prueba_Muestra_IdMuestraNavigationMstCodigo",
                table: "Prueba",
                column: "IdMuestraNavigationMstCodigo",
                principalTable: "Muestra",
                principalColumn: "MST_CODIGO",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parametro_Norma_Tipo_Muestra_IdTipoMuestraNavigationTpmstId",
                table: "Parametro_Norma");

            migrationBuilder.DropForeignKey(
                name: "fk_parametro_prueba",
                table: "Parametro_Norma");

            migrationBuilder.DropForeignKey(
                name: "FK_Prueba_Muestra_IdMuestraNavigationMstCodigo",
                table: "Prueba");

            migrationBuilder.DropIndex(
                name: "IX_Prueba_IdMuestraNavigationMstCodigo",
                table: "Prueba");

            migrationBuilder.DropIndex(
                name: "IX_Parametro_Norma_id_prueba",
                table: "Parametro_Norma");

            migrationBuilder.DropIndex(
                name: "IX_Parametro_Norma_IdTipoMuestraNavigationTpmstId",
                table: "Parametro_Norma");

            migrationBuilder.DropColumn(
                name: "IdMuestraNavigationMstCodigo",
                table: "Prueba");

            migrationBuilder.DropColumn(
                name: "IdParametroNorma",
                table: "Prueba");

            migrationBuilder.DropColumn(
                name: "IdTipoMuestraNavigationTpmstId",
                table: "Parametro_Norma");

            migrationBuilder.DropColumn(
                name: "tpmst_id",
                table: "Parametro_Norma");

            migrationBuilder.CreateIndex(
                name: "IX_Prueba_id_muestra",
                table: "Prueba",
                column: "id_muestra");

            migrationBuilder.AddForeignKey(
                name: "fk_parnorma_prueba",
                table: "Parametro_Norma",
                column: "id_prueba",
                principalTable: "Prueba",
                principalColumn: "id_prueba");

            migrationBuilder.AddForeignKey(
                name: "fk_prueba_muestra",
                table: "Prueba",
                column: "id_muestra",
                principalTable: "Muestra",
                principalColumn: "MST_CODIGO");
        }
    }
}
