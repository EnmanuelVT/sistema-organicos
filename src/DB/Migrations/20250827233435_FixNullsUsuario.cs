using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class FixNullsUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "Usuario",
                type: "varchar(80)",
                unicode: false,
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(80)",
                oldUnicode: false,
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "apellido",
                table: "Usuario",
                type: "varchar(80)",
                unicode: false,
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(80)",
                oldUnicode: false,
                oldMaxLength: 80);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "Usuario",
                type: "varchar(80)",
                unicode: false,
                maxLength: 80,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(80)",
                oldUnicode: false,
                oldMaxLength: 80,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "apellido",
                table: "Usuario",
                type: "varchar(80)",
                unicode: false,
                maxLength: 80,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(80)",
                oldUnicode: false,
                oldMaxLength: 80,
                oldNullable: true);
        }
    }
}
