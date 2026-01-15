using ENTIDAD.Models;

namespace API.Seed;

using DB.Datos;
using Microsoft.AspNetCore.Identity;
using Models;

public static class AppDataSeeder
{
    public static async Task SeedAsync(MasterDbContext context, RoleManager<IdentityRole>? roleManager = null)
    {
        int? GetTipoPruebaId(string codigo, string? nombreFallback = null)
        {
            var tipo = context.TipoPruebas.FirstOrDefault(t => t.Codigo == codigo)
                       ?? (nombreFallback != null
                           ? context.TipoPruebas.FirstOrDefault(t => t.Nombre == nombreFallback)
                           : null);

            return tipo?.IdTipoPrueba;
        }

        async Task EnsureTipoPruebaAsync(string codigo, string nombre)
        {
            var exists = context.TipoPruebas.Any(t => t.Codigo == codigo || t.Nombre == nombre);
            if (!exists)
            {
                context.TipoPruebas.Add(new TipoPrueba { Codigo = codigo, Nombre = nombre });
                await context.SaveChangesAsync();
            }
        }

        void EnsureTipoMuestraTipoPrueba(byte tpmstId, int? tipoPruebaId, int orden)
        {
            if (tipoPruebaId == null)
            {
                return;
            }

            var existing = context.TipoMuestraTipoPruebas
                .FirstOrDefault(t => t.TpmstId == tpmstId && t.TipoPruebaId == tipoPruebaId.Value);

            if (existing == null)
            {
                context.TipoMuestraTipoPruebas.Add(new TipoMuestraTipoPrueba
                {
                    TpmstId = tpmstId,
                    TipoPruebaId = tipoPruebaId.Value,
                    Orden = orden
                });
            }
            else if (existing.Orden != orden)
            {
                existing.Orden = orden;
            }
        }

        void UpsertParametroNorma(
            string nombreParametro,
            string? unidad,
            byte tpmstId,
            int? tipoPruebaId,
            decimal? valorMin,
            decimal? valorMax)
        {
            var existing = context.ParametroNormas
                .FirstOrDefault(p => p.TpmstId == tpmstId && p.NombreParametro == nombreParametro);

            if (existing == null)
            {
                context.ParametroNormas.Add(new ParametroNorma
                {
                    NombreParametro = nombreParametro,
                    Unidad = unidad,
                    TpmstId = tpmstId,
                    TipoPruebaId = tipoPruebaId,
                    ValorMin = valorMin,
                    ValorMax = valorMax
                });
            }
            else
            {
                existing.Unidad = unidad;
                existing.TipoPruebaId = tipoPruebaId;
                existing.ValorMin = valorMin;
                existing.ValorMax = valorMax;
            }
        }

        // Seed TipoMuestra
        if (!context.TipoMuestras.Any())
        {
            context.TipoMuestras.AddRange(
                new TipoMuestra { TpmstId = 1, Nombre = "Agua" },
                new TipoMuestra { TpmstId = 2, Nombre = "Alimento" },
                new TipoMuestra { TpmstId = 3, Nombre = "Bebida alcoholica" }
            );
        }

        // Seed TipoPrueba
        await EnsureTipoPruebaAsync("PAR_FQ", "Parámetros fisicoquímicos");
        await EnsureTipoPruebaAsync("MICRO", "Microbiológicos");
        await EnsureTipoPruebaAsync("AN_MICRO", "Análisis microbiológico");
        await EnsureTipoPruebaAsync("AN_FQ", "Análisis físico-químico");
        await EnsureTipoPruebaAsync("ETIQ", "Etiquetado");
        await EnsureTipoPruebaAsync("GRAD", "Graduación alcohólica");
        await EnsureTipoPruebaAsync("METALES", "Metales pesados");

        var tipoPruebaParFqId = GetTipoPruebaId("PAR_FQ", "Parámetros fisicoquímicos");
        var tipoPruebaMicroId = GetTipoPruebaId("MICRO", "Microbiológicos");
        var tipoPruebaAnalisisMicroId = GetTipoPruebaId("AN_MICRO", "Análisis microbiológico");

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
                new EstadoDocumento { IdEstadoDocumento = 2, Nombre = "Aprobado" },
                new EstadoDocumento { IdEstadoDocumento = 3, Nombre = "Preliminar" }
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

        // Agua:
        // - Analisis fisioquimico:
        //   - ph (6.5 - 8.5)
        //   - Solidos Totales (500 - 1500 mg/L)
        //   - Dureza (negativo)
        // Analisis microbiologico:
        //  - Recuento de Microorganismos (0 - 200 UFC/mL)
        //  - Coliformes Totales (0 - 10 NMP/100mL)

        // Alimento:
        // Todo lo que esta va en microbiologico

        // Alcohol:
        // Todo lo que esta va en fisioquimico

        // Agua
        UpsertParametroNorma("ph", "pH", 1, tipoPruebaParFqId, 6.5m, 8.5m);
        UpsertParametroNorma("Sólidos Totales", "mg/L", 1, tipoPruebaParFqId, 500m, 1500m);
        UpsertParametroNorma("Dureza", "mg/L", 1, tipoPruebaParFqId, 0m, 0m);
        UpsertParametroNorma("Recuento de Microorganismos", "UFC/mL", 1, tipoPruebaMicroId, 0m, 200m);
        UpsertParametroNorma("Coliformes Totales", "NMP/100mL", 1, tipoPruebaMicroId, 0m, 10m);

        // Alimento
        UpsertParametroNorma("Recuento de Microorganismos", "UFC/g", 2, tipoPruebaAnalisisMicroId, 0m, 100000m);
        UpsertParametroNorma("Coliformes Totales", "NMP/g", 2, tipoPruebaAnalisisMicroId, 0m, 100m);
        UpsertParametroNorma("E. coli", "NMP/g", 2, tipoPruebaAnalisisMicroId, 0m, 10m);
        UpsertParametroNorma("Salmonella spp.", "Presencia/25g", 2, tipoPruebaAnalisisMicroId, 0m, 0m);
        UpsertParametroNorma("Estafilococos aureus", "UFC/g", 2, tipoPruebaAnalisisMicroId, 0m, 100m);
        UpsertParametroNorma("Hongos", "UFC/g", 2, tipoPruebaAnalisisMicroId, 0m, 1000m);
        UpsertParametroNorma("Levaduras", "UFC/g", 2, tipoPruebaAnalisisMicroId, 0m, 1000m);
        UpsertParametroNorma("Esterilidad Comercial", "Presencia/25g", 2, tipoPruebaAnalisisMicroId, 0m, 0m);
        UpsertParametroNorma("Listeria monocytogenes", "Presencia/25g", 2, tipoPruebaAnalisisMicroId, 0m, 0m);

        // Bebida alcoholica
        // Grado Alcohólico
        UpsertParametroNorma("Grado Alcohólico", "% vol", 3, tipoPruebaParFqId, 5m, 40m);

        // Seed Tipo_Muestra_Tipo_Prueba
        EnsureTipoMuestraTipoPrueba(1, tipoPruebaParFqId, 1);
        EnsureTipoMuestraTipoPrueba(1, tipoPruebaMicroId, 2);

        EnsureTipoMuestraTipoPrueba(2, tipoPruebaAnalisisMicroId, 1);

        EnsureTipoMuestraTipoPrueba(3, tipoPruebaParFqId, 1);

        // Backfill: si existen pruebas sin tipo de prueba, asignarlas por nombre
        var pruebasSinTipo = context.Pruebas
            .Where(p => p.TipoPruebaId == null)
            .ToList();

        if (pruebasSinTipo.Count > 0)
        {
            var tiposPorNombre = context.TipoPruebas
                .ToDictionary(t => t.Nombre, t => t.IdTipoPrueba);

            foreach (var p in pruebasSinTipo)
            {
                if (tiposPorNombre.TryGetValue(p.NombrePrueba, out var tipoId))
                {
                    p.TipoPruebaId = tipoId;
                }
            }
        }

        if (roleManager != null)
        {
            string[] roleNames = { "Solicitante", "Analista", "Evaluador", "Admin" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        await context.SaveChangesAsync();
    }
}