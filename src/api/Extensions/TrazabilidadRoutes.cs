using System.Security.Claims;
using ENTIDAD.DTOs.Muestras;
using NEGOCIOS;

namespace API.Extensions;

public static class TrazabilidadRoutes
{
    public static RouteGroupBuilder MapTrazabilidadRoutes(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/trazabilidad").RequireAuthorization();

        group.MapGet("/", async (MuestraNegocio negocio) =>
        {
            try
            {
                var historial = await negocio.ObtenerHistorialTrazabilidadAsync();
                return Results.Ok(historial);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .RequireAuthorization("RequireAdminRole")
        .WithName("GetAllTrazabilidad")
        .WithSummary("Get all traceability records")
        .WithDescription("Retrieves all traceability records in the system. Requires admin role.")
        .WithTags("Trazabilidad", "Admin")
        .WithOpenApi()
        .Produces<List<HistorialDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapGet("/usuario/{id}", async (MuestraNegocio negocio, string id) =>
        {
            try
            {
                var historial = await negocio.ObtenerHistorialTrazabilidadPorIdAsync(id);
                return Results.Ok(historial);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .RequireAuthorization("RequireAdminRole")
        .WithName("GetTrazabilidadByUserId")
        .WithSummary("Get traceability records by user ID")
        .WithDescription("Retrieves traceability records for a specific user. Requires admin role.")
        .WithTags("Trazabilidad", "Admin")
        .WithOpenApi()
        .Produces<List<HistorialDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapGet("/me", async (MuestraNegocio negocio, ClaimsPrincipal user) =>
        {
            try
            {
                var usuarioId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (usuarioId == null)
                {
                    return Results.Unauthorized();
                }
                var historial = await negocio.ObtenerHistorialTrazabilidadPorIdAsync(usuarioId);
                return Results.Ok(historial);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithName("GetMyTrazabilidad")
        .WithSummary("Get current user's traceability records")
        .WithDescription("Retrieves traceability records for the currently authenticated user.")
        .WithTags("Trazabilidad")
        .WithOpenApi()
        .Produces<List<HistorialDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        return group;
    }
}