using DB.Datos;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    [DbContext(typeof(MasterDbContext))]
    [Migration("20260114091500_UpdateSpCrearPruebaRequireTipoPrueba")]
    public partial class UpdateSpCrearPruebaRequireTipoPrueba : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
SET NOCOUNT ON;

EXEC(N'
CREATE OR ALTER PROCEDURE dbo.sp_crear_prueba
    @p_nombre_prueba VARCHAR(120),
    @p_id_muestra VARCHAR(30),
    @p_tipo_prueba_id INT,
    @p_id_usuario VARCHAR(450)
AS
BEGIN
    SET NOCOUNT ON;

    IF (@p_tipo_prueba_id IS NULL OR @p_tipo_prueba_id <= 0)
    BEGIN
        RAISERROR(''Debe indicar @p_tipo_prueba_id.'', 16, 1);
        RETURN;
    END

    DECLARE @new_id_prueba INT;

    INSERT INTO dbo.Prueba (nombre_prueba, id_muestra, tipo_prueba_id)
    VALUES (@p_nombre_prueba, @p_id_muestra, @p_tipo_prueba_id);

    SET @new_id_prueba = SCOPE_IDENTITY();

    INSERT INTO dbo.Auditoria (id_usuario, accion, descripcion)
    VALUES (@p_id_usuario, ''CREAR_PRUEBA'', CONCAT(''PRB='', CAST(@new_id_prueba AS VARCHAR(10))));
END
');
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intencionalmente vacío: no se revierten procedimientos automáticamente.
        }
    }
}
