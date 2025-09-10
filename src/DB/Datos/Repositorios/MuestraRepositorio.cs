using ENTIDAD.DTOs.Muestras;
using ENTIDAD.DTOs.Documentos;
using Microsoft.EntityFrameworkCore;
using Models;
using System.IO;
using Aspose.Cells;
using DB.Helpers;
using ENTIDAD.DTOs.ResultadosPruebas;
using ENTIDAD.Models;
using Microsoft.AspNetCore.Identity;

namespace DB.Datos.Repositorios;

public class MuestraRepositorio
{
    private readonly IDocumentBuilder _documentBuilder;
    private readonly MasterDbContext _context;
    private readonly ResultadoRepositorio _resultadoRepositorio;
    private readonly ParametroRepositorio _parametroRepositorio;
    private readonly UsuarioRepositorio _usuarioRepositorio;
    private readonly UserManager<Usuario> _userManager;

    public MuestraRepositorio(MasterDbContext context, UserManager<Usuario> userManager, IDocumentBuilder documentBuilder)
    {
        _context = context;
        _resultadoRepositorio = new ResultadoRepositorio(context);
        _parametroRepositorio = new ParametroRepositorio(context);
        _usuarioRepositorio = new UsuarioRepositorio(context, userManager);
        _documentBuilder = documentBuilder;
    }
    public async Task<IEnumerable<MuestraDto>> ObtenerTodasLasMuestrasAsync()
    {
        return await _context.Muestras
            .Include(m => m.Tpmst)
            .Include(m => m.EstadoActualNavigation)
            .Select(m => new MuestraDto
            {
                MstCodigo = m.MstCodigo,
                TpmstId = m.TpmstId,
                Nombre = m.Nombre,
                Origen = m.Origen,
                CondicionesAlmacenamiento = m.CondicionesAlmacenamiento,
                CondicionesTransporte = m.CondicionesTransporte,
                EstadoActual = m.EstadoActual,
                FechaRecepcion = m.FechaRecepcion,
                FechaSalidaEstimada = m.FechaSalidaEstimada
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<MuestraDto>> ObtenerMuestrasPorUsuarioAsync(string usuarioId)
    {
        return await _context.Muestras
            .Where(m => m.IdUsuarioSolicitante == usuarioId)
            .Include(m => m.Tpmst)
            .Include(m => m.EstadoActualNavigation)
            .Select(m => new MuestraDto
            {
                MstCodigo = m.MstCodigo,
                TpmstId = m.TpmstId,
                Nombre = m.Nombre,
                Origen = m.Origen,
                CondicionesAlmacenamiento = m.CondicionesAlmacenamiento,
                CondicionesTransporte = m.CondicionesTransporte,
                EstadoActual = m.EstadoActual,
                FechaRecepcion = m.FechaRecepcion,
                FechaSalidaEstimada = m.FechaSalidaEstimada
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<MuestraDto>> ObtenerMuestrasPorAnalistaAsync(string analistaId)
    {
        // Validate analyst ID if provided
        if (!string.IsNullOrEmpty(analistaId))
        {
            var analistaExists = await _context.Users.AnyAsync(u => u.Id == analistaId);
            if (!analistaExists)
            {
                throw new Exception($"The specified analyst ID {analistaId} does not exist.");
            }
        }

        var muestrasIds = _context.BitacoraMuestras
            .Where(b => b.IdAnalista == analistaId)
            .Select(b => b.IdMuestra)
            .Distinct()
            .ToList();

        return await _context.Muestras
            .Where(m => muestrasIds.Contains(m.MstCodigo))
            .Include(m => m.Tpmst)
            .Include(m => m.EstadoActualNavigation)
            .Select(m => new MuestraDto
            {
                MstCodigo = m.MstCodigo,
                TpmstId = m.TpmstId,
                Nombre = m.Nombre,
                Origen = m.Origen,
                CondicionesAlmacenamiento = m.CondicionesAlmacenamiento,
                CondicionesTransporte = m.CondicionesTransporte,
                EstadoActual = m.EstadoActual,
                FechaRecepcion = m.FechaRecepcion,
                FechaSalidaEstimada = m.FechaSalidaEstimada
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditoriaDto>> ObtenerAuditoriasAsync()
    {
        return await _context.Auditoria
            //.Where(m => m.IdUsuario == usuarioId)
            //.Include(m => m.Tpmst)
            //.Include(m => m.EstadoActualNavigation)
            .Select(m => new AuditoriaDto
            {
                idAuditoria = m.IdAuditoria.ToString(),
                idUsuario = m.IdUsuario,
                Accion = m.Accion,
                fechaAcción = m.FechaAccion,
                descripcion = m.Descripcion ?? "",
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditoriaDto>> ObtenerAuditoriaPorIdAsync(string usuarioId)
    {
        return await _context.Auditoria
            .Where(m => m.IdUsuario == usuarioId)
            //.Include(m => m.Tpmst)
            //.Include(m => m.EstadoActualNavigation)
            .Select(m => new AuditoriaDto
            {
                idAuditoria = m.IdAuditoria.ToString(),
                idUsuario = m.IdUsuario,
                Accion = m.Accion,
                fechaAcción = m.FechaAccion,
                descripcion = m.Descripcion ?? "",
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<HistorialDto>> ObtenerHistorialTrazabilidadAsync()
    {
        return await _context.HistorialTrazabilidads
            //.Where(m => m.IdUsuario == usuarioId)
            //.Include(m => m.Tpmst)
            //.Include(m => m.EstadoActualNavigation)
            .Select(h => new HistorialDto
            {
                IdBitacora = h.IdHistorial,
                IdMuestra = h.IdMuestra,
                IdAnalista = h.IdUsuario,
                Estado = h.Estado,
                FechaCambio = h.FechaCambio,
                Observaciones = h.Observaciones ?? "",
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<HistorialDto>> ObtenerHistorialTrazabilidadPorIdAsync(string AnalistaId)
    {
        return await _context.HistorialTrazabilidads
            .Where(h => h.IdUsuario == AnalistaId)
            .Select(h => new HistorialDto
            {
                IdBitacora = h.IdHistorial,
                IdMuestra = h.IdMuestra,
                IdAnalista = h.IdUsuario,
                Estado = h.Estado,
                FechaCambio = h.FechaCambio,
                Observaciones = h.Observaciones ?? "",
            })
            .ToListAsync();
    }

    public async Task<MuestraDto?> CrearMuestraAsync(CreateMuestraDto nuevaMuestra, string usuarioId)
    {
        var result = await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_crear_muestra @p_MST_CODIGO = {0}, @p_TPMST_ID = {1}, @p_Nombre = {2}, @p_Fecha_recepcion = {3}, @p_origen = {4}, @p_Fecha_Salida_Estimada = {5}, @p_Cond_alm = {6}, @p_Cond_trans = {7}, @p_id_solicitante = {8}",
            nuevaMuestra.MstCodigo,
            nuevaMuestra.TpmstId,
            nuevaMuestra.Nombre,
            DateTime.Now,
            nuevaMuestra.Origen,
            null,
            nuevaMuestra.CondicionesAlmacenamiento,
            nuevaMuestra.CondicionesTransporte,
            usuarioId
        );

        // map result from stored procedure to Muestra object
        if (result <= 0) // Check if the stored procedure executed successfully
        {
            return null; // Return null if the stored procedure execution was not successful
        }

        // Retrieve the newly created sample using its ID
        var muestra = await _context.Muestras
            .Include(m => m.Tpmst)
            .Include(m => m.EstadoActualNavigation)
            .FirstOrDefaultAsync(m => m.MstCodigo == nuevaMuestra.MstCodigo);

        MuestraDto muestraDto = new MuestraDto
        {
            MstCodigo = muestra!.MstCodigo,
            TpmstId = muestra.TpmstId,
            Nombre = muestra.Nombre,
            Origen = muestra.Origen,
            CondicionesAlmacenamiento = muestra.CondicionesAlmacenamiento,
            CondicionesTransporte = muestra.CondicionesTransporte,
            EstadoActual = muestra.EstadoActual,
        };

        return muestraDto;
    }

    public async Task<BitacoraMuestra?> AsignarAnalistaAsync(AsignarAnalistaEnMuestraDto asignarAnalistaEnMuestraDto )
    {
        var result = await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_asignar_analista @p_MST_CODIGO = {0}, @p_id_analista = {1}, @p_observaciones = {2}",
            asignarAnalistaEnMuestraDto.MstCodigo,
            asignarAnalistaEnMuestraDto.IdAnalista,
            asignarAnalistaEnMuestraDto.Observaciones
            );

        if (result <= 0) return null; // Check if the stored procedure executed successfully

        // Retrieve the newly created bitacora entry using its composite key (MstCodigo, FechaCambio)

        var bitacoraEntry = await _context.BitacoraMuestras
            .Where(b => b.IdMuestra == asignarAnalistaEnMuestraDto.MstCodigo)
            .OrderByDescending(b => b.FechaAsignacion) // Order by FechaCambio descending to get the latest entry
            .FirstOrDefaultAsync();

        return bitacoraEntry;
    }

    public async Task<BitacoraMuestra?> CambiarEstadoAsync(AsignarEstadoMuestraDto asignarEstadoMuestraDto, string idUsuario)
    {
        var result = await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_cambiar_estado @p_MST_CODIGO = {0}, @p_nuevo_estado = {1}, @p_id_usuario = {2}, @p_observaciones = {3}",
            asignarEstadoMuestraDto.MstCodigo,
            asignarEstadoMuestraDto.estadoMuestra,
            idUsuario,
            asignarEstadoMuestraDto.Observaciones
            );

        if (result <= 0) return null; // Check if the stored procedure executed successfully

        // Retrieve the newly created bitacora entry using its composite key (MstCodigo, FechaCambio)

        var bitacoraEntry = await _context.BitacoraMuestras
            .Where(b => b.IdMuestra == asignarEstadoMuestraDto.MstCodigo)
            .OrderByDescending(b => b.FechaAsignacion) // Order by FechaCambio descending to get the latest entry
            .FirstOrDefaultAsync();

        return bitacoraEntry;
    }

    public async Task<MuestraDto?> ModificarMuestraAsync(Muestra muestraActualizada)
    {
        var existingMuestra = await _context.Muestras.FindAsync(muestraActualizada.MstCodigo);
        if (existingMuestra == null)
        {
            return null; // Muestra no encontrada
        }

        // Actualizar los campos necesarios
        existingMuestra.Nombre = muestraActualizada.Nombre;
        existingMuestra.TpmstId = muestraActualizada.TpmstId;
        existingMuestra.Origen = muestraActualizada.Origen;
        existingMuestra.FechaSalidaEstimada = muestraActualizada.FechaSalidaEstimada;
        existingMuestra.CondicionesAlmacenamiento = muestraActualizada.CondicionesAlmacenamiento;
        existingMuestra.CondicionesTransporte = muestraActualizada.CondicionesTransporte;
        existingMuestra.EstadoActual = muestraActualizada.EstadoActual;

        await _context.SaveChangesAsync();

        var muestraDto = new MuestraDto
        {
            MstCodigo = existingMuestra.MstCodigo,
            TpmstId = existingMuestra.TpmstId,
            Nombre = existingMuestra.Nombre,
            Origen = existingMuestra.Origen,
            CondicionesAlmacenamiento = existingMuestra.CondicionesAlmacenamiento,
            CondicionesTransporte = existingMuestra.CondicionesTransporte,
            EstadoActual = existingMuestra.EstadoActual,
        };

        return muestraDto;
    }

    public async Task<Muestra?> ObtenerMuestraPorIdAsync(string id)
    {
        return await _context.Muestras
            .Include<Muestra, object>(m => m.Tpmst)
            .Include<Muestra, object>(m => m.EstadoActualNavigation)
            .FirstOrDefaultAsync(m => m.MstCodigo == id);
    }

    public async Task<EvaluarMuestraResponseDto?> EvaluarMuestraAsync(EvaluarMuestraDto evaluarDto, string evaluadorId)
    {
        using var tx = await _context.Database.BeginTransactionAsync();
        try
        {
            var estadoEsperaId      = (byte)3;
            var estadoCertificadaId = (byte)5;
            var estadoEnAnalisisId  = (byte)2;

            // 1) Validaciones
            var muestra = await _context.Muestras
                .Include(m => m.Tpmst)
                .FirstOrDefaultAsync(m => m.MstCodigo == evaluarDto.MuestraId);

            if (muestra is null) return null;
            if (muestra.EstadoActual != estadoEsperaId)
                throw new Exception("La muestra debe estar en 'En espera' para ser evaluada.");

            DocumentoDto? documentoCreado = null;

            if (evaluarDto.Aprobado)
            {
                // 2) (Opcional) Persistir “los resultados validados” (los que van al certificado)
                //    Si NO quieres tocar la tabla, comenta este bloque:
                if (evaluarDto.Resultados?.Any() == true)
                {
                    foreach (var r in evaluarDto.Resultados)
                    {
                        _context.ResultadoPruebas.Add(new ResultadoPrueba
                        {
                            IdMuestra      = evaluarDto.MuestraId,
                            IdParametro    = r.IdParametro,
                            ValorObtenido  = r.ValorObtenido,
                            Unidad         = r.Unidad, // puede ser null; lo rellena la norma si lo prefieres
                            EstadoValidacion = "VALIDADO",
                            ValidadoPor    = evaluadorId,
                            // IdPrueba: si quieres asociarlo, colócalo aquí
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                // 3) Generar Documento (Excel -> PDF) con EXACTAMENTE esos resultados
                var version = await _context.Documentos
                    .Where(d => d.IdMuestra == evaluarDto.MuestraId)
                    .CountAsync() + 1;

                var outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certificados");
                var pdfPath = await _documentBuilder.CrearCertificadoAsync(
                    _context, muestra,
                    evaluarDto.Resultados ?? Enumerable.Empty<ResultadoParaDocumentoDto>(),
                    version,
                    outputDir);

                // 4) Persistir documento (ruta y PDF en binario para consulta web)
                var bytes = await File.ReadAllBytesAsync(pdfPath);
                var nuevoDocumento = new Documento
                {
                    IdMuestra           = evaluarDto.MuestraId,
                    IdTipoDoc           = 1, // Certificado
                    IdEstadoDocumento   = 2, // Aprobado
                    Version             = version,
                    FechaCreacion       = DateTime.Now,
                    RutaArchivo         = pdfPath,
                    DocPdf              = bytes
                };
                _context.Documentos.Add(nuevoDocumento);

                muestra.EstadoActual = estadoCertificadaId;
                await _context.SaveChangesAsync();

                documentoCreado = new DocumentoDto
                {
                    IdDocumento        = nuevoDocumento.IdDocumento,
                    IdMuestra          = nuevoDocumento.IdMuestra,
                    IdTipoDoc          = nuevoDocumento.IdTipoDoc,
                    IdEstadoDocumento  = nuevoDocumento.IdEstadoDocumento,
                    Version            = nuevoDocumento.Version,
                    FechaCreacion      = nuevoDocumento.FechaCreacion,
                    RutaArchivo        = nuevoDocumento.RutaArchivo,
                    DocPdf             = nuevoDocumento.DocPdf
                };
            }
            else
            {
                // Rechazada -> vuelve a "En análisis"
                muestra.EstadoActual = estadoEnAnalisisId;

                _context.BitacoraMuestras.Add(new BitacoraMuestra
                {
                    IdMuestra = evaluarDto.MuestraId,
                    IdAnalista = evaluadorId,
                    FechaAsignacion = DateTime.Now,
                    Observaciones = evaluarDto.Observaciones ?? "Muestra rechazada por el evaluador"
                });
                await _context.SaveChangesAsync();
            }

            // Auditoría
            _context.Auditoria.Add(new Auditorium
            {
                IdUsuario   = evaluadorId,
                Accion      = evaluarDto.Aprobado ? "EVALUAR_MUESTRA_APROBADA" : "EVALUAR_MUESTRA_RECHAZADA",
                Descripcion = $"MST={evaluarDto.MuestraId}, Resultado={evaluarDto.Aprobado}, Obs={evaluarDto.Observaciones}",
                FechaAccion = DateTime.Now
            });
            await _context.SaveChangesAsync();

            await tx.CommitAsync();

            // Respuesta
            var muestraDto = new MuestraDto
            {
                MstCodigo = muestra.MstCodigo,
                TpmstId = muestra.TpmstId,
                Nombre = muestra.Nombre,
                Origen = muestra.Origen,
                CondicionesAlmacenamiento = muestra.CondicionesAlmacenamiento,
                CondicionesTransporte = muestra.CondicionesTransporte,
                EstadoActual = muestra.EstadoActual,
                FechaRecepcion = muestra.FechaRecepcion,
                FechaSalidaEstimada = muestra.FechaSalidaEstimada
            };

            return new EvaluarMuestraResponseDto
            {
                Muestra   = muestraDto,
                Documento = documentoCreado
            };
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }
    
    public async Task<EvaluarPruebaResponseDto?> EvaluarPruebaAsync(EvaluarPruebaDto dto, string evaluadorId)
{
    using var tx = await _context.Database.BeginTransactionAsync();
    try
    {
        const byte EN_ESPERA = 3;
        const byte CERTIFICADA = 5;
        const byte EN_ANALISIS = 2;

        // 1) Traer la Prueba con todo
        var prueba = await _context.Pruebas
            .Include(p => p.IdMuestraNavigation)
            .ThenInclude(m => m.Tpmst)
            .Include(p => p.ResultadoPruebas)
            .FirstOrDefaultAsync(p => p.IdPrueba == dto.IdPrueba);

        if (prueba is null) return null;

        var muestra = prueba.IdMuestraNavigation!;
        if (muestra.EstadoActual != EN_ESPERA)
            throw new Exception("La muestra debe estar en 'En espera' para ser evaluada.");

        if (prueba.ResultadoPruebas is null || !prueba.ResultadoPruebas.Any())
            throw new Exception("La prueba no contiene resultados registrados.");

        DocumentoDto? documentoCreado = null;

        if (dto.Aprobado)
        {
            // 2) Validar resultados (opcional pero recomendado)
            foreach (var r in prueba.ResultadoPruebas)
            {
                r.EstadoValidacion = "VALIDADO";
                r.ValidadoPor = evaluadorId;

                // (Opcional) Calcula CumpleNorma aquí:
                var norma = await _context.ParametroNormas
                    .AsNoTracking()
                    .FirstOrDefaultAsync(n => n.IdParametro == r.IdParametro && n.TpmstId == muestra.TpmstId);

                if (norma != null)
                {
                    r.CumpleNorma = Cumple(norma, r.ValorObtenido.Value);
                    if (string.IsNullOrEmpty(r.Unidad) && !string.IsNullOrEmpty(norma.Unidad))
                        r.Unidad = norma.Unidad;
                }
            }
            await _context.SaveChangesAsync();

            // 3) Generar Documento desde la Prueba (Excel -> PDF)
            var version = await _context.Documentos
                .Where(d => d.IdMuestra == muestra.MstCodigo)
                .CountAsync() + 1;

            var outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certificados");
            var pdfPath = await _documentBuilder.CrearCertificadoAsync(
                _context,
                muestra,
                prueba.ResultadoPruebas,
                version,
                outputDir);

            var bytes = await File.ReadAllBytesAsync(pdfPath);
            var nuevoDocumento = new Documento
            {
                IdMuestra         = muestra.MstCodigo,
                IdTipoDoc         = 1, // Certificado
                IdEstadoDocumento = 2, // Aprobado
                Version           = version,
                FechaCreacion     = DateTime.Now,
                RutaArchivo       = pdfPath,
                DocPdf            = bytes
            };
            _context.Documentos.Add(nuevoDocumento);

            muestra.EstadoActual = CERTIFICADA;
            await _context.SaveChangesAsync();

            documentoCreado = new DocumentoDto
            {
                IdDocumento       = nuevoDocumento.IdDocumento,
                IdMuestra         = nuevoDocumento.IdMuestra,
                IdTipoDoc         = nuevoDocumento.IdTipoDoc,
                IdEstadoDocumento = nuevoDocumento.IdEstadoDocumento,
                Version           = nuevoDocumento.Version,
                FechaCreacion     = nuevoDocumento.FechaCreacion,
                RutaArchivo       = nuevoDocumento.RutaArchivo,
                DocPdf            = nuevoDocumento.DocPdf
            };
        }
        else
        {
            // Rechazada: vuelve a "En análisis" y registra observación
            muestra.EstadoActual = EN_ANALISIS;

            _context.BitacoraMuestras.Add(new BitacoraMuestra
            {
                IdMuestra = muestra.MstCodigo,
                IdAnalista = evaluadorId, // quien registra la observación (evaluador)
                FechaAsignacion = DateTime.Now,
                Observaciones = dto.Observaciones ?? "Prueba rechazada por el evaluador"
            });
            await _context.SaveChangesAsync();
        }

        // Auditoría
        _context.Auditoria.Add(new Auditorium
        {
            IdUsuario   = evaluadorId,
            Accion      = dto.Aprobado ? "EVALUAR_PRUEBA_APROBADA" : "EVALUAR_PRUEBA_RECHAZADA",
            Descripcion = $"PRB={dto.IdPrueba}, MST={muestra.MstCodigo}, Resultado={dto.Aprobado}, Obs={dto.Observaciones}",
            FechaAccion = DateTime.Now
        });
        await _context.SaveChangesAsync();

        await tx.CommitAsync();

        return new EvaluarPruebaResponseDto
        {
            IdPrueba = dto.IdPrueba,
            Muestra = new MuestraDto
            {
                MstCodigo = muestra.MstCodigo,
                TpmstId = muestra.TpmstId,
                Nombre = muestra.Nombre,
                Origen = muestra.Origen,
                CondicionesAlmacenamiento = muestra.CondicionesAlmacenamiento,
                CondicionesTransporte = muestra.CondicionesTransporte,
                EstadoActual = muestra.EstadoActual,
                FechaRecepcion = muestra.FechaRecepcion,
                FechaSalidaEstimada = muestra.FechaSalidaEstimada
            },
            Documento = documentoCreado
        };
    }
    catch
    {
        await tx.RollbackAsync();
        throw;
    }
}

// Helper de norma
private static bool Cumple(ParametroNorma n, decimal valor)
{
    // Caso "Presencia/25g" => en tus datos de semillas lo marcas con min=0 y max=0
    if (n.ValorMin == 0 && n.ValorMax == 0) return valor == 0;

    if (n.ValorMin.HasValue && valor < n.ValorMin.Value) return false;
    if (n.ValorMax.HasValue && valor > n.ValorMax.Value) return false;
    return true;
}

}
