using ENTIDAD.Models;

namespace API.Seed;

using DB.Datos;
using Models;

public static class AppDataSeeder
{
    public static async Task SeedAsync(MasterDbContext context)
    {
        // Seed TipoMuestra
        if (!context.TipoMuestras.Any())
        {
            context.TipoMuestras.AddRange(
                new TipoMuestra { TpmstId = 1, Nombre = "Agua" },
                new TipoMuestra { TpmstId = 2, Nombre = "Alimento" },
                new TipoMuestra { TpmstId = 3, Nombre = "Bebida alcoholica" }
            );
        }

        // Seed EstadoMuestra
        if (!context.EstadoMuestras.Any())
        {
            context.EstadoMuestras.AddRange(
                new EstadoMuestra { IdEstado = 1, Nombre = "Recibida" },
                new EstadoMuestra { IdEstado = 2, Nombre = "En analisis" },
                new EstadoMuestra { IdEstado = 2, Nombre = "En espera" },
                new EstadoMuestra { IdEstado = 4, Nombre = "Evaluada" },
                new EstadoMuestra { IdEstado = 5, Nombre = "Certificada" }
            );
        }

        // Seed EstadoDocumento
        if (!context.EstadoDocumentos.Any())
        {
            context.EstadoDocumentos.AddRange(
                new EstadoDocumento { IdEstadoDocumento = 1, Nombre = "Rechazado" },
                new EstadoDocumento { IdEstadoDocumento = 2, Nombre = "Aprobado" }
            );
        }

        // Seed TipoDocumento
        if (!context.TipoDocumentos.Any())
        {
            context.TipoDocumentos.AddRange(
                new TipoDocumento { IdTipoDoc = 1, Nombre = "Certificado" },
                new TipoDocumento { IdTipoDoc = 2, Nombre = "Informe" }
            );
        }

        await context.SaveChangesAsync();
    }
}