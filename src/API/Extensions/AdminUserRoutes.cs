using ENTIDAD.DTOs;
using NEGOCIOS;
using ENTIDAD.Models;

namespace API.Extensions
{
    public static class AdminUserRoutes
    {
        public static RouteGroupBuilder MapAdminUserRoutes(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/admin/users").RequireAuthorization("RequireAdminRole");

            group.MapGet("/", async (AdminUserNegocio negocio) =>
            {
                try
                {
                    var users = await negocio.ListarUsuariosAsync();
                    return Results.Ok(users);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });

            group.MapPost("/", async (AdminUserNegocio negocio, CreateUserDto userDto) =>
            {
                try
                {
                    var usuario = await negocio.CrearUsuarioAsync(userDto);
                    return Results.Created($"/api/admin/users/{usuario.Id}", usuario);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });

            group.MapPut("/{id}", async (AdminUserNegocio negocio, UserDto userDto) =>
            {
                try
                {
                    await negocio.ActualizarUsuarioAsync(userDto);
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });

            group.MapDelete("/{id}", async (AdminUserNegocio negocio, string id) =>
            {
                try
                {
                    await negocio.EliminarUsuarioAsync(id);
                    return Results.NoContent();
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