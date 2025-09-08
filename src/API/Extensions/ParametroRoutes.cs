using System.Security.Claims;
using ENTIDAD.DTOs.Parametros;
using NEGOCIOS;

namespace API.Extensions;

public static class ParametroRoutes
{
    public static RouteGroupBuilder MapParametroRoutes(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/parametro").RequireAuthorization();
        
        
        group.MapGet("/tipo-muestra/{tipoMuestraId}", async (ParametroNegocio negocio, int tipoMuestraId) =>
        {
            try
            {
                var parametros = await negocio.ObtenerParametrosPorTipoMuestraAsync(tipoMuestraId);
                return Results.Ok(parametros);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }).RequireAuthorization("RequireAnalistaRole");

        group.MapPost("/tipo-muestra",
            async (ParametroNegocio negocio, CreateParametroDto createParametroATipoMuestraDto, ClaimsPrincipal user) =>
            {
                try
                {
                    var usuarioId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (usuarioId == null)
                    {
                        return Results.Unauthorized();
                    }
                    var parametro = await negocio.AgregarParametroATipoMuestra(createParametroATipoMuestraDto, usuarioId);
                    return Results.Ok(parametro);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });
        
        return group;
    }
}