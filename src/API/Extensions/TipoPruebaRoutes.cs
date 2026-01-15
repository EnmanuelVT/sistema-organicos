using ENTIDAD.DTOs.TipoPruebas;
using NEGOCIOS;

namespace API.Extensions;

public static class TipoPruebaRoutes
{
    public static RouteGroupBuilder MapTipoPruebaRoutes(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/tipo-prueba").RequireAuthorization();

        group.MapGet("/", async (TipoPruebaNegocio negocio) =>
        {
            try
            {
                var tipos = await negocio.ObtenerTiposAsync();
                return Results.Ok(tipos);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .RequireAuthorization("RequireAnalistaRole")
        .WithName("GetTiposPrueba")
        .WithSummary("List test types")
        .WithDescription("Retrieves all available test types (Tipo_Prueba). Requires analyst role.")
        .WithTags("TipoPrueba", "Analyst")
        .WithOpenApi()
        .Produces<List<TipoPruebaDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        return group;
    }
}
