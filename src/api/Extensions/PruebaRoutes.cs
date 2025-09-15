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
        })
        .RequireAuthorization("RequireAnalistaRole")
        .WithName("GetPruebasByMuestra")
        .WithSummary("Get tests by sample ID")
        .WithDescription("Retrieves all tests associated with a specific sample. Requires analyst role.")
        .WithTags("Pruebas", "Analyst")
        .WithOpenApi()
        .Produces<List<PruebaDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
        
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
        })
        .RequireAuthorization("RequireAnalistaRole")
        .WithName("CreatePrueba")
        .WithSummary("Create a new test")
        .WithDescription("Creates a new test for a specific sample. The test will be associated with the authenticated analyst. Requires analyst role.")
        .WithTags("Pruebas", "Analyst")
        .WithOpenApi()
        .Accepts<CreatePruebaDto>("application/json")
        .Produces<object>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        return group;
    }
}