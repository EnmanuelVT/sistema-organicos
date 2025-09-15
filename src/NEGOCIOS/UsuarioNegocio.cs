using DB.Datos.Repositorios;
using ENTIDAD.DTOs.Users;
using Models;


namespace NEGOCIOS;

public class UsuarioNegocio
{
    private readonly UsuarioRepositorio _repositorio;
    public UsuarioNegocio(UsuarioRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<UserDto> ObtenerUsuarioAsync(string id)
    {
        return await _repositorio.ObtenerUsuarioAsync(id);
    }


}

