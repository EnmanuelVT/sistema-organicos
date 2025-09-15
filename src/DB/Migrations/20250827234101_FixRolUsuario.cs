using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class FixRolUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_usuario_rol",
                table: "Usuario");

            migrationBuilder.DropTable(
                name: "Rol_Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_id_rol",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "id_rol",
                table: "Usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "id_rol",
                table: "Usuario",
                type: "tinyint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Rol_Usuario",
                columns: table => new
                {
                    id_rol = table.Column<byte>(type: "tinyint", nullable: false),
                    nombre_rol = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rol_Usua__6ABCB5E0C2370F5E", x => x.id_rol);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_id_rol",
                table: "Usuario",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "UQ__Rol_Usua__673CB435C9C52528",
                table: "Rol_Usuario",
                column: "nombre_rol",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_usuario_rol",
                table: "Usuario",
                column: "id_rol",
                principalTable: "Rol_Usuario",
                principalColumn: "id_rol");
        }
    }
}
