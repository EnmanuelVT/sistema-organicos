using DB.Datos.Repositorios;
using ENTIDAD.DTOs.TipoPruebas;

namespace NEGOCIOS;

public class TipoPruebaNegocio
{
    private readonly TipoPruebaRepositorio _repositorio;

    public TipoPruebaNegocio(TipoPruebaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<TipoPruebaDto>> ObtenerTiposAsync()
    {
        return await _repositorio.ObtenerTiposAsync();
    }
}
