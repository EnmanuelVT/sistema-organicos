using DB.Datos.Repositorios;
using Models;

namespace NEGOCIOS;

public class DocumentoNegocio
{
    private readonly DocumentoRepositorio _repositorio;

    public DocumentoNegocio(DocumentoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

}