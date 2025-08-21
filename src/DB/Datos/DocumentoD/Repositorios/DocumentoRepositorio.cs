using DB.Datos.DocumentoD.Models;
using Models;
using Microsoft.EntityFrameworkCore;

namespace DB.Datos.DocumentoD.Repositorios;

public class DocumentoRepositorio
{
    private readonly MasterDbContext _context;

    public DocumentoRepositorio()
    {
        _context = MasterDbContext.Instance;
    }

    public async Task<IEnumerable<Documento>> ObtenerDocumentosAsync()
    {
        try
        {
            return await _context.Documentos
                .Select(d => new Documento
                {
                    IdDocumento = d.IdDocumento,
                    FechaCreacion = d.FechaCreacion,
                    IdMuestra = d.IdMuestra,
                    IdTipoDoc = d.IdTipoDoc,
                    Version = d.Version,
                    RutaArchivo = d.RutaArchivo,
                    DocPdf = d.DocPdf
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los documentos", ex);
        }
    }

    public async Task<Documento?> ObtenerDocumentoAsync(int id)
    {
        try
        {
            return await _context.Documentos
                .Where(d => d.IdDocumento == id)
                .Select(d => new Documento
                {
                    IdDocumento = d.IdDocumento,
                    FechaCreacion = d.FechaCreacion,
                    IdMuestra = d.IdMuestra,
                    IdTipoDoc = d.IdTipoDoc,
                    Version = d.Version,
                    RutaArchivo = d.RutaArchivo,
                    DocPdf = d.DocPdf
                })
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el documento", ex);
        }
    }

    public async Task GenerarDocumentoSpAsync(CreateDocumentoDto documentoDto, int idUsuario)
    {
        try
        {
            // Using Entity Framework to execute stored procedure
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_generar_documento @p_MST_CODIGO = {0}, @p_id_tipo_doc = {1}, @p_version = {2}, @p_ruta = {3}, @p_id_usuario = {4}",
                documentoDto.IdMuestra,
                documentoDto.IdTipoDoc,
                documentoDto.Version,
                documentoDto.RutaArchivo,
                idUsuario);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al generar el documento", ex);
        }
    }

    // Alternative approach using Entity Framework entities instead of stored procedure
    public async Task GenerarDocumentoAsync(CreateDocumentoDto documentoDto, string idUsuario)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            // Create the document
            var documento = new Documento
            {
                IdMuestra = documentoDto.IdMuestra,
                IdTipoDoc = documentoDto.IdTipoDoc,
                Version = documentoDto.Version,
                RutaArchivo = documentoDto.RutaArchivo,
                FechaCreacion = DateTime.Now
            };

            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();

            // Create audit record
            var auditoria = new Auditorium
            {
                IdUsuario = idUsuario,
                Accion = "GENERAR_DOCUMENTO",
                Descripcion = $"MST={documentoDto.IdMuestra}, DOC={documentoDto.IdTipoDoc}, v{documentoDto.Version}",
                FechaAccion = DateTime.Now
            };

            _context.Auditoria.Add(auditoria);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Error al generar el documento", ex);
        }
    }
}