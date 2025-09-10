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
    public async Task<IEnumerable<AuditoriaDto>> ObtenerAuditoriasAsync()
    {
        return await _repositorio.ObtenerAuditoriasAsync();
    }
    public async Task<IEnumerable<AuditoriaDto>> ObtenerAuditoriaPorIdAsync(string id)
    {
        return await _repositorio.ObtenerAuditoriaPorIdAsync(id);
    }

    public async Task<IEnumerable<HistorialDto>> ObtenerHistorialTrazabilidadAsync()
    {
        return await _repositorio.ObtenerHistorialTrazabilidadAsync();
    }

    public async Task<IEnumerable<HistorialDto>> ObtenerHistorialTrazabilidadPorIdAsync(string id)
    {
        return await _repositorio.ObtenerHistorialTrazabilidadPorIdAsync(id);
    }

    public async Task<MuestraDto?> CrearMuestraAsync(CreateMuestraDto nuevaMuestra, string usuarioId)
    {
        return await _repositorio.CrearMuestraAsync(nuevaMuestra, usuarioId);
    }

    public async Task<BitacoraMuestra?> AsignarAnalistaMuestraAsync(AsignarAnalistaEnMuestraDto asignarAnalistaEnMuestraDto)
    {
        return await _repositorio.AsignarAnalistaAsync(asignarAnalistaEnMuestraDto);
    }

    public async Task<BitacoraMuestra?> CambiarEstadoAsync(AsignarEstadoMuestraDto asignarEstadoMuestraDto, string idUsuario)
    {
        return await _repositorio.CambiarEstadoAsync(asignarEstadoMuestraDto, idUsuario);
    }

    public async Task<MuestraDto?> ModificarMuestraAsync(Muestra muestraActualizada)
    {
        return await _repositorio.ModificarMuestraAsync(muestraActualizada);
    }

    public async Task<EvaluarMuestraResponseDto?> EvaluarMuestraAsync(EvaluarMuestraDto evaluarDto, string evaluadorId)
    {
        // Validaciones de negocio
        if (string.IsNullOrWhiteSpace(evaluarDto.MuestraId))
        {
            throw new ArgumentException("El ID de la muestra es requerido");
        }

        if (string.IsNullOrWhiteSpace(evaluadorId))
        {
            throw new ArgumentException("El ID del evaluador es requerido");
        }

        if (!evaluarDto.Aprobado && string.IsNullOrWhiteSpace(evaluarDto.Observaciones))
        {
            throw new ArgumentException("Las observaciones son requeridas cuando se rechaza una muestra");
        }

        return await _repositorio.EvaluarMuestraAsync(evaluarDto, evaluadorId);
    }
    
    // NEGOCIOS/MuestraNegocio.cs
    public async Task<EvaluarPruebaResponseDto?> EvaluarPruebaAsync(int idPrueba, EvaluarPruebaDto dto, string evaluadorId)
    {
        if (string.IsNullOrWhiteSpace(evaluadorId))
            throw new ArgumentException("El ID del evaluador es requerido");

        if (!dto.Aprobado && string.IsNullOrWhiteSpace(dto.Observaciones))
            throw new ArgumentException("Las observaciones son requeridas cuando se rechaza una prueba");

        // Delegamos al repositorio (método que ya preparamos anteriormente)
        return await _repositorio.EvaluarPruebaAsync(
            new EvaluarPruebaDto { IdPrueba = idPrueba, Aprobado = dto.Aprobado, Observaciones = dto.Observaciones, /* (id va en la ruta) */ },
            evaluadorId
        );
    }
}