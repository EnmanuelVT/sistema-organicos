using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class QuitarNormaReferenciaDePrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "norma_referencia",
                table: "Prueba");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "norma_referencia",
                table: "Prueba",
                type: "varchar(120)",
                unicode: false,
                maxLength: 120,
                nullable: true);
        }
    }
}
