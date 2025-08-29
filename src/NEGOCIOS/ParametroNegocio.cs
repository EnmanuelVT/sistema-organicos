using DB.Datos.Repositorios;
using ENTIDAD.DTOs.Parametros;

namespace NEGOCIOS;

public class ParametroNegocio
{
    public readonly ParametroRepositorio _repositorio;

    public ParametroNegocio(ParametroRepositorio repositorio)
    {
        _repositorio = repositorio;
    }
    
    public async Task<IEnumerable<ParametroDto>> ObtenerParametrosPorTipoMuestraAsync(int tpmstId)
    {
        return await _repositorio.ObtenerParametrosPorTipoMuestraAsync(tpmstId);
    }
    
    public async Task<IEnumerable<ParametroDto>> ObtenerParametrosPorPruebaAsync(int idPrueba)
    {
        return await _repositorio.ObtenerParametrosPorPruebaAsync(idPrueba);
    }
    
    public async Task<ParametroDto?> AgregarParametroATipoMuestra(CreateParametroATipoDto createParametroATipoDto, string idUsuario)
    {
        return await _repositorio.AgregarParametroATipoMuestra(createParametroATipoDto, idUsuario);
    }
    
    public async Task<ParametroDto?> AgregarParametroAPrueba(CreateParametroAPruebaDto createParametroAPruebaDto, string idUsuario)
    {
        return await _repositorio.AgregarParametroAPrueba(createParametroAPruebaDto, idUsuario);
    }
}