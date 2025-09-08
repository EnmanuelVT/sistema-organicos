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
        }).RequireAuthorization("RequireAnalistaRole");
        
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
        }).RequireAuthorization("RequireAnalistaRole");

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
        }).RequireAuthorization("RequireAnalistaRole");
        
        group.MapPatch("/validar", async (ResultadoNegocio negocio, ValidarResultadoDto validarResultadoDto, ClaimsPrincipal user) =>
        {
            try
            {
                var usuarioId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (usuarioId == null)
                {
                    return Results.Unauthorized();
                }

                var resultado = await negocio.ValidarResultadoAsync(validarResultadoDto, usuarioId);
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
        }).RequireAuthorization("RequireEvaluadorRole");

        return group;
    }
}