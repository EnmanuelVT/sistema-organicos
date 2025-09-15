using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class ReworkDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_muestra_analista",
                table: "Muestra");

            migrationBuilder.DropForeignKey(
                name: "fk_prueba_parametro",
                table: "Parametro_Norma");

            migrationBuilder.DropIndex(
                name: "IX_Parametro_Norma_id_prueba",
                table: "Parametro_Norma");

            migrationBuilder.DropIndex(
                name: "uk_parametro",
                table: "Parametro_Norma");

            migrationBuilder.DropIndex(
                name: "IX_Muestra_id_Analista",
                table: "Muestra");

            migrationBuilder.DropColumn(
                name: "id_parametro_norma",
                table: "Prueba");

            migrationBuilder.DropColumn(
                name: "id_prueba",
                table: "Parametro_Norma");

            migrationBuilder.DropColumn(
                name: "id_Analista",
                table: "Muestra");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Muestra",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_estado_documento",
                table: "Documento",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Estado_Documento",
                columns: table => new
                {
                    id_estado_documento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Estado_D__5D9B2B2D1CBBE2F3", x => x.id_estado_documento);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Muestra_UsuarioId",
                table: "Muestra",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_id_estado_documento",
                table: "Documento",
                column: "id_estado_documento");

            migrationBuilder.CreateIndex(
                name: "UQ__Estado_D__72AFBCC6E3D8D2B1",
                table: "Estado_Documento",
                column: "nombre",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_doc_estadodoc",
                table: "Documento",
                column: "id_estado_documento",
                principalTable: "Estado_Documento",
                principalColumn: "id_estado_documento");

            migrationBuilder.AddForeignKey(
                name: "FK_Muestra_Usuario_UsuarioId",
                table: "Muestra",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_doc_estadodoc",
                table: "Documento");

            migrationBuilder.DropForeignKey(
                name: "FK_Muestra_Usuario_UsuarioId",
                table: "Muestra");

            migrationBuilder.DropTable(
                name: "Estado_Documento");

            migrationBuilder.DropIndex(
                name: "IX_Muestra_UsuarioId",
                table: "Muestra");

            migrationBuilder.DropIndex(
                name: "IX_Documento_id_estado_documento",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Muestra");

            migrationBuilder.DropColumn(
                name: "id_estado_documento",
                table: "Documento");

            migrationBuilder.AddColumn<int>(
                name: "id_parametro_norma",
                table: "Prueba",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "id_prueba",
                table: "Parametro_Norma",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "id_Analista",
                table: "Muestra",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parametro_Norma_id_prueba",
                table: "Parametro_Norma",
                column: "id_prueba",
                unique: true,
                filter: "[id_prueba] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "uk_parametro",
                table: "Parametro_Norma",
                columns: new[] { "id_prueba", "nombre_parametro" },
                unique: true,
                filter: "[id_prueba] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Muestra_id_Analista",
                table: "Muestra",
                column: "id_Analista");

            migrationBuilder.AddForeignKey(
                name: "fk_muestra_analista",
                table: "Muestra",
                column: "id_Analista",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "fk_prueba_parametro",
                table: "Parametro_Norma",
                column: "id_prueba",
                principalTable: "Prueba",
                principalColumn: "id_prueba");
        }
    }
}
