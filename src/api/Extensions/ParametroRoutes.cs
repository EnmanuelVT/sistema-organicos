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
        })
        .RequireAuthorization("RequireAnalistaRole")
        .WithName("GetParametrosByTipoMuestra")
        .WithSummary("Get parameters by sample type")
        .WithDescription("Retrieves all parameters associated with a specific sample type. Requires analyst role.")
        .WithTags("Parametros", "Analyst")
        .WithOpenApi()
        .Produces<List<ParametroDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapPost("/tipo-muestra", async (ParametroNegocio negocio, CreateParametroDto createParametroATipoMuestraDto, ClaimsPrincipal user) =>
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
        })
        .WithName("AddParametroToTipoMuestra")
        .WithSummary("Add parameter to sample type")
        .WithDescription("Creates a new parameter and associates it with a sample type. The parameter will be tracked with the authenticated user.")
        .WithTags("Parametros")
        .WithOpenApi()
        .Accepts<CreateParametroDto>("application/json")
        .Produces<object>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        return group;
    }
}
