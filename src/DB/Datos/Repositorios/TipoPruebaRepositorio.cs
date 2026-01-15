using ENTIDAD.DTOs.TipoPruebas;
using Microsoft.EntityFrameworkCore;

namespace DB.Datos.Repositorios;

public class TipoPruebaRepositorio
{
    private readonly MasterDbContext _context;

    public TipoPruebaRepositorio(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TipoPruebaDto>> ObtenerTiposAsync()
    {
        return await _context.TipoPruebas
            .OrderBy(t => t.Nombre)
            .Select(t => new TipoPruebaDto
            {
                IdTipoPrueba = t.IdTipoPrueba,
                Codigo = t.Codigo,
                Nombre = t.Nombre
            })
            .ToListAsync();
    }
}
