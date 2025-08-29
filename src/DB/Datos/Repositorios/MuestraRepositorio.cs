using ENTIDAD.DTOs.Muestras;
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
                IdAnalista = m.IdAnalista
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<MuestraDto>> ObtenerMuestrasPorUsuarioAsync(string usuarioId)
    {
        return await _context.Muestras
            .Where(m => m.IdUsuarioSolicitante == usuarioId || m.IdAnalista == usuarioId)
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
                IdAnalista = m.IdAnalista
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

        return await _context.Muestras
            .Where(m => m.IdAnalista == analistaId)
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
                IdAnalista = m.IdAnalista
            })
            .ToListAsync();
    }

    public async Task<MuestraDto?> CrearMuestraAsync(CreateMuestraDto nuevaMuestra, string usuarioId)
    {
        var result = await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_crear_muestra @p_MST_CODIGO = {0}, @p_TPMST_ID = {1}, @p_Nombre = {2}, @p_Fecha_recepcion = {3}, @p_origen = {4}, @p_Fecha_Salida_Estimada = {5}, @p_Cond_alm = {6}, @p_Cond_trans = {7}, @p_id_solicitante = {8}, @p_id_responsable = {9}",
            nuevaMuestra.MstCodigo,
            nuevaMuestra.TpmstId,
            nuevaMuestra.Nombre,
            DateTime.Now,
            nuevaMuestra.Origen,
            null,
            nuevaMuestra.CondicionesAlmacenamiento,
            nuevaMuestra.CondicionesTransporte,
            usuarioId,// id del usuario solicitante
            null // id del analista se coloca despues
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

        // Validate analyst ID if provided
        if (!string.IsNullOrEmpty(muestraActualizada.IdAnalista))
        {
            var analistaExists = await _context.Users.AnyAsync(u => u.Id == muestraActualizada.IdAnalista);
            if (!analistaExists)
            {
                throw new Exception($"The specified analyst ID {muestraActualizada.IdAnalista} does not exist.");
            }
        }

        // Actualizar los campos necesarios
        existingMuestra.Nombre = muestraActualizada.Nombre;
        existingMuestra.TpmstId = muestraActualizada.TpmstId;
        existingMuestra.Origen = muestraActualizada.Origen;
        existingMuestra.FechaSalidaEstimada = muestraActualizada.FechaSalidaEstimada;
        existingMuestra.CondicionesAlmacenamiento = muestraActualizada.CondicionesAlmacenamiento;
        existingMuestra.CondicionesTransporte = muestraActualizada.CondicionesTransporte;
        existingMuestra.IdAnalista = muestraActualizada.IdAnalista;
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
            IdAnalista = existingMuestra.IdAnalista
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
}