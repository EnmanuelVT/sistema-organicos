using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoPrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "tipo_prueba_id",
                table: "Prueba",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tipo_prueba_id",
                table: "Parametro_Norma",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tipo_Prueba",
                columns: table => new
                {
                    id_tipo_prueba = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Prueba", x => x.id_tipo_prueba);
                });

            migrationBuilder.CreateTable(
                name: "Tipo_Muestra_Tipo_Prueba",
                columns: table => new
                {
                    tpmst_id = table.Column<byte>(type: "tinyint", nullable: false),
                    tipo_prueba_id = table.Column<int>(type: "int", nullable: false),
                    orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Muestra_Tipo_Prueba", x => new { x.tpmst_id, x.tipo_prueba_id });
                    table.ForeignKey(
                        name: "fk_tmt_tp_tipo_muestra",
                        column: x => x.tpmst_id,
                        principalTable: "Tipo_Muestra",
                        principalColumn: "tpmst_id");
                    table.ForeignKey(
                        name: "fk_tmt_tp_tipo_prueba",
                        column: x => x.tipo_prueba_id,
                        principalTable: "Tipo_Prueba",
                        principalColumn: "id_tipo_prueba");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prueba_tipo_prueba_id",
                table: "Prueba",
                column: "tipo_prueba_id");

            migrationBuilder.CreateIndex(
                name: "IX_Parametro_Norma_tipo_prueba_id",
                table: "Parametro_Norma",
                column: "tipo_prueba_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tipo_Muestra_Tipo_Prueba_tipo_prueba_id",
                table: "Tipo_Muestra_Tipo_Prueba",
                column: "tipo_prueba_id");

            migrationBuilder.CreateIndex(
                name: "UQ_Tipo_Prueba_codigo",
                table: "Tipo_Prueba",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Tipo_Prueba_nombre",
                table: "Tipo_Prueba",
                column: "nombre",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_parametro_tipo_prueba",
                table: "Parametro_Norma",
                column: "tipo_prueba_id",
                principalTable: "Tipo_Prueba",
                principalColumn: "id_tipo_prueba");

            migrationBuilder.AddForeignKey(
                name: "fk_prueba_tipo_prueba",
                table: "Prueba",
                column: "tipo_prueba_id",
                principalTable: "Tipo_Prueba",
                principalColumn: "id_tipo_prueba");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_parametro_tipo_prueba",
                table: "Parametro_Norma");

            migrationBuilder.DropForeignKey(
                name: "fk_prueba_tipo_prueba",
                table: "Prueba");

            migrationBuilder.DropTable(
                name: "Tipo_Muestra_Tipo_Prueba");

            migrationBuilder.DropTable(
                name: "Tipo_Prueba");

            migrationBuilder.DropIndex(
                name: "IX_Prueba_tipo_prueba_id",
                table: "Prueba");

            migrationBuilder.DropIndex(
                name: "IX_Parametro_Norma_tipo_prueba_id",
                table: "Parametro_Norma");

            migrationBuilder.DropColumn(
                name: "tipo_prueba_id",
                table: "Prueba");

            migrationBuilder.DropColumn(
                name: "tipo_prueba_id",
                table: "Parametro_Norma");
        }
    }
}
