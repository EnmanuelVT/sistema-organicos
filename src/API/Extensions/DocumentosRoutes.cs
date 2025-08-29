using NEGOCIOS;

namespace API.Extensions;

using Microsoft.AspNetCore.Builder;

public static class DocumentosRoutes
{
    private static readonly string _assetsPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets");
    public static RouteGroupBuilder MapDocumentosRoutes(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/documentos");

        group.MapGet("/formulario-agua-registro", async (DocumentoNegocio negocio) =>
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

            try
            {
                var filePath = Path.Combine(_assetsPath, "FormularioAguaRegistro.xlsx");

                if (!System.IO.File.Exists(filePath))
                {
                    return Results.NotFound("El archivo no fue encontrado.");
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return Results.File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheet");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        return group;
    }
}
