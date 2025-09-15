using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDAD.DTOs;
using ENTIDAD.DTOs.Documentos;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DB.Datos.Repositorios
{
    public class DocumentoRepositorio
    {
        private readonly MasterDbContext _context;

        public DocumentoRepositorio(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DocumentoDto>> ObtenerDocumentosAsync()
        {
            try
            {
                return await _context.Documentos
                    .Select(d => new DocumentoDto
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

        public async Task<DocumentoDto?> ObtenerDocumentoAsync(int id)
        {
            try
            {
                return await _context.Documentos
                    .Where(d => d.IdDocumento == id)
                    .Select(d => new DocumentoDto
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

        public async Task<DocumentoDto?> GenerarDocumentoSpAsync(CreateDocumentoDto createDocumentoDto, string idUsuario)
        {
            try
            {
                // Using Entity Framework to execute stored procedure
                var result = await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_generar_documento @p_MST_CODIGO = {0}, @p_id_tipo_doc = {1}, @p_id_estado_documento = {2}, @p_version = {3}, @p_ruta = {4}, @p_doc_pdf = {5}, @p_id_usuario = {6}",
                    createDocumentoDto.IdMuestra,
                    createDocumentoDto.IdTipoDoc,
                    createDocumentoDto.IdEstadoDocumento,
                    createDocumentoDto.Version,
                    createDocumentoDto.RutaArchivo,
                    createDocumentoDto.DocPdf,
                    idUsuario);
                
                if (result <= 0)
                {
                    throw new Exception("No se pudo generar el documento");
                }
                
                var documentoDto = await _context.Documentos
                    .Where(d => d.IdMuestra == createDocumentoDto.IdMuestra && d.IdTipoDoc == createDocumentoDto.IdTipoDoc && d.Version == createDocumentoDto.Version)
                    .Select(d => new DocumentoDto
                    {
                        IdDocumento = d.IdDocumento,
                        FechaCreacion = d.FechaCreacion,
                        IdMuestra = d.IdMuestra,
                        IdTipoDoc = d.IdTipoDoc,
                        IdEstadoDocumento = d.IdEstadoDocumento,
                        Version = d.Version,
                        RutaArchivo = d.RutaArchivo,
                        DocPdf = d.DocPdf
                    })
                    .FirstOrDefaultAsync();

                return documentoDto;
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

        public async Task<DocumentoDto?> CambiarEstadoDocumentoAsync(CambiarEstadoDocumentoDto cambiarEstadoDocumentoDto, string idUsuario) {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_cambiar_estado_documento @p_id_documento = {0}, @p_id_estado_doc = {1}, @p_observaciones = {2}, @p_id_usuario = {3}",
                cambiarEstadoDocumentoDto.IdDocumento,
                cambiarEstadoDocumentoDto.IdEstadoDocumento,
                cambiarEstadoDocumentoDto.Observaciones,
                idUsuario
            );

            if (result <= 0)
            {
                throw new Exception("No se pudo cambiar el estado del documento");
            }

            return await _context.Documentos
                .Where(d => d.IdDocumento == cambiarEstadoDocumentoDto.IdDocumento)
                .Select(d => new DocumentoDto
                {
                    IdDocumento = d.IdDocumento,
                    FechaCreacion = d.FechaCreacion,
                    IdMuestra = d.IdMuestra,
                    IdTipoDoc = d.IdTipoDoc,
                    IdEstadoDocumento = d.IdEstadoDocumento,
                    Version = d.Version,
                    RutaArchivo = d.RutaArchivo,
                    DocPdf = d.DocPdf
                })
                .FirstOrDefaultAsync();
        }

    }
}
