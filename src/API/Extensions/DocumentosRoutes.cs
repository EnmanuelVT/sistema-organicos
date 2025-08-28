using NEGOCIOS;

namespace API.Extensions;

using Microsoft.AspNetCore.Builder;

public static class DocumentosRoutes
{
    public static RouteGroupBuilder MapDocumentosRoutes(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/documentos");

        group.MapGet("/", async (DocumentoNegocio negocio) =>
        {
            //try
            //{
            //    var documentos = await negocio.Listar();
            //    return Results.Ok(documentos);
            //}
            //catch (Exception ex)
            //{
            //    return Results.Problem(ex.Message);
            //}
        });

        return group;
    }
}
