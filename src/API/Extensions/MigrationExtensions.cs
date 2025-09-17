using DB.Datos;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
