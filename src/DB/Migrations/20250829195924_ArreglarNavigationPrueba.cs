using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class ArreglarNavigationPrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prueba_Muestra_IdMuestraNavigationMstCodigo",
                table: "Prueba");

            migrationBuilder.DropIndex(
                name: "IX_Prueba_IdMuestraNavigationMstCodigo",
                table: "Prueba");

            migrationBuilder.DropColumn(
                name: "IdMuestraNavigationMstCodigo",
                table: "Prueba");

            migrationBuilder.CreateIndex(
                name: "IX_Prueba_id_muestra",
                table: "Prueba",
                column: "id_muestra");

            migrationBuilder.AddForeignKey(
                name: "fk_prueba_muestra",
                table: "Prueba",
                column: "id_muestra",
                principalTable: "Muestra",
                principalColumn: "MST_CODIGO",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_Prueba_IdMuestraNavigationMstCodigo",
                table: "Prueba",
                column: "IdMuestraNavigationMstCodigo");

            migrationBuilder.AddForeignKey(
                name: "FK_Prueba_Muestra_IdMuestraNavigationMstCodigo",
                table: "Prueba",
                column: "IdMuestraNavigationMstCodigo",
                principalTable: "Muestra",
                principalColumn: "MST_CODIGO",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
