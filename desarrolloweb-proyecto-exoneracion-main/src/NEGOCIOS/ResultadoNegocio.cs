using DB.Datos.Repositorios;
using ENTIDAD.DTOs.ResultadosPruebas;

namespace NEGOCIOS;

public class ResultadoNegocio
{
    public readonly ResultadoRepositorio _repositorio;
    public ResultadoNegocio(ResultadoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }
    
    public async Task<IEnumerable<ResultadoPruebaDto?>> ObtenerResultadosPorMuestraAsync(string idMuestra)
    {
        return await _repositorio.ObtenerResultadosPorMuestraAsync(idMuestra);
    }
    
    public async Task<ResultadoPruebaDto?> ObtenerResultadoPorIdAsync(long idResultado)
    {
        return await _repositorio.ObtenerResultadoPorIdAsync(idResultado);
    }
    
    public async Task<ResultadoPruebaDto?> RegistrarResultadoAsync(CreateResultadoPruebaDto createResultadoPruebaDto, string idUsuario)
    {
        return await _repositorio.RegistrarResultadoAsync(createResultadoPruebaDto, idUsuario);
    }
}

