using ENTIDAD.DTOs.Users;
using NEGOCIOS;

namespace API.Extensions
{
    public static class AdminUserRoutes
    {
        public static RouteGroupBuilder MapAdminUserRoutes(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/admin/users").RequireAuthorization("RequireAdminRole");

            group.MapGet("/", async (AdminUserNegocio negocio) =>
            {
                try
                {
                    var users = await negocio.ListarUsuariosAsync();
                    return Results.Ok(users);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
            .WithName("GetAllUsers")
            .WithSummary("Get all users")
            .WithDescription("Retrieves a list of all users in the system. Requires admin role.")
            .WithTags("Admin", "Users")
            .WithOpenApi()
            .Produces<List<UserDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapPost("/", async (AdminUserNegocio negocio, CreateUserDto userDto) =>
            {
                try
                {
                    var usuario = await negocio.CrearUsuarioAsync(userDto);
                    return Results.Created($"/api/admin/users/{usuario.Id}", usuario);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
            .WithName("CreateUser")
            .WithSummary("Create a new user")
            .WithDescription("Creates a new user in the system with the provided information. Requires admin role.")
            .WithTags("Admin", "Users")
            .WithOpenApi()
            .Accepts<CreateUserDto>("application/json")
            .Produces<UserDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapPut("/{id}", async (AdminUserNegocio negocio, UserDto userDto) =>
            {
                try
                {
                    await negocio.ActualizarUsuarioAsync(userDto);
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
            .WithName("UpdateUser")
            .WithSummary("Update an existing user")
            .WithDescription("Updates an existing user's information. The user ID in the URL must match the ID in the request body. Requires admin role.")
            .WithTags("Admin", "Users")
            .WithOpenApi()
            .Accepts<UserDto>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapDelete("/{id}", async (AdminUserNegocio negocio, string id) =>
            {
                try
                {
                    await negocio.EliminarUsuarioAsync(id);
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
            .WithName("DeleteUser")
            .WithSummary("Delete a user")
            .WithDescription("Permanently deletes a user from the system. This action cannot be undone. Requires admin role.")
            .WithTags("Admin", "Users")
            .WithOpenApi()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}