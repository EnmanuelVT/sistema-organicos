using System.Security.Claims;
using ENTIDAD.DTOs.Pruebas;
using NEGOCIOS;

namespace API.Extensions;

public static class PruebaRoutes
{
    public static RouteGroupBuilder MapPruebaRoutes(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/pruebas").RequireAuthorization();
        
        group.MapGet("/muestra/{idMuestra}", async (PruebaNegocio negocio, string idMuestra) =>
        {
            try
            {
                var pruebas = await negocio.ObtenerPruebasPorMuestraAsync(idMuestra);
                return Results.Ok(pruebas);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }).RequireAuthorization("RequireAnalistaRole");
        
        group.MapPost("/", async (PruebaNegocio negocio, CreatePruebaDto createPruebaDto, ClaimsPrincipal user) =>
        {
            try
            {
                var usuarioId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (usuarioId == null)
                {
                    return Results.Unauthorized();
                }
                var prueba = await negocio.CrearPruebaAsync(createPruebaDto, usuarioId);
                return Results.Ok(prueba);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }).RequireAuthorization("RequireAnalistaRole");

        return group;
    }
}