using System.Security.Claims;
using ENTIDAD.DTOs.Muestras;
using NEGOCIOS;

namespace API.Extensions;

public static class AuditoriaRoutes
{
    public static RouteGroupBuilder MapAuditoriaRoutes(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/auditoria").RequireAuthorization();

        group.MapGet("/", async (MuestraNegocio negocio) =>
        {
            try
            {
                var auditorias = await negocio.ObtenerAuditoriasAsync();
                return Results.Ok(auditorias);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .RequireAuthorization("RequireAdminRole")
        .WithName("GetAllAuditorias")
        .WithSummary("Get all audit records")
        .WithDescription("Retrieves all audit records in the system. Requires admin role.")
        .WithTags("Auditoria", "Admin")
        .WithOpenApi()
        .Produces<List<AuditoriaDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapGet("/usuario/{id}", async (MuestraNegocio negocio, string id) =>
        {
            try
            {
                var auditoria = await negocio.ObtenerAuditoriaPorIdAsync(id);
                return Results.Ok(auditoria);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .RequireAuthorization("RequireAdminRole")
        .WithName("GetAuditoriaByUserId")
        .WithSummary("Get audit records by user ID")
        .WithDescription("Retrieves audit records for a specific user. Requires admin role.")
        .WithTags("Auditoria", "Admin")
        .WithOpenApi()
        .Produces<List<AuditoriaDto>>(StatusCodes.Status200OK)
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
                var auditoria = await negocio.ObtenerAuditoriaPorIdAsync(usuarioId);
                return Results.Ok(auditoria);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithName("GetMyAuditoria")
        .WithSummary("Get current user's audit records")
        .WithDescription("Retrieves audit records for the currently authenticated user.")
        .WithTags("Auditoria")
        .WithOpenApi()
        .Produces<List<AuditoriaDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        return group;
    }
}