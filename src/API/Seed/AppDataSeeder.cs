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
                new EstadoMuestra { IdEstado = 3, Nombre = "En espera" },
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

        if (!context.ParametroNormas.Any())
        {
            // Agua
            // ph, Sólidos Totales, Dureza
            // Recuento de Microorganismos Aerobios Mesófilos
            // Recuento Coliformes
            // Coliformes Totales
            
            // Alimento
            // Recuento de Microorganismos Aerobios Mesófilos
            // Recuento Coliformes
            // Coliformes Totales
            // E. coli
            // Salmonella spp.
            // Estafilococos aureus
            // Hongos
            // Levaduras
            // Esterilidad Comercial
            // Listeria monocytogenes

            context.ParametroNormas.AddRange(
                // Agua
                new ParametroNorma { NombreParametro = "ph", Unidad = "litros", TpmstId = 1, ValorMin = 4, ValorMax = 10 },
                new ParametroNorma { NombreParametro = "Sólidos Totales", Unidad = "mg/L", TpmstId = 1, ValorMin = 500, ValorMax = 1500 },
                new ParametroNorma { NombreParametro = "Dureza", Unidad = "mg/L", TpmstId = 1, ValorMin = 100, ValorMax = 300 },
                new ParametroNorma { NombreParametro = "Recuento de Microorganismos", Unidad = "UFC/mL", TpmstId = 1, ValorMin = 0, ValorMax = 500 },
                new ParametroNorma { NombreParametro = "Coliformes Totales", Unidad = "NMP/100mL", TpmstId = 1, ValorMin = 0, ValorMax = 10 },
                
                // Alimento
                new ParametroNorma { NombreParametro = "Recuento de Microorganismos", Unidad = "UFC/g", TpmstId = 2, ValorMin = 0, ValorMax = 100000 },
                new ParametroNorma { NombreParametro = "Coliformes Totales", Unidad = "NMP/g", TpmstId = 2, ValorMin = 0, ValorMax = 100 },
                new ParametroNorma { NombreParametro = "E. coli", Unidad = "NMP/g", TpmstId = 2, ValorMin = 0, ValorMax = 10 },
                new ParametroNorma { NombreParametro = "Salmonella spp.", Unidad = "Presencia/25g", TpmstId = 2, ValorMin = 0, ValorMax = 0 },
                new ParametroNorma { NombreParametro = "Estafilococos aureus", Unidad = "UFC/g", TpmstId = 2, ValorMin = 0, ValorMax = 100 },
                new ParametroNorma { NombreParametro = "Hongos", Unidad = "UFC/g", TpmstId = 2, ValorMin = 0, ValorMax = 1000 },
                new ParametroNorma { NombreParametro = "Levaduras", Unidad = "UFC/g", TpmstId = 2, ValorMin = 0, ValorMax = 1000 },
                new ParametroNorma { NombreParametro = "Esterilidad Comercial", Unidad = "Presencia/25g", TpmstId = 2, ValorMin = 0, ValorMax = 0 },
                new ParametroNorma { NombreParametro = "Listeria monocytogenes", Unidad = "Presencia/25g", TpmstId = 2, ValorMin = 0, ValorMax = 0 },
                
                // Bebida alcoholica
                // Grado Alcohólico
                new ParametroNorma { NombreParametro = "Grado Alcohólico", Unidad = "% vol", TpmstId = 3, ValorMin = 5, ValorMax = 40 }
            );
        }

        await context.SaveChangesAsync();
    }
}