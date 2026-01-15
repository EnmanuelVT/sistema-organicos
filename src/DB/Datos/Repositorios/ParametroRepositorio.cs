using ENTIDAD.DTOs.Parametros;
using Microsoft.EntityFrameworkCore;

namespace DB.Datos.Repositorios;

public class ParametroRepositorio
{
    private readonly MasterDbContext _context;
    
    public ParametroRepositorio(MasterDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ParametroDto>> ObtenerParametrosPorTipoMuestraAsync(int tpmstId)
    {
        return await _context.ParametroNormas
            .Where(p => p.TpmstId == tpmstId)
            .Select(p => new ParametroDto
            {
                IdParametro = p.IdParametro,
                NombreParametro = p.NombreParametro,
                ValorMin = p.ValorMin,
                ValorMax = p.ValorMax,
                Unidad = p.Unidad,
                TpmstId = p.TpmstId,
                TipoPruebaId = p.TipoPruebaId
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<ParametroDto>> ObtenerParametrosPorPruebaAsync(int idPrueba)
    {
        var pruebaInfo = await _context.Pruebas
            .Where(p => p.IdPrueba == idPrueba)
            .Select(p => new
            {
                p.TipoPruebaId,
                TpmstId = p.IdMuestraNavigation.TpmstId
            })
            .FirstOrDefaultAsync();

        if (pruebaInfo == null)
        {
            return Array.Empty<ParametroDto>();
        }

        var query = _context.ParametroNormas
            .Where(p => p.TpmstId == pruebaInfo.TpmstId);

        if (pruebaInfo.TipoPruebaId != null)
        {
            var tipoPruebaId = pruebaInfo.TipoPruebaId;
            query = query.Where(p => p.TipoPruebaId == tipoPruebaId);
        }

        return await query
            .Select(p => new ParametroDto
            {
                IdParametro = p.IdParametro,
                NombreParametro = p.NombreParametro,
                ValorMin = p.ValorMin,
                ValorMax = p.ValorMax,
                Unidad = p.Unidad,
                TpmstId = p.TpmstId,
                TipoPruebaId = p.TipoPruebaId
            })
            .ToListAsync();
    }
    
    public async Task<ParametroDto?> AgregarParametroATipoMuestra(CreateParametroDto createParametroATipoDto, string idUsuario)
    {
        var result = await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_agregar_parametro_a_tipo_muestra @p_nombre_parametro = {0}, @p_valor_min = {1}, @p_valor_max = {2}, @p_unidad = {3}, @p_tpmst_id = {4}, @p_tipo_prueba_id = {5}, @p_id_usuario = {6}",
            createParametroATipoDto.NombreParametro,
            createParametroATipoDto.ValorMin,
            createParametroATipoDto.ValorMax,
            createParametroATipoDto.Unidad,
            createParametroATipoDto.TpmstId,
            createParametroATipoDto.TipoPruebaId,
            idUsuario
        );
        
        if (result <= 0)
        {
            return null;
        }
        
        return await _context.ParametroNormas
            .Where(p => p.NombreParametro == createParametroATipoDto.NombreParametro
                        && p.TpmstId == createParametroATipoDto.TpmstId
                        && p.TipoPruebaId == createParametroATipoDto.TipoPruebaId)
            .Select(p => new ParametroDto
            {
                IdParametro = p.IdParametro,
                NombreParametro = p.NombreParametro,
                ValorMin = p.ValorMin,
                ValorMax = p.ValorMax,
                Unidad = p.Unidad,
                TpmstId = p.TpmstId,
                TipoPruebaId = p.TipoPruebaId
            })
            .FirstOrDefaultAsync();
    }
}