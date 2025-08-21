-- =========================================================
--  DB: Laboratorio_Muestras  (MySQL 8.0)
--  Modela Reto App Web (ago-oct 2025) + tu diseño de BD
--  Autor: generado por ChatGPT
-- =========================================================

/*DROP DATABASE IF EXISTS Laboratorio_Muestras;
CREATE DATABASE Laboratorio_Muestras CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE Laboratorio_Muestras;
*/
    
-- =========================
-- 1) CATALOGOS / LOOKUPS
-- =========================

CREATE TABLE Rol_Usuario (
                             id_rol        TINYINT UNSIGNED PRIMARY KEY,
                             nombre_rol    VARCHAR(30) NOT NULL UNIQUE
) ENGINE=InnoDB;

CREATE TABLE Tipo_Muestra (
                              TPMST_ID      TINYINT UNSIGNED PRIMARY KEY,
                              nombre        VARCHAR(40) NOT NULL UNIQUE  -- Agua, Alimento, Bebida alcohólica
) ENGINE=InnoDB;

CREATE TABLE Estado_Muestra (
                                id_estado     TINYINT UNSIGNED PRIMARY KEY,
                                nombre        VARCHAR(30) NOT NULL UNIQUE  -- Recibida, En análisis, Evaluada, Certificada
) ENGINE=InnoDB;

CREATE TABLE Tipo_Documento (
                                id_tipo_doc   TINYINT UNSIGNED PRIMARY KEY,
                                nombre        VARCHAR(30) NOT NULL UNIQUE  -- Certificado, Informe, etc.
) ENGINE=InnoDB;

-- =========================
-- 2) NUCLEO
-- =========================

CREATE TABLE Usuario (
                         US_Cedula         VARCHAR(15) PRIMARY KEY,        -- cédula o NIT/RNC; string para conservar ceros
                         nombre            VARCHAR(80) NOT NULL,
                         apellido          VARCHAR(80) NOT NULL,
                         razon_social      VARCHAR(120) NULL,
                         Direccion         VARCHAR(200) NULL,
                         Correo            VARCHAR(120) NOT NULL,
                         Telefono          VARCHAR(30)  NULL,
                         CONTACTO          VARCHAR(120) NULL,              -- persona contacto si es empresa
                         Username          VARCHAR(60)  NOT NULL UNIQUE,
                         contrasena        VARCHAR(255) NOT NULL,          -- hash
                         id_rol            TINYINT UNSIGNED NOT NULL,
                         CONSTRAINT fk_usuario_rol FOREIGN KEY (id_rol) REFERENCES Rol_Usuario(id_rol)
) ENGINE=InnoDB;

CREATE TABLE Muestra (
                         MST_CODIGO            VARCHAR(30) PRIMARY KEY,    -- código único de la muestra
                         TPMST_ID              TINYINT UNSIGNED NOT NULL,  -- FK a Tipo_Muestra
                         Nombre                VARCHAR(120) NULL,          -- nombre/alias de la muestra si aplica
                         Fecha_recepcion       DATETIME NOT NULL,          -- (pedido explícito) :contentReference[oaicite:2]{index=2}
                         origen                VARCHAR(200) NOT NULL,      -- fabricante, distribuidor, punto de toma
                         Fecha_Salida_Estimada DATETIME NULL,
                         Condiciones_almacenamiento VARCHAR(200) NULL,
                         Condiciones_transporte    VARCHAR(200) NULL,
                         id_usuario_solicitante    VARCHAR(15) NOT NULL,   -- US_Cedula solicitante (rol = solicitante)
                         id_Analista               VARCHAR(15) NULL,       -- US_Cedula analista asignado principal
                         estado_actual             TINYINT UNSIGNED NOT NULL,
                         CONSTRAINT fk_muestra_tipo     FOREIGN KEY (TPMST_ID) REFERENCES Tipo_Muestra(TPMST_ID),
                         CONSTRAINT fk_muestra_solic    FOREIGN KEY (id_usuario_solicitante) REFERENCES Usuario(US_Cedula),
                         CONSTRAINT fk_muestra_analista FOREIGN KEY (id_Analista) REFERENCES Usuario(US_Cedula),
                         CONSTRAINT fk_muestra_estado   FOREIGN KEY (estado_actual) REFERENCES Estado_Muestra(id_estado)
) ENGINE=InnoDB;

CREATE TABLE Bitacora_Muestra (
                                  id_bitacora    BIGINT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
                                  id_muestra     VARCHAR(30) NOT NULL,
                                  id_analista    VARCHAR(15) NOT NULL,
                                  fecha_asignacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                  observaciones  VARCHAR(255) NULL,
                                  CONSTRAINT fk_bit_muestra  FOREIGN KEY (id_muestra) REFERENCES Muestra(MST_CODIGO),
                                  CONSTRAINT fk_bit_analista FOREIGN KEY (id_analista) REFERENCES Usuario(US_Cedula),
                                  INDEX (id_muestra), INDEX(id_analista)
) ENGINE=InnoDB;

CREATE TABLE Prueba (
                        id_prueba          INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
                        nombre_prueba      VARCHAR(120) NOT NULL,
                        tipo_muestra_asociada TINYINT UNSIGNED NOT NULL,  -- restringe por tipo
                        norma_referencia   VARCHAR(120) NULL,             -- Codex, NORDOM, etc.
                        CONSTRAINT fk_prueba_tipo FOREIGN KEY (tipo_muestra_asociada) REFERENCES Tipo_Muestra(TPMST_ID),
                        UNIQUE KEY uk_prueba (nombre_prueba, tipo_muestra_asociada)
) ENGINE=InnoDB;

CREATE TABLE Parametro_Norma (
                                 id_parametro       INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
                                 id_prueba          INT UNSIGNED NOT NULL,         -- acoplar norma a prueba
                                 nombre_parametro   VARCHAR(120) NOT NULL,         -- pH, turbidez, etc.
                                 valor_min          DECIMAL(18,6) NULL,
                                 valor_max          DECIMAL(18,6) NULL,            -- añadido para comparar rangos completos
                                 unidad             VARCHAR(30)  NULL,
                                 CONSTRAINT fk_parnorma_prueba FOREIGN KEY (id_prueba) REFERENCES Prueba(id_prueba),
                                 UNIQUE KEY uk_parametro (id_prueba, nombre_parametro)
) ENGINE=InnoDB;

CREATE TABLE Resultado_Prueba (
                                  id_resultado     BIGINT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
                                  id_prueba        INT UNSIGNED NOT NULL,
                                  id_muestra       VARCHAR(30) NOT NULL,
                                  valor_obtenido   DECIMAL(18,6) NULL,
                                  unidad           VARCHAR(30)  NULL,
                                  cumple_norma     TINYINT(1)   NULL,               -- 1 Sí / 0 No / NULL si no aplica
                                  fecha_registro   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                  validado_por     VARCHAR(15)  NULL,               -- US_Cedula evaluador/validador
                                  CONSTRAINT fk_res_prueba   FOREIGN KEY (id_prueba)  REFERENCES Prueba(id_prueba),
                                  CONSTRAINT fk_res_muestra  FOREIGN KEY (id_muestra) REFERENCES Muestra(MST_CODIGO),
                                  CONSTRAINT fk_res_validador FOREIGN KEY (validado_por) REFERENCES Usuario(US_Cedula),
                                  INDEX (id_muestra), INDEX(id_prueba)
) ENGINE=InnoDB;

CREATE TABLE Documento (
                           id_documento     BIGINT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
                           id_muestra       VARCHAR(30) NOT NULL,
                           id_tipo_doc      TINYINT UNSIGNED NOT NULL,
                           version          INT UNSIGNED NOT NULL DEFAULT 1,
                           ruta_archivo     VARCHAR(255) NULL,               -- soporte a “almacenamiento digital”
                           fecha_creacion   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                           CONSTRAINT fk_doc_muestra  FOREIGN KEY (id_muestra)  REFERENCES Muestra(MST_CODIGO),
                           CONSTRAINT fk_doc_tipodoc  FOREIGN KEY (id_tipo_doc) REFERENCES Tipo_Documento(id_tipo_doc),
                           INDEX(id_muestra)
) ENGINE=InnoDB;

CREATE TABLE Historial_Trazabilidad (
                                        id_historial     BIGINT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
                                        id_muestra       VARCHAR(30) NOT NULL,
                                        id_usuario       VARCHAR(15) NOT NULL,            -- responsable del cambio
                                        estado           TINYINT UNSIGNED NOT NULL,
                                        fecha_cambio     DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        observaciones    VARCHAR(255) NULL,
                                        CONSTRAINT fk_ht_muestra FOREIGN KEY (id_muestra) REFERENCES Muestra(MST_CODIGO),
                                        CONSTRAINT fk_ht_usuario FOREIGN KEY (id_usuario) REFERENCES Usuario(US_Cedula),
                                        CONSTRAINT fk_ht_estado  FOREIGN KEY (estado)     REFERENCES Estado_Muestra(id_estado),
                                        INDEX(id_muestra), INDEX(id_usuario)
) ENGINE=InnoDB;

CREATE TABLE Notificacion (
                              id_notificacion  BIGINT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
                              id_muestra       VARCHAR(30) NOT NULL,
                              tipo_alerta      VARCHAR(60) NOT NULL,            -- Plazo, Fuera de norma, etc. (del reto) :contentReference[oaicite:3]{index=3}
                              destinatario     VARCHAR(120) NOT NULL,
                              enviado          TINYINT(1) NOT NULL DEFAULT 0,
                              fecha_envio      DATETIME NULL,
                              detalle          VARCHAR(255) NULL,
                              CONSTRAINT fk_not_muestra FOREIGN KEY (id_muestra) REFERENCES Muestra(MST_CODIGO),
                              INDEX(id_muestra)
) ENGINE=InnoDB;

CREATE TABLE Auditoria (
                           id_auditoria     BIGINT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
                           id_usuario       VARCHAR(15) NOT NULL,
                           accion           VARCHAR(120) NOT NULL,
                           fecha_accion     DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                           descripcion      VARCHAR(255) NULL,
                           CONSTRAINT fk_aud_usuario FOREIGN KEY (id_usuario) REFERENCES Usuario(US_Cedula),
                           INDEX(id_usuario)
) ENGINE=InnoDB;

-- =========================
-- 3) SEED BASICO
-- =========================

INSERT INTO Rol_Usuario (id_rol, nombre_rol) VALUES
                                                 (1,'Solicitante'),(2,'Analista'),(3,'Evaluador'),(4,'Administrador');

INSERT INTO Tipo_Muestra (TPMST_ID, nombre) VALUES
                                                (1,'Agua'),(2,'Alimento'),(3,'Bebida alcohólica');

INSERT INTO Estado_Muestra (id_estado, nombre) VALUES
                                                   (1,'Recibida'),(2,'En análisis'),(3,'Evaluada'),(4,'Certificada');

INSERT INTO Tipo_Documento (id_tipo_doc, nombre) VALUES
                                                     (1,'Certificado'),(2,'Informe');

-- =========================
-- 4) STORED PROCEDURES
-- =========================

DELIMITER $$

-- 4.1 Crear muestra (inicia en 'Recibida' y traza historial)
CREATE PROCEDURE sp_crear_muestra (
    IN p_MST_CODIGO VARCHAR(30),
    IN p_TPMST_ID TINYINT UNSIGNED,
    IN p_Nombre VARCHAR(120),
    IN p_Fecha_recepcion DATETIME,
    IN p_origen VARCHAR(200),
    IN p_Fecha_Salida_Estimada DATETIME,
    IN p_Cond_alm VARCHAR(200),
    IN p_Cond_trans VARCHAR(200),
    IN p_id_solicitante VARCHAR(15),
    IN p_id_responsable VARCHAR(15)    -- responsable técnico inicial (puede ser NULL)
)
BEGIN
  DECLARE v_estado_recibida TINYINT UNSIGNED DEFAULT 1;

INSERT INTO Muestra
(MST_CODIGO, TPMST_ID, Nombre, Fecha_recepcion, origen, Fecha_Salida_Estimada,
 Condiciones_almacenamiento, Condiciones_transporte, id_usuario_solicitante,
 id_Analista, estado_actual)
VALUES
    (p_MST_CODIGO, p_TPMST_ID, p_Nombre, p_Fecha_recepcion, p_origen, p_Fecha_Salida_Estimada,
     p_Cond_alm, p_Cond_trans, p_id_solicitante, p_id_responsable, v_estado_recibida);

INSERT INTO Historial_Trazabilidad (id_muestra, id_usuario, estado, observaciones)
VALUES (p_MST_CODIGO, COALESCE(p_id_responsable,p_id_solicitante), v_estado_recibida, 'Creación de muestra');

INSERT INTO Auditoria (id_usuario, accion, descripcion)
VALUES (COALESCE(p_id_responsable,p_id_solicitante), 'CREAR_MUESTRA', CONCAT('MST=',p_MST_CODIGO));
END$$


-- 4.2 Asignar analista a una muestra (bitácora + set analista actual)
CREATE PROCEDURE sp_asignar_analista (
    IN p_MST_CODIGO VARCHAR(30),
    IN p_id_analista VARCHAR(15),
    IN p_observaciones VARCHAR(255)
)
BEGIN
INSERT INTO Bitacora_Muestra (id_muestra, id_analista, observaciones)
VALUES (p_MST_CODIGO, p_id_analista, p_observaciones);

UPDATE Muestra SET id_Analista = p_id_analista WHERE MST_CODIGO = p_MST_CODIGO;

INSERT INTO Auditoria (id_usuario, accion, descripcion)
VALUES (p_id_analista, 'ASIGNAR_ANALISTA', CONCAT('MST=',p_MST_CODIGO));
END$$


-- 4.3 Cambiar estado de la muestra (y trazar historial)
CREATE PROCEDURE sp_cambiar_estado (
    IN p_MST_CODIGO VARCHAR(30),
    IN p_nuevo_estado TINYINT UNSIGNED,
    IN p_id_usuario VARCHAR(15),
    IN p_observaciones VARCHAR(255)
)
BEGIN
UPDATE Muestra SET estado_actual = p_nuevo_estado WHERE MST_CODIGO = p_MST_CODIGO;

INSERT INTO Historial_Trazabilidad (id_muestra, id_usuario, estado, observaciones)
VALUES (p_MST_CODIGO, p_id_usuario, p_nuevo_estado, p_observaciones);

INSERT INTO Auditoria (id_usuario, accion, descripcion)
VALUES (p_id_usuario, 'CAMBIAR_ESTADO', CONCAT('MST=',p_MST_CODIGO,', ESTADO=',p_nuevo_estado));
END$$


-- 4.4 Registrar resultado de una prueba con evaluación de norma
--     Si está fuera de norma, genera registro en Notificacion (para trazabilidad).
CREATE PROCEDURE sp_registrar_resultado (
    IN p_MST_CODIGO VARCHAR(30),
    IN p_id_prueba INT UNSIGNED,
    IN p_valor DECIMAL(18,6),
    IN p_unidad VARCHAR(30),
    IN p_validado_por VARCHAR(15)
)
BEGIN
  DECLARE v_cumple TINYINT(1);
  DECLARE v_min DECIMAL(18,6);
  DECLARE v_max DECIMAL(18,6);
  DECLARE v_param VARCHAR(120);

  -- Tomamos 1er parámetro de norma de esa prueba (si hay múltiples parámetros, podrías extender a tabla detalle)
SELECT nombre_parametro, valor_min, valor_max
INTO v_param, v_min, v_max
FROM Parametro_Norma
WHERE id_prueba = p_id_prueba
ORDER BY id_parametro
    LIMIT 1;

-- Regla: si no hay norma registrada, se deja NULL; si hay, se evalúa en rango [min,max] ignorando nulos
SET v_cumple = NULL;
  IF v_min IS NOT NULL OR v_max IS NOT NULL THEN
    SET v_cumple = 1;
    IF v_min IS NOT NULL AND p_valor < v_min THEN SET v_cumple = 0; END IF;
    IF v_max IS NOT NULL AND p_valor > v_max THEN SET v_cumple = 0; END IF;
END IF;

INSERT INTO Resultado_Prueba
(id_prueba, id_muestra, valor_obtenido, unidad, cumple_norma, validado_por)
VALUES
    (p_id_prueba, p_MST_CODIGO, p_valor, p_unidad, v_cumple, p_validado_por);

-- Notificación si no cumple
IF v_cumple = 0 THEN
    INSERT INTO Notificacion (id_muestra, tipo_alerta, destinatario, enviado, detalle)
SELECT p_MST_CODIGO, 'Resultado fuera de norma', u.Correo, 0,
       CONCAT('Prueba ', pr.nombre_prueba, ' valor=', p_valor,
              ' (rango ', IFNULL(CONCAT('[',v_min,','), '['),
              IFNULL(CONCAT(v_max,']'), '∞)'))
FROM Muestra m
         JOIN Usuario u ON u.US_Cedula = m.id_usuario_solicitante
         JOIN Prueba pr ON pr.id_prueba = p_id_prueba
WHERE m.MST_CODIGO = p_MST_CODIGO;
END IF;

INSERT INTO Auditoria (id_usuario, accion, descripcion)
VALUES (p_validado_por, 'REGISTRAR_RESULTADO', CONCAT('MST=',p_MST_CODIGO,', PRB=',p_id_prueba));
END$$


-- 4.5 Generar registro de documento (metadato; el archivo lo gestiona la app/FS)
CREATE PROCEDURE sp_generar_documento (
    IN p_MST_CODIGO VARCHAR(30),
    IN p_id_tipo_doc TINYINT UNSIGNED,
    IN p_version INT UNSIGNED,
    IN p_ruta VARCHAR(255),
    IN p_id_usuario VARCHAR(15)
)
BEGIN
INSERT INTO Documento (id_muestra, id_tipo_doc, version, ruta_archivo)
VALUES (p_MST_CODIGO, p_id_tipo_doc, p_version, p_ruta);

INSERT INTO Auditoria (id_usuario, accion, descripcion)
VALUES (p_id_usuario, 'GENERAR_DOCUMENTO', CONCAT('MST=',p_MST_CODIGO,', DOC=',p_id_tipo_doc,', v',p_version));
END$$


-- 4.6 Registrar notificación (para trazabilidad; el envío real lo hace la app)
CREATE PROCEDURE sp_registrar_notificacion (
    IN p_MST_CODIGO VARCHAR(30),
    IN p_tipo_alerta VARCHAR(60),
    IN p_destinatario VARCHAR(120),
    IN p_detalle VARCHAR(255),
    IN p_enviado TINYINT(1),
    IN p_fecha_envio DATETIME
        )
BEGIN
INSERT INTO Notificacion (id_muestra, tipo_alerta, destinatario, detalle, enviado, fecha_envio)
VALUES (p_MST_CODIGO, p_tipo_alerta, p_destinatario, p_detalle, p_enviado, p_fecha_envio);
END$$

DELIMITER ;

-- =========================
-- 5) INDICES UTILES EXTRA
-- =========================
CREATE INDEX idx_muestra_fechas ON Muestra(Fecha_recepcion, Fecha_Salida_Estimada);
CREATE INDEX idx_resultado_muestra_fecha ON Resultado_Prueba(id_muestra, fecha_registro);