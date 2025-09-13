using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AnadirParametroAResultadoPrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_parametro",
                table: "Resultado_Prueba",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Resultado_Prueba_id_parametro",
                table: "Resultado_Prueba",
                column: "id_parametro");

            migrationBuilder.AddForeignKey(
                name: "fk_res_parametro",
                table: "Resultado_Prueba",
                column: "id_parametro",
                principalTable: "Parametro_Norma",
                principalColumn: "id_parametro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_res_parametro",
                table: "Resultado_Prueba");

            migrationBuilder.DropIndex(
                name: "IX_Resultado_Prueba_id_parametro",
                table: "Resultado_Prueba");

            migrationBuilder.DropColumn(
                name: "id_parametro",
                table: "Resultado_Prueba");
        }
    }
}
