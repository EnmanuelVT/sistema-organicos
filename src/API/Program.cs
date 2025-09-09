using API.Common;
using Microsoft.AspNetCore.Identity;
using DB.Datos;
using API.Extensions;
using API.Seed;
using Microsoft.EntityFrameworkCore;
using ENTIDAD.Models;
using DB.Datos.Repositorios;
using Microsoft.OpenApi.Models;
using NEGOCIOS;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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

// builder.Services.AddAntiforgery();
builder.Services.AddScoped<DocumentoNegocio>();
builder.Services.AddScoped<DocumentoRepositorio>();
builder.Services.AddScoped<AdminUserNegocio>();
builder.Services.AddScoped<AdminUserRepositorio>();
builder.Services.AddScoped<UsuarioNegocio>();
builder.Services.AddScoped<UsuarioRepositorio>();
builder.Services.AddScoped<MuestraNegocio>();
builder.Services.AddScoped<MuestraRepositorio>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<Usuario>, AdditionalUserClaimsPrincipalFactory>();
builder.Services.AddScoped<ResultadoNegocio>();
builder.Services.AddScoped<ResultadoRepositorio>();
builder.Services.AddScoped<PruebaNegocio>();
builder.Services.AddScoped<PruebaRepositorio>();
builder.Services.AddScoped<ParametroNegocio>();
builder.Services.AddScoped<ParametroRepositorio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

// Enable CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// app.UseAntiforgery();

app.MapIdentityApi<Usuario>();
app.MapDocumentosRoutes();
app.MapAdminUserRoutes();
app.MapUserRoutes();
app.MapMuestraRoutes();
app.MapResultadoRoutes();
app.MapPruebaRoutes();
app.MapParametroRoutes();

// In Program.cs or Startup.cs
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
    await AppDataSeeder.SeedAsync(context);

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedRolesAsync(roleManager);
}

app.Run();
