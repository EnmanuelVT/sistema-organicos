using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class SeedTipoPrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
SET NOCOUNT ON;

-- Trigger actualizado: insertar también tipo_prueba_id
EXEC(N'
CREATE OR ALTER TRIGGER dbo.trg_muestra_generar_pruebas
ON dbo.Muestra
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @outerTranCount INT = @@TRANCOUNT;

    IF @outerTranCount > 0
        SAVE TRAN trg_muestra_generar_pruebas;
    ELSE
        BEGIN TRAN;

    BEGIN TRY
        -- Agua (1)
        INSERT INTO dbo.Prueba (nombre_prueba, id_muestra, tipo_prueba_id)
        SELECT v.nombre_prueba, i.MST_CODIGO, tp.id_tipo_prueba
        FROM inserted i
        CROSS APPLY (VALUES
            (N''Parámetros fisicoquímicos''),
            (N''Microbiológicos'')
        ) v(nombre_prueba)
        LEFT JOIN dbo.Tipo_Prueba tp ON tp.nombre = v.nombre_prueba
        WHERE i.TPMST_ID = 1
          AND NOT EXISTS (
              SELECT 1
              FROM dbo.Prueba p
              WHERE p.id_muestra = i.MST_CODIGO
                AND p.nombre_prueba = v.nombre_prueba
          );

                -- Alimento (2)
                INSERT INTO dbo.Prueba (nombre_prueba, id_muestra, tipo_prueba_id)
                SELECT v.nombre_prueba, i.MST_CODIGO, tp.id_tipo_prueba
                FROM inserted i
                CROSS APPLY (VALUES
                        (N''Análisis microbiológico'')
                ) v(nombre_prueba)
                LEFT JOIN dbo.Tipo_Prueba tp ON tp.nombre = v.nombre_prueba
                WHERE i.TPMST_ID = 2
                    AND NOT EXISTS (
                            SELECT 1
                            FROM dbo.Prueba p
                            WHERE p.id_muestra = i.MST_CODIGO
                                AND p.nombre_prueba = v.nombre_prueba
                    );

                -- Bebida alcohólica (3)
                INSERT INTO dbo.Prueba (nombre_prueba, id_muestra, tipo_prueba_id)
                SELECT v.nombre_prueba, i.MST_CODIGO, tp.id_tipo_prueba
                FROM inserted i
                CROSS APPLY (VALUES
                        (N''Parámetros fisicoquímicos'')
                ) v(nombre_prueba)
                LEFT JOIN dbo.Tipo_Prueba tp ON tp.nombre = v.nombre_prueba
                WHERE i.TPMST_ID = 3
                    AND NOT EXISTS (
                            SELECT 1
                            FROM dbo.Prueba p
                            WHERE p.id_muestra = i.MST_CODIGO
                                AND p.nombre_prueba = v.nombre_prueba
                    );

        IF @outerTranCount = 0
            COMMIT;
    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg NVARCHAR(2048) = ERROR_MESSAGE();
        DECLARE @ErrSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrState INT = ERROR_STATE();

        IF XACT_STATE() <> 0
        BEGIN
            IF @outerTranCount = 0
                ROLLBACK;
            ELSE
                ROLLBACK TRAN trg_muestra_generar_pruebas;
        END

        IF @ErrSeverity IS NULL OR @ErrSeverity < 11 SET @ErrSeverity = 16;
        IF @ErrSeverity > 18 SET @ErrSeverity = 16;
        IF @ErrState IS NULL OR @ErrState < 1 SET @ErrState = 1;

        RAISERROR(@ErrMsg, @ErrSeverity, @ErrState);
    END CATCH
END
');

-- 4) SP endurecida: validar relación prueba/parametro por tipo_prueba_id (cuando aplique)
EXEC(N'
CREATE OR ALTER PROCEDURE dbo.sp_registrar_resultado
    @p_MST_CODIGO VARCHAR(30),
    @p_id_prueba INT,
    @p_id_parametro INT,
    @p_valor DECIMAL(18,6),
    @p_unidad VARCHAR(30),
    @p_id_usuario VARCHAR(450)
AS
BEGIN
    SET NOCOUNT ON;

    -- Validación: la prueba debe pertenecer a la muestra
    IF NOT EXISTS (SELECT 1 FROM dbo.Prueba WHERE id_prueba = @p_id_prueba AND id_muestra = @p_MST_CODIGO)
    BEGIN
        RAISERROR(N''La prueba no pertenece a la muestra.'', 16, 1);
        RETURN;
    END

    DECLARE @v_prueba_tipo_prueba_id INT;
    DECLARE @v_param_tipo_prueba_id INT;

    SELECT @v_prueba_tipo_prueba_id = tipo_prueba_id
    FROM dbo.Prueba
    WHERE id_prueba = @p_id_prueba;

    SELECT @v_param_tipo_prueba_id = tipo_prueba_id
    FROM dbo.Parametro_Norma
    WHERE id_parametro = @p_id_parametro;

    -- Si ambos lados especifican tipo de prueba, deben coincidir
    IF @v_prueba_tipo_prueba_id IS NOT NULL
       AND @v_param_tipo_prueba_id IS NOT NULL
       AND @v_prueba_tipo_prueba_id <> @v_param_tipo_prueba_id
    BEGIN
        RAISERROR(N''El parámetro no pertenece al tipo de prueba seleccionado.'', 16, 1);
        RETURN;
    END

    DECLARE @v_cumple BIT;
    DECLARE @v_min DECIMAL(18,6);
    DECLARE @v_max DECIMAL(18,6);
    DECLARE @v_param VARCHAR(120);

    SELECT TOP 1
        @v_param = nombre_parametro,
        @v_min = valor_min,
        @v_max = valor_max
    FROM dbo.Parametro_Norma
    WHERE id_parametro = @p_id_parametro
    ORDER BY id_parametro;

    SET @v_cumple = NULL;
    IF @v_min IS NOT NULL OR @v_max IS NOT NULL
    BEGIN
        SET @v_cumple = 1;
        IF @v_min IS NOT NULL AND @p_valor < @v_min SET @v_cumple = 0;
        IF @v_max IS NOT NULL AND @p_valor > @v_max SET @v_cumple = 0;
    END

    INSERT INTO dbo.Resultado_Prueba
        (id_prueba, id_parametro, id_muestra, valor_obtenido, unidad, cumple_norma)
    VALUES
        (@p_id_prueba, @p_id_parametro, @p_MST_CODIGO, @p_valor, @p_unidad, @v_cumple);

    IF @v_cumple = 0
    BEGIN
        INSERT INTO dbo.Notificacion(id_muestra, tipo_alerta, destinatario, detalle, enviado, fecha_envio)
        VALUES (
            @p_MST_CODIGO,
            ''Fuera de norma'',
            (SELECT Email FROM dbo.Usuario WHERE Id = (SELECT id_usuario_solicitante FROM dbo.Muestra WHERE MST_CODIGO = @p_MST_CODIGO)),
            CONCAT(''El parámetro '',@v_param,'' de la prueba '',CAST(@p_id_prueba AS VARCHAR(10)),'' no cumple con la norma. Valor obtenido: '',CAST(@p_valor AS VARCHAR(20)),''. Rango esperado: ['',
                CASE WHEN @v_min IS NOT NULL THEN CAST(@v_min AS VARCHAR(20)) ELSE ''-inf'' END, '' - '',
                CASE WHEN @v_max IS NOT NULL THEN CAST(@v_max AS VARCHAR(20)) ELSE ''+inf'' END, ''].''),
            0,
            NULL
        );
    END

    INSERT INTO dbo.Auditoria (id_usuario, accion, descripcion)
    VALUES (@p_id_usuario, ''REGISTRAR_RESULTADO'', CONCAT(''MST='',@p_MST_CODIGO,'', PRB='',CAST(@p_id_prueba AS VARCHAR(10))));
END
');

");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intencionalmente vacío: no se borran datos sembrados ni se revierte SP/trigger.
        }
    }
}
