using ENTIDAD.DTOs.Users;
using NEGOCIOS;
using System.Security.Claims;

namespace API.Extensions
{
    public static class UsuariosRoutes
    {
        public static RouteGroupBuilder MapUserRoutes(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/usuarios");

            group.MapGet("/me", async (UsuarioNegocio negocio, ClaimsPrincipal user) =>
            {
                try
                {
                    var usuarioId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (usuarioId == null)
                    {
                        return Results.Unauthorized();
                    }
                    var usuarios = await negocio.ObtenerUsuarioAsync(usuarioId);
                    return Results.Ok(usuarios);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });
            return group;
        }
    }
}
