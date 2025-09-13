using System.Data;
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
using Microsoft.Data.SqlClient;

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
        var pTpmstId        = new SqlParameter("@p_TPMST_ID", SqlDbType.TinyInt) { Value = nuevaMuestra.TpmstId };
        var pNombre         = new SqlParameter("@p_Nombre", SqlDbType.VarChar, 120) { Value = (object?)nuevaMuestra.Nombre ?? DBNull.Value };
        var pFechaRecep     = new SqlParameter("@p_Fecha_recepcion", SqlDbType.DateTime) { Value = DateTime.Now };
        var pOrigen         = new SqlParameter("@p_origen", SqlDbType.VarChar, 200) { Value = (object?)nuevaMuestra.Origen ?? DBNull.Value };
        var pFechaSalidaEst = new SqlParameter("@p_Fecha_Salida_Estimada", SqlDbType.DateTime) { Value = DBNull.Value };
        var pCondAlm        = new SqlParameter("@p_Cond_alm", SqlDbType.VarChar, 200) { Value = (object?)nuevaMuestra.CondicionesAlmacenamiento ?? DBNull.Value };
        var pCondTrans      = new SqlParameter("@p_Cond_trans", SqlDbType.VarChar, 200) { Value = (object?)nuevaMuestra.CondicionesTransporte ?? DBNull.Value };
        var pSolicitante    = new SqlParameter("@p_id_solicitante", SqlDbType.VarChar, 450) { Value = usuarioId };

        var pOutCodigo = new SqlParameter("@o_MST_CODIGO", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };

        var sql = @"
            EXEC dbo.sp_crear_muestra
                 @p_TPMST_ID,
                 @p_Nombre,
                 @p_Fecha_recepcion,
                 @p_origen,
                 @p_Fecha_Salida_Estimada,
                 @p_Cond_alm,
                 @p_Cond_trans,
                 @p_id_solicitante,
                 @o_MST_CODIGO OUTPUT";

        // Nota: ExecuteSqlRawAsync puede devolver -1 y AÚN ASÍ ser éxito.
        await _context.Database.ExecuteSqlRawAsync(
            sql,
            pTpmstId, pNombre, pFechaRecep, pOrigen, pFechaSalidaEst, pCondAlm, pCondTrans, pSolicitante, pOutCodigo
        );

        var mstCodigo = pOutCodigo.Value as string;
        if (string.IsNullOrWhiteSpace(mstCodigo)) return null;

        var muestra = await _context.Muestras
            .Include(m => m.Tpmst)
            .Include(m => m.EstadoActualNavigation)
            .FirstOrDefaultAsync(m => m.MstCodigo == mstCodigo);

        if (muestra is null) return null;

        return new MuestraDto
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
    
    public async Task<DocumentoDto?> GenerarDocumentoPreliminarAsync(int idPrueba, string analistaId, string? observaciones = null)
    {
        // Estados del negocio (solo para claridad local; no mutamos la muestra aquí)
        const byte EN_ANALISIS = 2;
        const int  TIPO_DOC_INFORME = 2;
        const int  ESTADO_DOC_PRELIMINAR = 3; // de la semilla

        // 1) Traer la prueba con su muestra y resultados
        var prueba = await _context.Pruebas
            .Include(p => p.IdMuestraNavigation)
            .ThenInclude(m => m.Tpmst)
            .Include(p => p.ResultadoPruebas)
            .FirstOrDefaultAsync(p => p.IdPrueba == idPrueba);

        if (prueba is null)
            throw new Exception($"No existe la prueba {idPrueba}.");

        var muestra = prueba.IdMuestraNavigation!;
        if (muestra is null)
            throw new Exception($"La prueba {idPrueba} no tiene muestra asociada.");

        // Recomendado, pero opcional: permitir informe preliminar solo si la muestra está "En análisis"
        if (muestra.EstadoActual != EN_ANALISIS)
        {
            // Si deseas permitir en otros estados, comenta este guard.
            throw new Exception("Solo se pueden generar informes preliminares cuando la muestra está 'En análisis'.");
        }

        if (prueba.ResultadoPruebas is null || !prueba.ResultadoPruebas.Any())
            throw new Exception("La prueba no contiene resultados registrados.");

        // 2) Calcular versión de informe preliminar para esta muestra
        var version = await _context.Documentos
            .Where(d => d.IdMuestra == muestra.MstCodigo && d.IdTipoDoc == TIPO_DOC_INFORME)
            .CountAsync() + 1;

        // 3) Generar PDF usando el DocumentBuilder (sin stored procedures)
        var outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "informes");
        Directory.CreateDirectory(outputDir);

        // Nota: reutilizamos el método que construye el Excel->PDF. El nombre del método no afecta el contenido.
        var pdfPath = await _documentBuilder.CrearCertificadoAsync(
            _context,
            muestra,
            prueba.ResultadoPruebas, // <- IEnumerable<ResultadoPrueba>
            version,
            outputDir
        );

        var bytes = await File.ReadAllBytesAsync(pdfPath);

        // 4) Persistir Documento como "Preliminar"
        var nuevoDocumento = new Documento
        {
            IdMuestra         = muestra.MstCodigo,
            IdTipoDoc         = TIPO_DOC_INFORME,       // Informe (no certificado)
            IdEstadoDocumento = ESTADO_DOC_PRELIMINAR,  // Preliminar
            Version           = version,
            FechaCreacion     = DateTime.Now,
            RutaArchivo       = pdfPath,
            DocPdf            = bytes
        };
        _context.Documentos.Add(nuevoDocumento);

        // 5) Auditoría
        _context.Auditoria.Add(new Auditorium
        {
            IdUsuario   = analistaId,
            Accion      = "GENERAR_DOCUMENTO_PRELIMINAR",
            Descripcion = $"PRB={idPrueba}, MST={muestra.MstCodigo}, v{version}, Obs={(observaciones ?? "-")}",
            FechaAccion = DateTime.Now
        });

        await _context.SaveChangesAsync();

        // 6) DTO de salida
        return new DocumentoDto
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
