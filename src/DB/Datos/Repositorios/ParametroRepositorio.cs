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
                TpmstId = p.TpmstId
            })
            .ToListAsync();
    }
    
    public async Task<IEnumerable<ParametroDto>> ObtenerParametrosPorPruebaAsync(int idPrueba)
    {
        return await _context.ParametroNormas
            .Where(p => p.IdPrueba == idPrueba)
            .Select(p => new ParametroDto
            {
                IdParametro = p.IdParametro,
                IdPrueba = p.IdPrueba,
                NombreParametro = p.NombreParametro,
                ValorMin = p.ValorMin,
                ValorMax = p.ValorMax,
                Unidad = p.Unidad
            })
            .ToListAsync();
    }

    public async Task<ParametroDto?> AgregarParametroATipoMuestra(CreateParametroATipoDto createParametroATipoDto, string idUsuario)
    {
        var result = await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_agregar_parametro_a_tipo_muestra @p_nombre_parametro = {0}, @p_valor_min = {1}, @p_valor_max = {2}, @p_unidad = {3}, @p_tpmst_id = {4}, @p_id_usuario = {5}",
            createParametroATipoDto.NombreParametro,
            createParametroATipoDto.ValorMin,
            createParametroATipoDto.ValorMax,
            createParametroATipoDto.Unidad,
            createParametroATipoDto.TpmstId,
            idUsuario
        );
        
        if (result <= 0)
        {
            return null;
        }
        
        return await _context.ParametroNormas
            .Where(p => p.NombreParametro == createParametroATipoDto.NombreParametro && p.TpmstId == createParametroATipoDto.TpmstId)
            .Select(p => new ParametroDto
            {
                IdParametro = p.IdParametro,
                NombreParametro = p.NombreParametro,
                ValorMin = p.ValorMin,
                ValorMax = p.ValorMax,
                Unidad = p.Unidad,
                TpmstId = p.TpmstId
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ParametroDto?> AgregarParametroAPrueba(CreateParametroAPruebaDto createParametroAPruebaDto,
        string idUsuario)
    {
        var result = await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_agregar_parametro_a_prueba @p_id_prueba = {0}, @p_nombre_parametro = {1}, @p_valor_min = {2}, @p_valor_max = {3}, @p_unidad = {4}, @p_id_usuario = {5}",
            createParametroAPruebaDto.IdPrueba,
            createParametroAPruebaDto.NombreParametro,
            createParametroAPruebaDto.ValorMin,
            createParametroAPruebaDto.ValorMax,
            createParametroAPruebaDto.Unidad,
            idUsuario
        );
        
        if (result <= 0)
        {
            return null;
        }
        
        return await _context.ParametroNormas
            .Where(p => p.NombreParametro == createParametroAPruebaDto.NombreParametro && p.IdPrueba == createParametroAPruebaDto.IdPrueba)
            .Select(p => new ParametroDto
            {
                IdParametro = p.IdParametro,
                IdPrueba = p.IdPrueba,
                NombreParametro = p.NombreParametro,
                ValorMin = p.ValorMin,
                ValorMax = p.ValorMax,
                Unidad = p.Unidad
            })
            .FirstOrDefaultAsync();
    }
}