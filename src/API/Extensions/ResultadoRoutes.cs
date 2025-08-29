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

        group.MapPost("/", async (ResultadoNegocio negocio, CreateResultadoPruebaDto createResultadoPruebaDto) =>
        {
            try
            {
                var resultado = await negocio.RegistrarResultadoAsync(createResultadoPruebaDto);
                return Results.Ok(resultado);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }).RequireAuthorization("RequireAnalistaRole");
        
        group.MapPatch("/validar", async (ResultadoNegocio negocio, ValidarResultadoDto validarResultadoDto) =>
        {
            try
            {
                var resultado = await negocio.ValidarResulltadoAsync(validarResultadoDto);
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