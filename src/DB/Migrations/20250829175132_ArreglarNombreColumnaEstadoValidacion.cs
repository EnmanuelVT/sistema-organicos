using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class ArreglarNombreColumnaEstadoValidacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstadoValidacion",
                table: "Resultado_Prueba",
                newName: "estado_validacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "estado_validacion",
                table: "Resultado_Prueba",
                newName: "EstadoValidacion");
        }
    }
}
