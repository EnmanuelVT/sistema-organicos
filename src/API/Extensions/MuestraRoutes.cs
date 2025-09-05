using System.Security.Claims;
using ENTIDAD.DTOs.Muestras;
using Models;
using NEGOCIOS;

namespace API.Extensions;

public static class MuestraRoutes
{
    public static RouteGroupBuilder MapMuestraRoutes(this IEndpointRouteBuilder routes) 
    {
        var group = routes.MapGroup("/api/muestras").RequireAuthorization();

        group.MapGet("/", async (MuestraNegocio negocio) => 
        {
            try 
            {
                var muestras = await negocio.ObtenerTodasLasMuestrasAsync();
                return Results.Ok(muestras);
            } 
            catch (Exception ex) 
            {
                return Results.Problem(ex.Message);
            }
        }).RequireAuthorization("RequireAnalistaRole");

        group.MapGet("/me", async (MuestraNegocio negocio, ClaimsPrincipal user) => 
        {
            try 
            {
                var usuarioId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (usuarioId == null) 
                {
                    return Results.Unauthorized();
                }
                var muestras = await negocio.ObtenerMuestrasPorUsuario(usuarioId);
                return Results.Ok(muestras);
            } 
            catch (Exception ex) 
            {
                return Results.Problem(ex.Message);
            }
        });

        group.MapGet("/usuario/{id}", async (MuestraNegocio negocio, ClaimsPrincipal user, string id) => 
        {
            try 
            {
                var muestras = await negocio.ObtenerMuestrasPorUsuario(id);
                return Results.Ok(muestras);
            } 
            catch (Exception ex) 
            {
                return Results.Problem(ex.Message);
            }
        }).RequireAuthorization("RequireAnalistaRole");

        group.MapGet("/analista/me", async (MuestraNegocio negocio, ClaimsPrincipal user) => 
        {
            try 
            {
                var analistaId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (analistaId == null) 
                {
                    return Results.Unauthorized();
                }
                var muestras = await negocio.ObtenerMuestrasPorAnalista(analistaId);
                return Results.Ok(muestras);
            } 
            catch (Exception ex) 
            {
                return Results.Problem(ex.Message);
            }
        }).RequireAuthorization("RequireAnalistaRole");

        group.MapGet("/analista/{id}", async (MuestraNegocio negocio, ClaimsPrincipal user, string id) => 
        {
            try 
            {
                var muestras = await negocio.ObtenerMuestrasPorAnalista(id);
                return Results.Ok(muestras);
            } 
            catch (Exception ex) 
            {
                return Results.Problem(ex.Message);
            }
        }).RequireAuthorization("RequireAdminRole");

        group.MapGet("/{id}", async (MuestraNegocio negocio, string id) => 
        {
            try 
            {
                var muestra = await negocio.ObtenerMuestraPorIdAsync(id);
                if (muestra == null) 
                {
                    return Results.NotFound();
                }
                return Results.Ok(muestra);
            } 
            catch (Exception ex) 
            {
                return Results.Problem(ex.Message);
            }
        });

        group.MapGet("Auditoria{id}", async (MuestraNegocio negocio, string id) =>
        {
            try
            {
                var auditoria = await negocio.ObtenerAuditoriaPorIdAsync(id);
                if (auditoria == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(auditoria);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        group.MapPost("/", async (MuestraNegocio negocio, CreateMuestraDto muestraDto, ClaimsPrincipal user) => 
        {
            try 
            {
                var usuarioId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (usuarioId == null) 
                {
                    return Results.Unauthorized();
                }

                var creadaMuestra = await negocio.CrearMuestraAsync(muestraDto, usuarioId);
                if (creadaMuestra == null) throw new Exception("No se pudo crear la muestra");
                return Results.Created($"/api/muestras/{creadaMuestra.MstCodigo}", (object?)creadaMuestra);
            } 
            catch (Exception ex) 
            {
                return Results.Problem(ex.Message);
            }
        });

        group.MapPatch("asignar-analista", async (MuestraNegocio negocio, AsignarAnalistaEnMuestraDto asignarAnalistaDto) =>
        {
            try
            {
                var bitacora = await negocio.AsignarAnalistaMuestraAsync(asignarAnalistaDto);
                if (bitacora == null)
                {
                    return Results.BadRequest("No se pudo asignar el analista a la muestra.");
                }
                return Results.Ok(bitacora);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        group.MapPatch("cambiar-estado", async (MuestraNegocio negocio, AsignarEstadoMuestraDto asignarEstadoDto, ClaimsPrincipal user) =>
        {
            try
            {
                var usuarioId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (usuarioId == null)
                {
                    return Results.Unauthorized();
                }

                var bitacora = await negocio.CambiarEstadoAsync(asignarEstadoDto, usuarioId);
                if (bitacora == null)
                {
                    return Results.BadRequest("No se pudo cambiar el estado de la muestra.");
                }
                return Results.Ok(bitacora);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        group.MapPut("/{id}", async (MuestraNegocio negocio, string id, CreateMuestraDto muestraDto) => 
        {
            try 
            {
                var muestraExistente = await negocio.ObtenerMuestraPorIdAsync(id);
                if (muestraExistente == null) 
                {
                    return Results.NotFound();
                }

                muestraExistente.Nombre = muestraDto.Nombre;
                muestraExistente.TpmstId = muestraDto.TpmstId;
                muestraExistente.Origen = muestraDto.Origen;
                muestraExistente.EstadoActual = muestraDto.EstadoActual;
                muestraExistente.CondicionesAlmacenamiento = muestraDto.CondicionesAlmacenamiento;
                muestraExistente.CondicionesTransporte = muestraDto.CondicionesTransporte;

                var actualizadaMuestra = await negocio.ModificarMuestraAsync(muestraExistente);
                return Results.Ok(actualizadaMuestra);
            } 
            catch (Exception ex) 
            {
                return Results.Problem(ex.Message);
            }
        }).RequireAuthorization("RequireAnalistaRole");
        // Solo el administrador puede modificar una muestra

        return group;
    }
}