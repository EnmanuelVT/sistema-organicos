using DB.Datos.Repositorios;
using ENTIDAD.DTOs.Documentos;
using Models;

namespace NEGOCIOS;

public class DocumentoNegocio
{
    private readonly DocumentoRepositorio _repositorio;

    public DocumentoNegocio(DocumentoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<DocumentoDto>> ObtenerDocumentosAsync()
    {
        return await _repositorio.ObtenerDocumentosAsync();
    }
    
    public async Task<DocumentoDto?> ObtenerDocumentoAsync(int id)
    {
        return await _repositorio.ObtenerDocumentoAsync(id);
    }
    
    public async Task<DocumentoDto?> CrearDocumentoAsync(CreateDocumentoDto createDocumentoDto, string idUsuario)
    {
        return await _repositorio.GenerarDocumentoSpAsync(createDocumentoDto, idUsuario);
    }

    public async Task<DocumentoDto?> CambiarEstadoDocumentoAsync(CambiarEstadoDocumentoDto cambiarEstadoDocumentoDto, string idUsuario)
    {
        return await _repositorio.CambiarEstadoDocumentoAsync(cambiarEstadoDocumentoDto, idUsuario);
    }
}