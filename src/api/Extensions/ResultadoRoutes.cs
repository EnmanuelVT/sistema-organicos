using System.Security.Claims;
using ENTIDAD.DTOs.ResultadosPruebas;
using NEGOCIOS;

namespace API.Extensions;

public static class ResultadoRoutes
{
    public static RouteGroupBuilder MapResultadoRoutes(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/resultados").RequireAuthorization();
        
        group.MapGet("/muestra/{idMuestra}", async (ResultadoNegocio negocio, string idMuestra) =>
        {
            try
            {
                var resultados = await negocio.ObtenerResultadosPorMuestraAsync(idMuestra);
                return Results.Ok(resultados);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .RequireAuthorization("RequireAnalistaRole")
        .WithName("GetResultadosByMuestra")
        .WithSummary("Get test results by sample ID")
        .WithDescription("Retrieves all test results associated with a specific sample. Requires analyst role.")
        .WithTags("Resultados", "Analyst")
        .WithOpenApi()
        .Produces<List<ResultadoPruebaDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
        
        group.MapGet("/{idResultado}", async (ResultadoNegocio negocio, long idResultado) =>
        {
            try
            {
                var resultado = await negocio.ObtenerResultadoPorIdAsync(idResultado);
                if (resultado == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(resultado);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .RequireAuthorization("RequireAnalistaRole")
        .WithName("GetResultadoById")
        .WithSummary("Get test result by ID")
        .WithDescription("Retrieves a specific test result by its unique identifier. Requires analyst role.")
        .WithTags("Resultados", "Analyst")
        .WithOpenApi()
        .Produces<ResultadoPruebaDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapPost("/", async (ResultadoNegocio negocio, CreateResultadoPruebaDto createResultadoPruebaDto, ClaimsPrincipal user) =>
        {
            try
            {
                var usuarioId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (usuarioId == null)
                {
                    return Results.Unauthorized();
                }

                var resultado = await negocio.RegistrarResultadoAsync(createResultadoPruebaDto, usuarioId);
                return Results.Ok(resultado);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .RequireAuthorization("RequireAnalistaRole")
        .WithName("CreateResultado")
        .WithSummary("Register a new test result")
        .WithDescription("Creates a new test result for a specific sample and parameter. The result will be associated with the authenticated analyst. Requires analyst role.")
        .WithTags("Resultados", "Analyst")
        .WithOpenApi()
        .Accepts<CreateResultadoPruebaDto>("application/json")
        .Produces<object>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        return group;
    }
}