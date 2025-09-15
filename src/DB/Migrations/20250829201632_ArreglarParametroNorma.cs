using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class ArreglarParametroNorma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Parametro_Norma_id_prueba",
                table: "Parametro_Norma");

            migrationBuilder.DropIndex(
                name: "uk_parametro",
                table: "Parametro_Norma");

            migrationBuilder.AlterColumn<byte>(
                name: "tpmst_id",
                table: "Parametro_Norma",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<int>(
                name: "id_prueba",
                table: "Parametro_Norma",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<byte>(
                name: "TMPST_ID",
                table: "Parametro_Norma",
                type: "tinyint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parametro_Norma_id_prueba",
                table: "Parametro_Norma",
                column: "id_prueba",
                unique: true,
                filter: "[id_prueba] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Parametro_Norma_TMPST_ID",
                table: "Parametro_Norma",
                column: "TMPST_ID");

            migrationBuilder.CreateIndex(
                name: "uk_parametro",
                table: "Parametro_Norma",
                columns: new[] { "id_prueba", "nombre_parametro" },
                unique: true,
                filter: "[id_prueba] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "fk_parametro_tipo",
                table: "Parametro_Norma",
                column: "TMPST_ID",
                principalTable: "Tipo_Muestra",
                principalColumn: "TPMST_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_parametro_tipo",
                table: "Parametro_Norma");

            migrationBuilder.DropIndex(
                name: "IX_Parametro_Norma_id_prueba",
                table: "Parametro_Norma");

            migrationBuilder.DropIndex(
                name: "IX_Parametro_Norma_TMPST_ID",
                table: "Parametro_Norma");

            migrationBuilder.DropIndex(
                name: "uk_parametro",
                table: "Parametro_Norma");

            migrationBuilder.DropColumn(
                name: "TMPST_ID",
                table: "Parametro_Norma");

            migrationBuilder.AlterColumn<byte>(
                name: "tpmst_id",
                table: "Parametro_Norma",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id_prueba",
                table: "Parametro_Norma",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parametro_Norma_id_prueba",
                table: "Parametro_Norma",
                column: "id_prueba",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_parametro",
                table: "Parametro_Norma",
                columns: new[] { "id_prueba", "nombre_parametro" },
                unique: true);
        }
    }
}
