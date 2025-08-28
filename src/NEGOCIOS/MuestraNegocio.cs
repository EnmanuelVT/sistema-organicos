using DB.Datos.Repositorios;
using ENTIDAD.DTOs.Muestras;
using Models;

namespace NEGOCIOS;

public class MuestraNegocio
{
    private readonly MuestraRepositorio _repositorio;
    public MuestraNegocio(MuestraRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<Muestra?> ObtenerMuestraPorIdAsync(string id)
    {
        return await _repositorio.ObtenerMuestraPorIdAsync(id);
    }

    public async Task<IEnumerable<MuestraDto>> ObtenerTodasLasMuestrasAsync()
    {
        return await _repositorio.ObtenerTodasLasMuestrasAsync();
    }
    public async Task<IEnumerable<MuestraDto>> ObtenerMuestrasPorUsuario(string id)
    {
        return await _repositorio.ObtenerMuestrasPorUsuarioAsync(id);
    }
    public async Task<IEnumerable<MuestraDto>> ObtenerMuestrasPorAnalista(string id)
    {
        return await _repositorio.ObtenerMuestrasPorAnalistaAsync(id);
    }

    public async Task<MuestraDto?> CrearMuestraAsync(CreateMuestraDto nuevaMuestra, string usuarioId)
    {
        return await _repositorio.CrearMuestraAsync(nuevaMuestra, usuarioId);
    }

    public async Task<BitacoraMuestra?> AsignarAnalistaMuestraAsync(AsignarAnalistaEnMuestraDto asignarAnalistaEnMuestraDto)
    {
        return await _repositorio.AsignarAnalistaAsync(asignarAnalistaEnMuestraDto);
    }

    public async Task<MuestraDto?> ModificarMuestraAsync(Muestra muestraActualizada)
    {
        return await _repositorio.ModificarMuestraAsync(muestraActualizada);
    }
}