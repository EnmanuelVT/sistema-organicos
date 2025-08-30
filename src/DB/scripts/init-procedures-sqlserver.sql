-- =========================================================
--  DB: Laboratorio_Muestras  (SQL Server)
--  Modela Reto App Web (ago-oct 2025) + tu diseño de BD
--  Autor: generado por ChatGPT
-- =========================================================

/*DROP DATABASE IF EXISTS Laboratorio_Muestras;
CREATE DATABASE Laboratorio_Muestras;
USE Laboratorio_Muestras;
*/
    
-- =========================
-- 1) CATALOGOS / LOOKUPS
-- =========================

--CREATE TABLE Rol_Usuario (
--    id_rol        TINYINT PRIMARY KEY,
--    nombre_rol    VARCHAR(30) NOT NULL UNIQUE
--);

--CREATE TABLE Tipo_Muestra (
--    TPMST_ID      TINYINT PRIMARY KEY,
--    nombre        VARCHAR(40) NOT NULL UNIQUE  -- Agua, Alimento, Bebida alcohólica
--);

--CREATE TABLE Estado_Muestra (
--    id_estado     TINYINT PRIMARY KEY,
--    nombre        VARCHAR(30) NOT NULL UNIQUE  -- Recibida, En análisis, Evaluada, Certificada
--);

--CREATE TABLE Tipo_Documento (
--    id_tipo_doc   TINYINT PRIMARY KEY,
--    nombre        VARCHAR(30) NOT NULL UNIQUE  -- Certificado, Informe, etc.
--);

---- =========================
---- 2) NUCLEO - Create tables without foreign keys first
---- =========================

--CREATE TABLE Usuario (
--    US_Cedula         VARCHAR(15) PRIMARY KEY,        -- cédula o NIT/RNC; string para conservar ceros
--    nombre            VARCHAR(80) NOT NULL,
--    apellido          VARCHAR(80) NOT NULL,
--    razon_social      VARCHAR(120) NULL,
--    Direccion         VARCHAR(200) NULL,
--    Correo            VARCHAR(120) NOT NULL,
--    Telefono          VARCHAR(30)  NULL,
--    CONTACTO          VARCHAR(120) NULL,              -- persona contacto si es empresa
--    Username          VARCHAR(60)  NOT NULL UNIQUE,
--    contrasena        VARCHAR(255) NOT NULL,          -- hash
--    id_rol            TINYINT NOT NULL
--);

--CREATE TABLE Muestra (
--    MST_CODIGO            VARCHAR(30) PRIMARY KEY,    -- código único de la muestra
--    TPMST_ID              TINYINT NOT NULL,          -- FK a Tipo_Muestra
--    Nombre                VARCHAR(120) NULL,          -- nombre/alias de la muestra si aplica
--    Fecha_recepcion       DATETIME NOT NULL,          -- (pedido explícito)
--    origen                VARCHAR(200) NOT NULL,      -- fabricante, distribuidor, punto de toma
--    Fecha_Salida_Estimada DATETIME NULL,
--    Condiciones_almacenamiento VARCHAR(200) NULL,
--    Condiciones_transporte    VARCHAR(200) NULL,
--    id_usuario_solicitante    VARCHAR(15) NOT NULL,   -- US_Cedula solicitante (rol = solicitante)
--    id_Analista               VARCHAR(15) NULL,       -- US_Cedula analista asignado principal
--    estado_actual             TINYINT NOT NULL
--);

--CREATE TABLE Bitacora_Muestra (
--    id_bitacora    BIGINT IDENTITY(1,1) PRIMARY KEY,
--    id_muestra     VARCHAR(30) NOT NULL,
--    id_analista    VARCHAR(15) NOT NULL,
--    fecha_asignacion DATETIME NOT NULL DEFAULT GETDATE(),
--    observaciones  VARCHAR(255) NULL
--);

--CREATE TABLE Prueba (
--    id_prueba          INT IDENTITY(1,1) PRIMARY KEY,
--    nombre_prueba      VARCHAR(120) NOT NULL,
--    tipo_muestra_asociada TINYINT NOT NULL,          -- restringe por tipo
--    norma_referencia   VARCHAR(120) NULL             -- Codex, NORDOM, etc.
--);

--CREATE TABLE Parametro_Norma (
--    id_parametro       INT IDENTITY(1,1) PRIMARY KEY,
--    id_prueba          INT NOT NULL,                  -- acoplar norma a prueba
--    nombre_parametro   VARCHAR(120) NOT NULL,         -- pH, turbidez, etc.
--    valor_min          DECIMAL(18,6) NULL,
--    valor_max          DECIMAL(18,6) NULL,            -- añadido para comparar rangos completos
--    unidad             VARCHAR(30)  NULL
--);

--CREATE TABLE Resultado_Prueba (
--    id_resultado     BIGINT IDENTITY(1,1) PRIMARY KEY,
--    id_prueba        INT NOT NULL,
--    id_muestra       VARCHAR(30) NOT NULL,
--    valor_obtenido   DECIMAL(18,6) NULL,
--    unidad           VARCHAR(30)  NULL,
--    cumple_norma     BIT          NULL,               -- 1 Sí / 0 No / NULL si no aplica
--    fecha_registro   DATETIME NOT NULL DEFAULT GETDATE(),
--    validado_por     VARCHAR(15)  NULL                -- US_Cedula evaluador/validador
--);

--CREATE TABLE Documento (
--    id_documento     BIGINT IDENTITY(1,1) PRIMARY KEY,
--    id_muestra       VARCHAR(30) NOT NULL,
--    id_tipo_doc      TINYINT NOT NULL,
--    version          INT NOT NULL DEFAULT 1,
--    ruta_archivo     VARCHAR(255) NULL,               -- soporte a "almacenamiento digital"
--    DOC_PDF          VARBINARY(MAX) NULL,               -- opcional: almacenar PDF directamente
--    fecha_creacion   DATETIME NOT NULL DEFAULT GETDATE()
--);

--CREATE TABLE Historial_Trazabilidad (
--    id_historial     BIGINT IDENTITY(1,1) PRIMARY KEY,
--    id_muestra       VARCHAR(30) NOT NULL,
--    id_usuario       VARCHAR(15) NOT NULL,            -- responsable del cambio
--    estado           TINYINT NOT NULL,
--    fecha_cambio     DATETIME NOT NULL DEFAULT GETDATE(),
--    observaciones    VARCHAR(255) NULL
--);

--CREATE TABLE Notificacion (
--    id_notificacion  BIGINT IDENTITY(1,1) PRIMARY KEY,
--    id_muestra       VARCHAR(30) NOT NULL,
--    tipo_alerta      VARCHAR(60) NOT NULL,            -- Plazo, Fuera de norma, etc.
--    destinatario     VARCHAR(120) NOT NULL,
--    enviado          BIT NOT NULL DEFAULT 0,
--    fecha_envio      DATETIME NULL,
--    detalle          VARCHAR(255) NULL
--);

--CREATE TABLE Auditoria (
--    id_auditoria     BIGINT IDENTITY(1,1) PRIMARY KEY,
--    id_usuario       VARCHAR(15) NOT NULL,
--    accion           VARCHAR(120) NOT NULL,
--    fecha_accion     DATETIME NOT NULL DEFAULT GETDATE(),
--    descripcion      VARCHAR(255) NULL
--);
--GO

---- =========================
---- 3) ADD FOREIGN KEY CONSTRAINTS
---- =========================

---- Usuario constraints
--ALTER TABLE Usuario ADD CONSTRAINT fk_usuario_rol FOREIGN KEY (id_rol) REFERENCES Rol_Usuario(id_rol);

---- Muestra constraints
--ALTER TABLE Muestra ADD CONSTRAINT fk_muestra_tipo FOREIGN KEY (TPMST_ID) REFERENCES Tipo_Muestra(TPMST_ID);
--ALTER TABLE Muestra ADD CONSTRAINT fk_muestra_solic FOREIGN KEY (id_usuario_solicitante) REFERENCES Usuario(US_Cedula);
--ALTER TABLE Muestra ADD CONSTRAINT fk_muestra_analista FOREIGN KEY (id_Analista) REFERENCES Usuario(US_Cedula);
--ALTER TABLE Muestra ADD CONSTRAINT fk_muestra_estado FOREIGN KEY (estado_actual) REFERENCES Estado_Muestra(id_estado);

---- Bitacora_Muestra constraints
--ALTER TABLE Bitacora_Muestra ADD CONSTRAINT fk_bit_muestra FOREIGN KEY (id_muestra) REFERENCES Muestra(MST_CODIGO);
--ALTER TABLE Bitacora_Muestra ADD CONSTRAINT fk_bit_analista FOREIGN KEY (id_analista) REFERENCES Usuario(US_Cedula);

---- Prueba constraints
--ALTER TABLE Prueba ADD CONSTRAINT fk_prueba_tipo FOREIGN KEY (tipo_muestra_asociada) REFERENCES Tipo_Muestra(TPMST_ID);
--ALTER TABLE Prueba ADD CONSTRAINT uk_prueba UNIQUE (nombre_prueba, tipo_muestra_asociada);

---- Parametro_Norma constraints
--ALTER TABLE Parametro_Norma ADD CONSTRAINT fk_parnorma_prueba FOREIGN KEY (id_prueba) REFERENCES Prueba(id_prueba);
--ALTER TABLE Parametro_Norma ADD CONSTRAINT uk_parametro UNIQUE (id_prueba, nombre_parametro);

---- Resultado_Prueba constraints
--ALTER TABLE Resultado_Prueba ADD CONSTRAINT fk_res_prueba FOREIGN KEY (id_prueba) REFERENCES Prueba(id_prueba);
--ALTER TABLE Resultado_Prueba ADD CONSTRAINT fk_res_muestra FOREIGN KEY (id_muestra) REFERENCES Muestra(MST_CODIGO);
--ALTER TABLE Resultado_Prueba ADD CONSTRAINT fk_res_validador FOREIGN KEY (validado_por) REFERENCES Usuario(US_Cedula);

---- Documento constraints
--ALTER TABLE Documento ADD CONSTRAINT fk_doc_muestra FOREIGN KEY (id_muestra) REFERENCES Muestra(MST_CODIGO);
--ALTER TABLE Documento ADD CONSTRAINT fk_doc_tipodoc FOREIGN KEY (id_tipo_doc) REFERENCES Tipo_Documento(id_tipo_doc);

---- Historial_Trazabilidad constraints
--ALTER TABLE Historial_Trazabilidad ADD CONSTRAINT fk_ht_muestra FOREIGN KEY (id_muestra) REFERENCES Muestra(MST_CODIGO);
--ALTER TABLE Historial_Trazabilidad ADD CONSTRAINT fk_ht_usuario FOREIGN KEY (id_usuario) REFERENCES Usuario(US_Cedula);
--ALTER TABLE Historial_Trazabilidad ADD CONSTRAINT fk_ht_estado FOREIGN KEY (estado) REFERENCES Estado_Muestra(id_estado);

---- Notificacion constraints
--ALTER TABLE Notificacion ADD CONSTRAINT fk_not_muestra FOREIGN KEY (id_muestra) REFERENCES Muestra(MST_CODIGO);

---- Auditoria constraints
--ALTER TABLE Auditoria ADD CONSTRAINT fk_aud_usuario FOREIGN KEY (id_usuario) REFERENCES Usuario(US_Cedula);
--GO

---- =========================
---- 4) CREATE INDEXES
---- =========================

--CREATE INDEX IX_Bitacora_Muestra_id_muestra ON Bitacora_Muestra(id_muestra);
--CREATE INDEX IX_Bitacora_Muestra_id_analista ON Bitacora_Muestra(id_analista);
--CREATE INDEX IX_Resultado_Prueba_id_muestra ON Resultado_Prueba(id_muestra);
--CREATE INDEX IX_Resultado_Prueba_id_prueba ON Resultado_Prueba(id_prueba);
--CREATE INDEX IX_Documento_id_muestra ON Documento(id_muestra);
--CREATE INDEX IX_Historial_Trazabilidad_id_muestra ON Historial_Trazabilidad(id_muestra);
--CREATE INDEX IX_Historial_Trazabilidad_id_usuario ON Historial_Trazabilidad(id_usuario);
--CREATE INDEX IX_Notificacion_id_muestra ON Notificacion(id_muestra);
--CREATE INDEX IX_Auditoria_id_usuario ON Auditoria(id_usuario);
--CREATE INDEX idx_muestra_fechas ON Muestra(Fecha_recepcion, Fecha_Salida_Estimada);
--CREATE INDEX idx_resultado_muestra_fecha ON Resultado_Prueba(id_muestra, fecha_registro);
--GO

-- =========================
-- 5) SEED BASICO
-- =========================

-- INSERT INTO Rol_Usuario (id_rol, nombre_rol) VALUES
-- (1,'Solicitante'),(2,'Analista'),(3,'Evaluador'),(4,'Administrador');

-- INSERT INTO Tipo_Muestra (TPMST_ID, nombre) VALUES
-- (1,'Agua'),(2,'Alimento'),(3,'Bebida alcoholica');

-- INSERT INTO Estado_Muestra (id_estado, nombre) VALUES
-- (1,'Recibida'),(2,'En analisis'),(3,'Evaluada'),(4,'Certificada');

-- INSERT INTO Tipo_Documento (id_tipo_doc, nombre) VALUES
-- (1,'Certificado'),(2,'Informe');
-- GO

-- =========================
-- 6) STORED PROCEDURES
-- =========================

-- 6.1 Crear muestra (inicia en 'Recibida' y traza historial)
-- Create the procedure with updated parameter types
CREATE OR ALTER PROCEDURE sp_crear_muestra 
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
GO

-- 6.2 Asignar analista a una muestra (bitácora + set analista actual)
CREATE OR ALTER PROCEDURE sp_asignar_analista 
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
GO

-- 6.3 Cambiar estado de la muestra (y trazar historial)
CREATE OR ALTER PROCEDURE sp_cambiar_estado 
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
GO

CREATE OR ALTER PROCEDURE sp_cambiar_estado_documento
    @p_id_documento INT,
    @p_id_estado_doc INT,
    @p_observaciones VARCHAR(255),
    @p_id_usuario VARCHAR(450)
AS
BEGIN
    UPDATE Documento SET id_estado_documento = @p_id_estado_doc WHERE id_documento = @p_id_documento;

    INSERT INTO Historial_Trazabilidad (id_documento, id_usuario, estado, observaciones)
    VALUES (@p_id_documento, @p_id_usuario, @p_id_estado_doc, @p_observaciones);

    INSERT INTO Auditoria (id_usuario, accion, descripcion)
    VALUES (@p_id_usuario, 'CAMBIAR_ESTADO_DOCUMENTO', CONCAT('DOC=',@p_id_documento,', ESTADO=',CAST(@p_id_estado_doc AS VARCHAR(10))));
END
GO

CREATE OR ALTER PROCEDURE sp_crear_prueba
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


CREATE OR ALTER PROCEDURE sp_agregar_parametro_a_tipo_muestra
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
GO

DROP PROCEDURE sp_agregar_parametro_a_prueba
GO

-- 6.4 Registrar resultado de una prueba con evaluación de norma
CREATE OR ALTER PROCEDURE sp_registrar_resultado 
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
GO

-- 6.5 Generar registro de documento
CREATE OR ALTER PROCEDURE sp_generar_documento 
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
GO

CREATE OR ALTER PROCEDURE dbo.sp_validar_resultado
    @id_resultado INT,
    @id_usuario   VARCHAR(450),
    @accion       NVARCHAR(20),  -- 'Aprobado' o 'Rechazado'
    @obs          NVARCHAR(300) = NULL
AS
BEGIN
    IF @accion NOT IN ('Aprobado','Rechazado')
        BEGIN
            RAISERROR('Acción inválida. Use Aprobado o Rechazado.',16,1);
            RETURN;
        END

    -- Actualiza el resultado con la decisión
    UPDATE dbo.Resultado_Prueba
    SET validado_por = @id_usuario,
        estado_validacion = @accion
    WHERE id_resultado = @id_resultado;

    -- Si fue rechazado, la muestra regresa a análisis
    IF @accion = 'Rechazado'
        BEGIN
            DECLARE @muestra VARCHAR(30);
            SELECT @muestra = id_muestra FROM dbo.Resultado_Prueba WHERE id_resultado = @id_resultado;

            UPDATE dbo.Muestra
            SET estado_actual = 'En análisis'
            WHERE MST_CODIGO = @muestra;

            INSERT INTO dbo.Historial_Trazabilidad(id_muestra,id_usuario,estado,observaciones)
            VALUES(@muestra,@id_usuario,2,@obs);
        END
    ELSE
        BEGIN
            -- Si fue aprobado, registrar trazabilidad normal
            DECLARE @muestraA VARCHAR(30);
            SELECT @muestraA = id_muestra FROM dbo.Resultado_Prueba WHERE id_resultado = @id_resultado;

            INSERT INTO dbo.Historial_Trazabilidad(id_muestra,id_usuario,estado,observaciones)
            VALUES(@muestraA,@id_usuario,3,@obs);
        END
END
GO

-- 6.6 Registrar notificación
CREATE OR ALTER PROCEDURE sp_registrar_notificacion 
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
GO
