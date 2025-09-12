using ENTIDAD.DTOs.Pruebas;
using Microsoft.EntityFrameworkCore;

namespace DB.Datos.Repositorios;

public class PruebaRepositorio
{
    private readonly MasterDbContext _context;
    
    public PruebaRepositorio(MasterDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<PruebaDto?>> ObtenerPruebasPorMuestraAsync(string idMuestra)
    {
        return await _context.Pruebas
            .Where(p => p.IdMuestra == idMuestra)
            .Select(p => new PruebaDto
            {
                IdPrueba = p.IdPrueba,
                IdMuestra = p.IdMuestra,
                NombrePrueba = p.NombrePrueba,
            })
            .ToListAsync();
    }
    
    public async Task<PruebaDto?> CrearPruebaAsync(CreatePruebaDto createPruebaDto, string idUsuario)
    {
        var result = await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_crear_prueba @p_nombre_prueba = {0}, @p_id_muestra = {1}, @p_id_usuario = {2}",
            createPruebaDto.NombrePrueba,
            createPruebaDto.IdMuestra,
            idUsuario
        );

        if (result <= 0)
        {
            return null;
        }
        
        return await _context.Pruebas
            .Where(p => p.IdMuestra == createPruebaDto.IdMuestra)
            .Select(p => new PruebaDto
            {
                IdPrueba = p.IdPrueba,
                IdMuestra = p.IdMuestra,
                NombrePrueba = p.NombrePrueba,
            })
            .FirstOrDefaultAsync();
    }
}