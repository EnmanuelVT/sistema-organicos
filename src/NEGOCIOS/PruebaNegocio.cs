using DB.Datos.Repositorios;
using ENTIDAD.DTOs.Pruebas;

namespace NEGOCIOS;

public class PruebaNegocio
{
    private readonly PruebaRepositorio _repositorio;
    public PruebaNegocio(PruebaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }
    
    public async Task<IEnumerable<PruebaDto?>> ObtenerPruebasPorMuestraAsync(string idMuestra)
    {
        return await _repositorio.ObtenerPruebasPorMuestraAsync(idMuestra);
    }
    
    public async Task<PruebaDto?> CrearPruebaAsync(CreatePruebaDto createPruebaDto, string idUsuario)
    {
        return await _repositorio.CrearPruebaAsync(createPruebaDto, idUsuario);
    }
}