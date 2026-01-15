using ENTIDAD.Models;

namespace API.Seed;

using DB.Datos;
using Models;

public static class AppDataSeeder
{
    public static async Task SeedAsync(MasterDbContext context)
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

        // Seed TipoMuestra
        if (!context.TipoMuestras.Any())
        {
            context.TipoMuestras.AddRange(
                new TipoMuestra { TpmstId = 1, Nombre = "Agua" },
                new TipoMuestra { TpmstId = 2, Nombre = "Alimento" },
                new TipoMuestra { TpmstId = 3, Nombre = "Bebida alcoholica" }
            );
        }

    // Seed TipoPrueba está en la migración (SeedTipoPrueba)
    // Aun así, aseguramos que existan los tipos mínimos para que los parámetros queden relacionados.
    await EnsureTipoPruebaAsync("PAR_FQ", "Parámetros fisicoquímicos");
    await EnsureTipoPruebaAsync("MICRO", "Microbiológicos");
    await EnsureTipoPruebaAsync("AN_MICRO", "Análisis microbiológico");
    await EnsureTipoPruebaAsync("GRAD", "Graduación alcohólica");

    var tipoPruebaParFqId = GetTipoPruebaId("PAR_FQ", "Parámetros fisicoquímicos");
    var tipoPruebaMicroId = GetTipoPruebaId("MICRO", "Microbiológicos");
    var tipoPruebaAnalisisMicroId = GetTipoPruebaId("AN_MICRO", "Análisis microbiológico");
    var tipoPruebaGraduacionId = GetTipoPruebaId("GRAD", "Graduación alcohólica");

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

        if (!context.ParametroNormas.Any())
        {
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
            

            context.ParametroNormas.AddRange(
                // Agua
                new ParametroNorma { NombreParametro = "ph", Unidad = "litros", TpmstId = 1, TipoPruebaId = tipoPruebaParFqId, ValorMin = 4, ValorMax = 10 },
                new ParametroNorma { NombreParametro = "Sólidos Totales", Unidad = "mg/L", TpmstId = 1, TipoPruebaId = tipoPruebaParFqId, ValorMin = 500, ValorMax = 1500 },
                new ParametroNorma { NombreParametro = "Dureza", Unidad = "mg/L", TpmstId = 1, TipoPruebaId = tipoPruebaParFqId, ValorMin = 100, ValorMax = 300 },
                new ParametroNorma { NombreParametro = "Recuento de Microorganismos", Unidad = "UFC/mL", TpmstId = 1, TipoPruebaId = tipoPruebaMicroId, ValorMin = 0, ValorMax = 500 },
                new ParametroNorma { NombreParametro = "Coliformes Totales", Unidad = "NMP/100mL", TpmstId = 1, TipoPruebaId = tipoPruebaMicroId, ValorMin = 0, ValorMax = 10 },
                
                // Alimento
                new ParametroNorma { NombreParametro = "Recuento de Microorganismos", Unidad = "UFC/g", TpmstId = 2, TipoPruebaId = tipoPruebaAnalisisMicroId, ValorMin = 0, ValorMax = 100000 },
                new ParametroNorma { NombreParametro = "Coliformes Totales", Unidad = "NMP/g", TpmstId = 2, TipoPruebaId = tipoPruebaAnalisisMicroId, ValorMin = 0, ValorMax = 100 },
                new ParametroNorma { NombreParametro = "E. coli", Unidad = "NMP/g", TpmstId = 2, TipoPruebaId = tipoPruebaAnalisisMicroId, ValorMin = 0, ValorMax = 10 },
                new ParametroNorma { NombreParametro = "Salmonella spp.", Unidad = "Presencia/25g", TpmstId = 2, TipoPruebaId = tipoPruebaAnalisisMicroId, ValorMin = 0, ValorMax = 0 },
                new ParametroNorma { NombreParametro = "Estafilococos aureus", Unidad = "UFC/g", TpmstId = 2, TipoPruebaId = tipoPruebaAnalisisMicroId, ValorMin = 0, ValorMax = 100 },
                new ParametroNorma { NombreParametro = "Hongos", Unidad = "UFC/g", TpmstId = 2, TipoPruebaId = tipoPruebaAnalisisMicroId, ValorMin = 0, ValorMax = 1000 },
                new ParametroNorma { NombreParametro = "Levaduras", Unidad = "UFC/g", TpmstId = 2, TipoPruebaId = tipoPruebaAnalisisMicroId, ValorMin = 0, ValorMax = 1000 },
                new ParametroNorma { NombreParametro = "Esterilidad Comercial", Unidad = "Presencia/25g", TpmstId = 2, TipoPruebaId = tipoPruebaAnalisisMicroId, ValorMin = 0, ValorMax = 0 },
                new ParametroNorma { NombreParametro = "Listeria monocytogenes", Unidad = "Presencia/25g", TpmstId = 2, TipoPruebaId = tipoPruebaAnalisisMicroId, ValorMin = 0, ValorMax = 0 },
                
                // Bebida alcoholica
                // Grado Alcohólico
                new ParametroNorma { NombreParametro = "Grado Alcohólico", Unidad = "% vol", TpmstId = 3, TipoPruebaId = tipoPruebaGraduacionId, ValorMin = 5, ValorMax = 40 }
            );
        }

        // Backfill: si existen parámetros sin tipo de prueba, asignarlos según reglas del seeder
        // (Esto es importante porque el frontend ahora filtra estrictamente por tipo de prueba.)
        var parametrosSinTipo = context.ParametroNormas
            .Where(p => p.TipoPruebaId == null)
            .ToList();

        if (parametrosSinTipo.Count > 0)
        {
            foreach (var p in parametrosSinTipo)
            {
                if (p.TpmstId == 1)
                {
                    if (p.NombreParametro == "ph" || p.NombreParametro == "Sólidos Totales" || p.NombreParametro == "Dureza")
                    {
                        p.TipoPruebaId = tipoPruebaParFqId;
                    }
                    else if (p.NombreParametro == "Recuento de Microorganismos" || p.NombreParametro == "Coliformes Totales")
                    {
                        p.TipoPruebaId = tipoPruebaMicroId;
                    }
                }
                else if (p.TpmstId == 2)
                {
                    p.TipoPruebaId = tipoPruebaAnalisisMicroId;
                }
                else if (p.TpmstId == 3)
                {
                    if (p.NombreParametro == "Grado Alcohólico")
                    {
                        p.TipoPruebaId = tipoPruebaGraduacionId;
                    }
                }
            }
        }

        await context.SaveChangesAsync();
    }
}