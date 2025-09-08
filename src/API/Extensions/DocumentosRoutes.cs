using System.Security.Claims;
using ENTIDAD.DTOs.Documentos;
using Microsoft.AspNetCore.Mvc;
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
        
        group.MapPost("/formulario-agua-registro", async (HttpRequest request, DocumentoNegocio negocio, ClaimsPrincipal user) =>
        {
            try
            {
                var usuarioId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (usuarioId == null)
                {
                    return Results.Unauthorized();
                }

                if (!request.HasFormContentType)
                {
                    return Results.BadRequest("El contenido debe ser de tipo formulario.");
                }

                var form = await request.ReadFormAsync();
                var file = form.Files.GetFile("file");
                
                if (file == null || file.Length == 0)
                {
                    return Results.BadRequest("No se ha proporcionado un archivo válido.");
                }

                // Extract CreateDocumentoDto from form data
                var createDocumentoDto = new CreateDocumentoDto
                {
                    IdMuestra = form["IdMuestra"].ToString(),
                    IdTipoDoc = byte.Parse(form["IdTipoDoc"]),
                    Version = int.Parse(form["Version"]),
                    RutaArchivo = form["RutaArchivo"].ToString() ?? "",
                    DocPdf = await GetFileBytesAsync(file)
                };

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                await negocio.CrearDocumentoAsync(createDocumentoDto, usuarioId);

                return Results.Ok(new { Message = "Archivo subido exitosamente.", FilePath = filePath });
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        group.MapPatch("/cambiar-estado", async (CambiarEstadoDocumentoDto cambiarEstadoDocumentoDto, DocumentoNegocio negocio, ClaimsPrincipal user) =>
        {
            try
            {
                var usuarioId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (usuarioId == null)
                {
                    return Results.Unauthorized();
                }

                var resultado = await negocio.CambiarEstadoDocumentoAsync(cambiarEstadoDocumentoDto, usuarioId);
                if (resultado == null)
                {
                    return Results.NotFound("Documento no encontrado.");
                }
                return Results.Ok(resultado);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        return group;
    }

    private static async Task<byte[]> GetFileBytesAsync(IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
