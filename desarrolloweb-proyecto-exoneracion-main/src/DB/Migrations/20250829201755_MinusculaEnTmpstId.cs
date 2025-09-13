using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class MinusculaEnTmpstId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TPMST_ID",
                table: "Tipo_Muestra",
                newName: "tpmst_id");

            migrationBuilder.RenameColumn(
                name: "TMPST_ID",
                table: "Parametro_Norma",
                newName: "tmpst_id");

            migrationBuilder.RenameIndex(
                name: "IX_Parametro_Norma_TMPST_ID",
                table: "Parametro_Norma",
                newName: "IX_Parametro_Norma_tmpst_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tpmst_id",
                table: "Tipo_Muestra",
                newName: "TPMST_ID");

            migrationBuilder.RenameColumn(
                name: "tmpst_id",
                table: "Parametro_Norma",
                newName: "TMPST_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Parametro_Norma_tmpst_id",
                table: "Parametro_Norma",
                newName: "IX_Parametro_Norma_TMPST_ID");
        }
    }
}
