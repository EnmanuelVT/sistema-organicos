using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSpsForTipoPrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
SET NOCOUNT ON;

-- sp_agregar_parametro_a_tipo_muestra: ahora acepta @p_tipo_prueba_id (nullable)
EXEC(N'
CREATE OR ALTER PROCEDURE dbo.sp_agregar_parametro_a_tipo_muestra
    @p_nombre_parametro VARCHAR(120),
    @p_valor_min DECIMAL(18,6),
    @p_valor_max DECIMAL(18,6),
    @p_unidad VARCHAR(30),
    @p_tpmst_id TINYINT,
    @p_tipo_prueba_id INT = NULL,
    @p_id_usuario VARCHAR(450)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Parametro_Norma (tpmst_id, tipo_prueba_id, nombre_parametro, valor_min, valor_max, unidad)
    VALUES (@p_tpmst_id, @p_tipo_prueba_id, @p_nombre_parametro, @p_valor_min, @p_valor_max, @p_unidad);

    INSERT INTO dbo.Auditoria (id_usuario, accion, descripcion)
    VALUES (
        @p_id_usuario,
        ''AGREGAR_PARAMETRO_A_TIPO_MUESTRA'',
        CONCAT(
            ''TPMST='', CAST(@p_tpmst_id AS VARCHAR(10)),
            '', TP='', COALESCE(CAST(@p_tipo_prueba_id AS VARCHAR(10)), ''NULL''),
            '', PARAM='', @p_nombre_parametro
        )
    );
END
');

-- sp_crear_prueba: intenta inferir tipo_prueba_id por nombre
EXEC(N'
CREATE OR ALTER PROCEDURE dbo.sp_crear_prueba
    @p_nombre_prueba VARCHAR(120),
    @p_id_muestra VARCHAR(30),
    @p_id_usuario VARCHAR(450)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @new_id_prueba INT;
    DECLARE @v_tipo_prueba_id INT;

    SELECT TOP 1 @v_tipo_prueba_id = id_tipo_prueba
    FROM dbo.Tipo_Prueba
    WHERE nombre = @p_nombre_prueba;

    INSERT INTO dbo.Prueba (nombre_prueba, id_muestra, tipo_prueba_id)
    VALUES (@p_nombre_prueba, @p_id_muestra, @v_tipo_prueba_id);

    SET @new_id_prueba = SCOPE_IDENTITY();

    INSERT INTO dbo.Auditoria (id_usuario, accion, descripcion)
    VALUES (@p_id_usuario, ''CREAR_PRUEBA'', CONCAT(''PRB='', CAST(@new_id_prueba AS VARCHAR(10))));
END
');

");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intencionalmente vacío: no se revierten procedimientos automáticamente.
        }
    }
}
