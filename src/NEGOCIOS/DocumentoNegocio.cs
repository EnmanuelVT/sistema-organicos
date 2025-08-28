using DB.Datos.DocumentoD.Repositorios;
using Models;

namespace NEGOCIOS;

public class DocumentoNegocio
{
    private readonly DocumentoRepositorio _repositorio;

    public DocumentoNegocio(DocumentoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Documento>> Listar() 
    {
        var documentos = await _repositorio.ObtenerDocumentosAsync();
        return documentos;
    }
}