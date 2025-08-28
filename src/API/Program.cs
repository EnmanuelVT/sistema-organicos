using API.Common;
using Microsoft.AspNetCore.Identity;
using DB.Datos;
using API.Extensions;
using API.Seed;
using Microsoft.EntityFrameworkCore;
using ENTIDAD.Models;
using DB.Datos.DocumentoD.Repositorios;
using DB.Datos.Repositorios;
using Microsoft.OpenApi.Models;
using NEGOCIOS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
    // Example in Startup.cs or Program.cs
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

        // Define the JWT Bearer security scheme
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

        // Add the security requirement
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddAuthorization(options =>
{
    AuthorizationPolicies.AddAuthorizationPolicies(options);
});
builder.Services.AddAuthentication(IdentityConstants.BearerScheme)
    .AddCookie(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<Usuario>()
    .AddRoles<IdentityRole>() 
    .AddEntityFrameworkStores<MasterDbContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<MasterDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<DocumentoNegocio>();
builder.Services.AddScoped<DocumentoRepositorio>();
builder.Services.AddScoped<AdminUserNegocio>();
builder.Services.AddScoped<AdminUserRepositorio>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<Usuario>, AdditionalUserClaimsPrincipalFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.MapIdentityApi<Usuario>();
app.MapDocumentosRoutes();
app.MapAdminUserRoutes();

// In Program.cs or Startup.cs
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
    await AppDataSeeder.SeedAsync(context);

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedRolesAsync(roleManager);
}

app.Run();
