using DB.Datos.DocumentoD.Models;
using Models;

namespace DB.Datos.DocumentoD.Repositorios;

public class DocumentoRepositorio
{
    private readonly Conexion _conexion;

    public DocumentoRepositorio()
    {
        _conexion = Conexion.Instance;
    }

    public async Task<IEnumerable<Documento>> ObtenerDocumentosAsync()
    {
        try
        {
            var documentos = new List<Documento>();
            await _conexion.Connection.OpenAsync();
            using (var command = _conexion.Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Documentos";
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        documentos.Add(new Documento
                        {
                            IdDocumento = reader.GetInt32(0),
                            FechaCreacion = reader.GetDateTime(3)
                        });
                    }
                }
            }

            await _conexion.Connection.CloseAsync();
            return documentos;
        }
        catch (Exception ex)
        {
            await _conexion.Connection.CloseAsync();
            throw new Exception("Error al obtener los documentos", ex);
        }
        finally
        {
            if (_conexion.Connection.State == System.Data.ConnectionState.Open)
            {
                await _conexion.Connection.CloseAsync();
            }
        }
    }

    public async Task<Documento> ObtenerDocumentoAsync(int id)
    {
        try
        {
            Documento documento = null;
            await _conexion.Connection.OpenAsync();
            using (var command = _conexion.Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Documentos WHERE IdDocumento = @Id";
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        documento = new Documento
                        {
                            IdDocumento = reader.GetInt32(0),
                            FechaCreacion = reader.GetDateTime(3)
                        };
                    }
                }
            }

            await _conexion.Connection.CloseAsync();
            return documento;
        }
        catch (Exception ex)
        {
            await _conexion.Connection.CloseAsync();
            throw new Exception("Error al obtener el documento", ex);
        }
        finally
        {
            if (_conexion.Connection.State == System.Data.ConnectionState.Open)
            {
                await _conexion.Connection.CloseAsync();
            }
        }
    }

    public async Task GenerarDocumentoSpAsync(CreateDocumentoDto documentoDto)
    {
        // sp_generar_documento
        /* 
            -- 6.5 Generar registro de documento
            CREATE PROCEDURE sp_generar_documento 
                @p_MST_CODIGO VARCHAR(30),
                @p_id_tipo_doc TINYINT,
                @p_version INT,
                @p_ruta VARCHAR(255),
                @p_id_usuario VARCHAR(15)
            AS
            BEGIN
                INSERT INTO Documento (id_muestra, id_tipo_doc, version, ruta_archivo)
                VALUES (@p_MST_CODIGO, @p_id_tipo_doc, @p_version, @p_ruta);

                INSERT INTO Auditoria (id_usuario, accion, descripcion)
                VALUES (@p_id_usuario, 'GENERAR_DOCUMENTO', CONCAT('MST=',@p_MST_CODIGO,', DOC=',CAST(@p_id_tipo_doc AS VARCHAR(10)),', v',CAST(@p_version AS VARCHAR(10))));
            END
            go
        */
        
        // consumir el stored procedure sp_generar_documento
        try
        {
            await _conexion.Connection.OpenAsync();
            using (var command = _conexion.Connection.CreateCommand())
            {
                command.CommandText = "sp_generar_documento";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                // Agregar parámetros necesarios
                command.Parameters.AddWithValue("@p_MST_CODIGO", "Muestra123");
                command.Parameters.AddWithValue("@p_id_tipo_doc", 1);
                command.Parameters.AddWithValue("@p_version", 1);
                command.Parameters.AddWithValue("@p_ruta", "/ruta/al/documento.pdf");
                command.Parameters.AddWithValue("@p_id_usuario", "usuario123");

                await command.ExecuteNonQueryAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al generar el documento", ex);
        }
        finally
        {
            if (_conexion.Connection.State == System.Data.ConnectionState.Open)
            {
                await _conexion.Connection.CloseAsync();
            }
        }
    }
}