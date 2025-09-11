CREATE   PROCEDURE sp_agregar_parametro_a_tipo_muestra
    @p_nombre_parametro VARCHAR(120),
    @p_valor_min DECIMAL(18,6),
    @p_valor_max DECIMAL(18,6),
    @p_unidad VARCHAR(30),
    @p_tpmst_id TINYINT,
    @p_id_usuario VARCHAR(450)
AS
BEGIN
INSERT INTO Parametro_Norma (tpmst_id, nombre_parametro, valor_min, valor_max, unidad)
VALUES (@p_tpmst_id, @p_nombre_parametro, @p_valor_min, @p_valor_max, @p_unidad);

INSERT INTO Auditoria (id_usuario, accion, descripcion) VALUES (@p_id_usuario, 'AGREGAR_PARAMETRO_A_TIPO_MUESTRA', CONCAT('PRB=',
                                                                                                                          CAST(@p_tpmst_id AS VARCHAR(10)),', PARAM=',@p_nombre_parametro));
END
go

-- 6.2 Asignar analista a una muestra (bitácora + set analista actual)
CREATE   PROCEDURE sp_asignar_analista
    @p_MST_CODIGO VARCHAR(30),
    @p_id_analista VARCHAR(450),
    @p_observaciones VARCHAR(255)
AS
BEGIN
INSERT INTO Bitacora_Muestra (id_muestra, id_analista, observaciones)
VALUES (@p_MST_CODIGO, @p_id_analista, @p_observaciones);

INSERT INTO Auditoria (id_usuario, accion, descripcion)
VALUES (@p_id_analista, 'ASIGNAR_ANALISTA', CONCAT('MST=',@p_MST_CODIGO));
END
go

-- 6.3 Cambiar estado de la muestra (y trazar historial)
CREATE   PROCEDURE sp_cambiar_estado
    @p_MST_CODIGO VARCHAR(30),
    @p_nuevo_estado TINYINT,
    @p_id_usuario VARCHAR(450),
    @p_observaciones VARCHAR(255)
AS
BEGIN
UPDATE Muestra SET estado_actual = @p_nuevo_estado WHERE MST_CODIGO = @p_MST_CODIGO;

INSERT INTO Historial_Trazabilidad (id_muestra, id_usuario, estado, observaciones)
VALUES (@p_MST_CODIGO, @p_id_usuario, @p_nuevo_estado, @p_observaciones);

INSERT INTO Auditoria (id_usuario, accion, descripcion)
VALUES (@p_id_usuario, 'CAMBIAR_ESTADO', CONCAT('MST=',@p_MST_CODIGO,', ESTADO=',CAST(@p_nuevo_estado AS VARCHAR(10))));
END
go

CREATE   PROCEDURE sp_crear_muestra
    @p_MST_CODIGO VARCHAR(30),
    @p_TPMST_ID TINYINT,
    @p_Nombre VARCHAR(120),
    @p_Fecha_recepcion DATETIME,
    @p_origen VARCHAR(200),
    @p_Fecha_Salida_Estimada DATETIME,
    @p_Cond_alm VARCHAR(200),
    @p_Cond_trans VARCHAR(200),
    @p_id_solicitante VARCHAR(450) -- Changed from VARCHAR(15) to VARCHAR(450)
AS
BEGIN
    DECLARE @v_estado_recibida TINYINT = 1;

INSERT INTO Muestra
(MST_CODIGO, TPMST_ID, Nombre, Fecha_recepcion, origen, Fecha_Salida_Estimada,
 Condiciones_almacenamiento, Condiciones_transporte, id_usuario_solicitante,
 estado_actual)
VALUES
    (@p_MST_CODIGO, @p_TPMST_ID, @p_Nombre, @p_Fecha_recepcion, @p_origen, @p_Fecha_Salida_Estimada,
     @p_Cond_alm, @p_Cond_trans, @p_id_solicitante, @v_estado_recibida);

INSERT INTO Auditoria (id_usuario, accion, descripcion)
VALUES (@p_id_solicitante, 'CREAR_MUESTRA', CONCAT('MST=',@p_MST_CODIGO));
END
go


CREATE   PROCEDURE sp_crear_prueba
    @p_nombre_prueba VARCHAR(120),
    @p_tipo_muestra_asociada TINYINT,
    @p_id_muestra VARCHAR(30),
    @p_id_usuario VARCHAR(450)
AS
BEGIN
    DECLARE @new_id_prueba INT;

INSERT INTO Prueba (nombre_prueba, tipo_muestra_asociada, id_muestra)
VALUES (@p_nombre_prueba, @p_tipo_muestra_asociada, @p_id_muestra);
SET @new_id_prueba = SCOPE_IDENTITY();

INSERT INTO Auditoria (id_usuario, accion, descripcion) VALUES (@p_id_usuario, 'CREAR_PRUEBA', CONCAT('PRB=',
                                                                                                      CAST(@new_id_prueba AS VARCHAR(10))));
END
go


-- 6.5 Generar registro de documento
CREATE   PROCEDURE sp_generar_documento
    @p_MST_CODIGO VARCHAR(30),
    @p_id_tipo_doc TINYINT,
    @p_id_estado_doc INT,
    @p_version INT,
    @p_ruta VARCHAR(255),
    @p_doc_pdf VARBINARY(MAX),  -- opcional: almacenar PDF directamente
    @p_id_usuario VARCHAR(30)
AS
BEGIN
INSERT INTO Documento (id_muestra, id_tipo_doc, version, ruta_archivo, DOC_PDF, id_estado_documento)
VALUES (@p_MST_CODIGO, @p_id_tipo_doc, @p_version, @p_ruta, @p_doc_pdf, @p_id_estado_doc);

INSERT INTO Auditoria (id_usuario, accion, descripcion)
VALUES (@p_id_usuario, 'GENERAR_DOCUMENTO', CONCAT('MST=',@p_MST_CODIGO,', DOC=',CAST(@p_id_tipo_doc AS VARCHAR(10)),', v',CAST(@p_version AS VARCHAR(10))));
END
go


-- 6.6 Registrar notificación
CREATE   PROCEDURE sp_registrar_notificacion
    @p_MST_CODIGO VARCHAR(30),
    @p_tipo_alerta VARCHAR(60),
    @p_destinatario VARCHAR(120),
    @p_detalle VARCHAR(255),
    @p_enviado BIT,
    @p_fecha_envio DATETIME
AS
BEGIN
INSERT INTO Notificacion (id_muestra, tipo_alerta, destinatario, detalle, enviado, fecha_envio)
VALUES (@p_MST_CODIGO, @p_tipo_alerta, @p_destinatario, @p_detalle, @p_enviado, @p_fecha_envio);
END
go

CREATE   PROCEDURE sp_registrar_resultado
    @p_MST_CODIGO VARCHAR(30),
    @p_id_prueba INT,
    @p_id_parametro INT,
    @p_valor DECIMAL(18,6),
    @p_unidad VARCHAR(30),
    @p_id_usuario VARCHAR(450)
AS
BEGIN
    DECLARE @v_cumple BIT;
    DECLARE @v_min DECIMAL(18,6);
    DECLARE @v_max DECIMAL(18,6);
    DECLARE @v_param VARCHAR(120);
    
    -- Tomamos 1er parámetro de norma de esa prueba
SELECT TOP 1 @v_param = nombre_parametro, @v_min = valor_min, @v_max = valor_max
FROM Parametro_Norma
WHERE id_parametro = @p_id_parametro
ORDER BY id_parametro;

-- Regla: si no hay norma registrada, se deja NULL; si hay, se evalúa en rango [min,max]
SET @v_cumple = NULL;
    IF @v_min IS NOT NULL OR @v_max IS NOT NULL BEGIN
        SET @v_cumple = 1;
        IF @v_min IS NOT NULL AND @p_valor < @v_min SET @v_cumple = 0;
        IF @v_max IS NOT NULL AND @p_valor > @v_max SET @v_cumple = 0;
END

INSERT INTO Resultado_Prueba
(id_prueba, id_parametro, id_muestra, valor_obtenido, unidad, cumple_norma)
VALUES
    (@p_id_prueba, @p_id_parametro, @p_MST_CODIGO, @p_valor, @p_unidad, @v_cumple);

-- Notificación si no cumple
IF @v_cumple = 0 BEGIN
        INSERT INTO Notificacion(id_muestra, tipo_alerta, destinatario, detalle, enviado, fecha_envio)
        VALUES (@p_MST_CODIGO, 'Fuera de norma', (SELECT Email FROM Usuario WHERE Id = (SELECT id_usuario_solicitante FROM Muestra WHERE MST_CODIGO = @p_MST_CODIGO)),
                CONCAT('El parámetro ',@v_param,' de la prueba ',CAST(@p_id_prueba AS VARCHAR(10)),' no cumple con la norma. Valor obtenido: ',CAST(@p_valor AS VARCHAR(20)),'. Rango esperado: [', 
                       CASE WHEN @v_min IS NOT NULL THEN CAST(@v_min AS VARCHAR(20)) ELSE '-inf' END, ' - ', 
                       CASE WHEN @v_max IS NOT NULL THEN CAST(@v_max AS VARCHAR(20)) ELSE '+inf' END, '].'),
                0, NULL
        );
END

INSERT INTO Auditoria (id_usuario, accion, descripcion)
VALUES (@p_id_usuario, 'REGISTRAR_RESULTADO', CONCAT('MST=',@p_MST_CODIGO,', PRB=',CAST(@p_id_prueba AS VARCHAR(10))));
END
go
