using ENTIDAD.DTOs.Muestras;
using ENTIDAD.DTOs.Documentos;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DB.Datos.Repositorios;

public class MuestraRepositorio
{
    private readonly MasterDbContext _context;

    public MuestraRepositorio(MasterDbContext context)
    {
        _context = context;
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
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Verificar que la muestra existe
            var muestra = await _context.Muestras
                .Include(m => m.Tpmst)
                .Include(m => m.EstadoActualNavigation)
                .FirstOrDefaultAsync(m => m.MstCodigo == evaluarDto.MuestraId);

            if (muestra == null)
            {
                return null;
            }

            // Verificar que la muestra tiene resultados de pruebas (estado En espera = 3)
            if (muestra.EstadoActual != 3) // Estado "En espera"
            {
                throw new Exception("La muestra debe estar en estado 'En espera' para poder ser evaluada por el evaluador");
            }

            DocumentoDto? documentoCreado = null;

            if (evaluarDto.Aprobado)
            {
                // APROBADO: Cambiar estado a Certificada (5) y crear documento
                muestra.EstadoActual = 5; // Estado "Certificada"

                // Crear documento certificado
                var siguienteVersion = await _context.Documentos
                    .Where(d => d.IdMuestra == evaluarDto.MuestraId)
                    .CountAsync() + 1;

                var nuevoDocumento = new Documento
                {
                    IdMuestra = evaluarDto.MuestraId,
                    IdTipoDoc = 1, // Tipo "Certificado"
                    IdEstadoDocumento = 2, // Estado "Aprobado"
                    Version = siguienteVersion,
                    FechaCreacion = DateTime.Now,
                    RutaArchivo = $"certificados/{evaluarDto.MuestraId}_v{siguienteVersion}.pdf"
                };

                _context.Documentos.Add(nuevoDocumento);
                await _context.SaveChangesAsync();

                documentoCreado = new DocumentoDto
                {
                    IdDocumento = nuevoDocumento.IdDocumento,
                    IdMuestra = nuevoDocumento.IdMuestra,
                    IdTipoDoc = nuevoDocumento.IdTipoDoc,
                    IdEstadoDocumento = nuevoDocumento.IdEstadoDocumento,
                    Version = nuevoDocumento.Version,
                    FechaCreacion = nuevoDocumento.FechaCreacion,
                    RutaArchivo = nuevoDocumento.RutaArchivo,
                    DocPdf = nuevoDocumento.DocPdf
                };
            }
            else
            {
                // RECHAZADO: Cambiar estado a En analisis (2) 
                muestra.EstadoActual = 2; // Estado "En analisis"
                
                // Crear registro en BitacoraMuestra con las observaciones del evaluador
                var bitacora = new BitacoraMuestra
                {
                    IdMuestra = evaluarDto.MuestraId,
                    IdAnalista = evaluadorId, // El evaluador aparece como quien hace la observación
                    FechaAsignacion = DateTime.Now,
                    Observaciones = evaluarDto.Observaciones ?? "Muestra rechazada por el evaluador"
                };

                _context.BitacoraMuestras.Add(bitacora);
            }

            // Guardar cambios en la muestra
            await _context.SaveChangesAsync();

            // Crear registro de auditoría
            var auditoria = new Auditorium
            {
                IdUsuario = evaluadorId,
                Accion = evaluarDto.Aprobado ? "EVALUAR_MUESTRA_APROBADA" : "EVALUAR_MUESTRA_RECHAZADA",
                Descripcion = $"MST={evaluarDto.MuestraId}, Resultado={evaluarDto.Aprobado}, Obs={evaluarDto.Observaciones}",
                FechaAccion = DateTime.Now
            };

            _context.Auditoria.Add(auditoria);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            // Preparar respuesta con la muestra actualizada
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
                Muestra = muestraDto,
                Documento = documentoCreado
            };
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}