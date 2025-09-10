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
        })
        .RequireAuthorization("RequireAnalistaRole")
        .WithName("GetAllMuestras")
        .WithSummary("Get all samples")
        .WithDescription("Retrieves a list of all samples in the system. Requires analyst role.")
        .WithTags("Muestras")
        .WithOpenApi()
        .Produces<List<MuestraDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
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
                var muestras = await negocio.ObtenerMuestrasPorUsuario(usuarioId);
                return Results.Ok(muestras);
            } 
            catch (Exception ex) 
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithName("GetMyMuestras")
        .WithSummary("Get current user's samples")
        .WithDescription("Retrieves all samples belonging to the currently authenticated user.")
        .WithTags("Muestras")
        .WithOpenApi()
        .Produces<List<MuestraDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

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
        })
        .RequireAuthorization("RequireAnalistaRole")
        .WithName("GetMuestrasByUser")
        .WithSummary("Get samples by user ID")
        .WithDescription("Retrieves all samples belonging to a specific user. Requires analyst role.")
        .WithTags("Muestras")
        .WithOpenApi()
        .Produces<List<MuestraDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

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
        })
        .RequireAuthorization("RequireAnalistaRole")
        .WithName("GetMyAssignedMuestras")
        .WithSummary("Get samples assigned to current analyst")
        .WithDescription("Retrieves all samples assigned to the currently authenticated analyst.")
        .WithTags("Muestras", "Analyst")
        .WithOpenApi()
        .Produces<List<MuestraDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

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
        })
        .RequireAuthorization("RequireAdminRole")
        .WithName("GetMuestrasByAnalyst")
        .WithSummary("Get samples assigned to specific analyst")
        .WithDescription("Retrieves all samples assigned to a specific analyst. Requires admin role.")
        .WithTags("Muestras", "Admin", "Analyst")
        .WithOpenApi()
        .Produces<List<MuestraDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

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
        })
        .WithName("GetMuestraById")
        .WithSummary("Get sample by ID")
        .WithDescription("Retrieves a specific sample by its unique identifier.")
        .WithTags("Muestras")
        .WithOpenApi()
        .Produces<MuestraDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapGet("Auditoria", async (MuestraNegocio negocio) =>
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
        .WithName("GetMuestraAudits")
        .WithSummary("Get sample audit trail")
        .WithDescription("Retrieves the audit trails. Requires admin role.")
        .WithTags("Muestras", "Admin", "Audit")
        .WithOpenApi()
        .Produces<object>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

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
        })
        .RequireAuthorization("RequireAdminRole")
        .WithName("GetMuestraAuditById")
        .WithSummary("Get sample audit trail by ID")
        .WithDescription("Retrieves the audit trail for a specific sample. Requires admin role.")
        .WithTags("Muestras", "Admin", "Audit")
        .WithOpenApi()
        .Produces<object>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapGet("Historial", async (MuestraNegocio negocio) =>
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
        .WithName("GetMuestraHistorial")
        .WithSummary("Get history")
        .WithDescription("Retrieves the history. Requires admin role.")
        .WithTags("Muestras", "Admin", "History")
        .WithOpenApi()
        .Produces<object>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapGet("Historial{id}", async (MuestraNegocio negocio, string id) =>
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
        .WithName("GetMuestraHistorialById")
        .WithSummary("Get history")
        .WithDescription("Retrieves the history. Requires admin role.")
        .WithTags("Muestras", "Admin", "History")
        .WithOpenApi()
        .Produces<object>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

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
        })
        .WithName("CreateMuestra")
        .WithSummary("Create a new sample")
        .WithDescription("Creates a new sample in the system with the provided information.")
        .WithTags("Muestras")
        .WithOpenApi()
        .Accepts<CreateMuestraDto>("application/json")
        .Produces<object>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

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
        })
        .WithName("AssignAnalystToMuestra")
        .WithSummary("Assign analyst to sample")
        .WithDescription("Assigns a specific analyst to a sample for analysis.")
        .WithTags("Muestras", "Analyst")
        .WithOpenApi()
        .Accepts<AsignarAnalistaEnMuestraDto>("application/json")
        .Produces<object>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

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
        })
        .WithName("ChangeMuestraStatus")
        .WithSummary("Change sample status")
        .WithDescription("Updates the status of a sample with tracking information.")
        .WithTags("Muestras")
        .WithOpenApi()
        .Accepts<AsignarEstadoMuestraDto>("application/json")
        .Produces<object>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

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
        })
        .RequireAuthorization("RequireAnalistaRole")
        .WithName("UpdateMuestra")
        .WithSummary("Update an existing sample")
        .WithDescription("Updates an existing sample's information. Requires analyst role.")
        .WithTags("Muestras", "Analyst")
        .WithOpenApi()
        .Accepts<CreateMuestraDto>("application/json")
        .Produces<object>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapPost("/{id}/evaluar", async (MuestraNegocio negocio, string id, EvaluarMuestraDto evaluarDto, ClaimsPrincipal user) =>
        {
            try
            {
                var evaluadorId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (evaluadorId == null)
                {
                    return Results.Unauthorized();
                }

                // Asegurar que el ID de la muestra coincida
                evaluarDto.MuestraId = id;

                var resultado = await negocio.EvaluarMuestraAsync(evaluarDto, evaluadorId);
                if (resultado == null)
                {
                    return Results.NotFound("Muestra no encontrada");
                }

                return Results.Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .RequireAuthorization("RequireEvaluadorRole")
        .WithName("EvaluateMuestra")
        .WithSummary("Evaluate a sample")
        .WithDescription("Performs evaluation of a sample, marking it as approved or rejected with observations. Requires evaluator role.")
        .WithTags("Muestras", "Evaluator")
        .WithOpenApi()
        .Accepts<EvaluarMuestraDto>("application/json")
        .Produces<object>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
        
        // API.Extensions/MuestraRoutes.cs (dentro de MapMuestraRoutes)
        group.MapPost("/pruebas/{id:int}/evaluar", async (MuestraNegocio negocio, int id, EvaluarPruebaDto body, ClaimsPrincipal user) =>
            {
                try
                {
                    var evaluadorId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (evaluadorId == null)
                        return Results.Unauthorized();

                    var resp = await negocio.EvaluarPruebaAsync(id, body, evaluadorId);
                    if (resp is null) return Results.NotFound("Prueba no encontrada o inválida");

                    return Results.Ok(resp);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
            .RequireAuthorization("RequireEvaluadorRole")
            .WithName("EvaluatePrueba")
            .WithSummary("Evaluate a test (Prueba)")
            .WithDescription("Approves or rejects a specific test and generates the corresponding certificate when approved. Requires evaluator role.")
            .WithTags("Muestras", "Pruebas", "Evaluator")
            .WithOpenApi()
            .Accepts<EvaluarPruebaDto>("application/json")
            .Produces<object>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);


        return group;
    }
}