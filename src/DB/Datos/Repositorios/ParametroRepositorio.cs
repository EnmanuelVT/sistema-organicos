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
    
    public async Task<ParametroDto?> AgregarParametroATipoMuestra(CreateParametroDto createParametroATipoDto, string idUsuario)
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
}