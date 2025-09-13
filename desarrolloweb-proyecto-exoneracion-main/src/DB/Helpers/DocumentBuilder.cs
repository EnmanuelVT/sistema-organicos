using DB.Datos;
using Models;

namespace DB.Helpers;

using Aspose.Cells;
using ENTIDAD.DTOs.Muestras;
using ENTIDAD.DTOs.ResultadosPruebas;
using ENTIDAD.Models;
using Microsoft.EntityFrameworkCore;

public interface IDocumentBuilder
{
    Task<string> CrearCertificadoAsync(
        MasterDbContext db,
        Muestra muestra,
        IEnumerable<ResultadoParaDocumentoDto> resultados,
        int version,
        string outputDir);
    Task<string> CrearCertificadoAsync(
        MasterDbContext db,
        Muestra muestra,
        IEnumerable<ResultadoPrueba> resultados,   // <-- ahora acepta resultados de la Prueba
        int version,
        string outputDir);
}

public class DocumentBuilder : IDocumentBuilder
{
    private static readonly Dictionary<int, string> TemplatePorTipo = new()
    {
        { 1, "Assets/FormularioAguaRegistro.xlsx" },                      // Agua
        { 2, "Assets/Formulario Integrado  Alimentos - Actualizado.xlsx" }, // Alimento
        { 3, "Assets/Formulario para  Bebidas alcohólicas - Actualizado.xlsx" } // Bebida alcohólica
    };

    // Mapas de celdas por “alias” (o por IdParametro si prefieres)
    private static readonly Dictionary<int, Dictionary<string, (string celdaValor, string celdaRango)>> CeldasPorTipo
        = new()
    {
        {
            1, // Agua
            new(StringComparer.OrdinalIgnoreCase)
            {
                // alias -> (celda para el valor obtenido, celda para el rango normativo)
                ["ph"] = ("M55","R55"),
                ["solidos_totales"] = ("M56","R56"),
                ["dureza"] = ("M57","R57"),
                ["recuento_microorganismos"] = ("M63","R63"),
                ["coliformes_totales"] = ("M70","R70"),
            }
        },
        {
            2, // Alimento
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["recuento_microorganismos"] = ("M29","R29"),
                ["coliformes_totales"]      = ("M30","R30"),
                ["e_coli"]                  = ("M31","R31"),
                ["salmonella_spp"]          = ("M32","R32"),
                ["estafilococos_aureus"]    = ("M33","R33"),
                ["hongos"]                  = ("M34","R34"),
                ["levaduras"]               = ("M35","R35"),
                ["esterilidad_comercial"]   = ("M36","R36"),
                ["listeria_monocytogenes"]  = ("M37","R37"),
            }
        },
        {
            3, // Bebida alcohólica
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["grado_alcoholico"] = ("M55","R55")
            }
        }
    };
    
    private static string ResolveTemplatePath(string relativeOrFileName)
    {
        // 1) Si viene una ruta absoluta y existe, úsala
        if (Path.IsPathRooted(relativeOrFileName) && File.Exists(relativeOrFileName))
            return relativeOrFileName;

        // 2) Intentar Assets/ (junto al ejecutable)
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var assetPath = Path.Combine(baseDir, relativeOrFileName);
        if (File.Exists(assetPath)) return assetPath;

        // 3) Fallback a /mnt/data (por si las plantillas viven allí)
        var mntPath = Path.Combine("/mnt/data", Path.GetFileName(relativeOrFileName));
        if (File.Exists(mntPath)) return mntPath;

        // 4) Último intento: Assets/<fileName> en BaseDirectory
        var assetsFolder = Path.Combine(baseDir, "Assets", Path.GetFileName(relativeOrFileName));
        if (File.Exists(assetsFolder)) return assetsFolder;

        // Si nada funcionó, devolvemos tal cual (Aspose fallará y el catch dirá la ruta)
        return relativeOrFileName;
    }

    public async Task<string> CrearCertificadoAsync(
        MasterDbContext db,
        Muestra muestra,
        IEnumerable<ResultadoParaDocumentoDto> resultados,
        int version,
        string outputDir)
    {
        Directory.CreateDirectory(outputDir);

        if (!TemplatePorTipo.TryGetValue(muestra.TpmstId, out var template))
            throw new InvalidOperationException($"No hay template configurado para tipo {muestra.TpmstId}");
        
        template = ResolveTemplatePath(template);

        using var wb = new Workbook(template);
        var ws = wb.Worksheets[0];

        // 1) Encabezado
        ws.Cells["S10"].Value = muestra.MstCodigo;
        ws.Cells["H20"].Value = muestra.TpmstId;

        // 2) Datos del solicitante (si existen)
        //    *Opcional: cargar con un repositorio de Usuario si lo deseas*
        // ws.Cells["H12"].Value = solicitanteNombreOEmail;

        // 3) Traer normas para este tipo
        var normas = await db.ParametroNormas
            .AsNoTracking()
            .Where(p => p.TpmstId == muestra.TpmstId)
            .ToListAsync();

        // 4) Mapa de celdas para el tipo
        if (!CeldasPorTipo.TryGetValue(muestra.TpmstId, out var mapaCeldas))
            throw new InvalidOperationException($"No hay mapa de celdas para el tipo {muestra.TpmstId}");

        // 5) Poner cada resultado en su celda y el rango normativo al lado
        foreach (var r in resultados)
        {
            // Buscar la norma asociada por IdParametro (preciso)…
            var norma = normas.FirstOrDefault(n => n.IdParametro == r.IdParametro);

            // …o caer por alias (útil si no quieres atarte al Id)
            var alias = (r.Alias ?? norma?.NombreParametro ?? "").Trim();
            alias = NormalizarAlias(alias);

            if (string.IsNullOrWhiteSpace(alias)) continue;
            if (!mapaCeldas.TryGetValue(alias, out var celdas)) continue;

            // Unidad: prioriza la del DTO; si no, la de la norma
            var unidad = r.Unidad?.Trim();
            if (string.IsNullOrEmpty(unidad)) unidad = norma?.Unidad;

            // Rango de norma (si existe)
            var rango = norma is null
                ? ""
                : (norma.ValorMin.HasValue || norma.ValorMax.HasValue)
                    ? $"{Trunc(norma.ValorMin)} - {Trunc(norma.ValorMax)} {unidad}"
                    : unidad == null ? "" : $"Unidad: {unidad}";

            ws.Cells[celdas.celdaValor].Value = r.ValorObtenido; // valor medido/validado
            if (!string.IsNullOrEmpty(rango))
                ws.Cells[celdas.celdaRango].Value = rango;
        }

        // 6) Export a PDF
        var pdfPath = Path.Combine(outputDir, $"{muestra.MstCodigo}_v{version}.pdf");
        var pdfOptions = new PdfSaveOptions { OnePagePerSheet = true, AllColumnsInOnePagePerSheet = true };
        wb.Save(pdfPath, pdfOptions);

        return pdfPath;
    }
    
    // Overload NUEVO: acepta Resultados de la Prueba y reutiliza el overload existente
    public async Task<string> CrearCertificadoAsync(
        MasterDbContext db,
        Muestra muestra,
        IEnumerable<ResultadoPrueba> resultados,   // resultados de la PRUEBA
        int version,
        string outputDir)
    {
        // 1) Proyectar ResultadoPrueba -> ResultadoParaDocumentoDto
        var proyectados = await ProyectarResultadosAsync(db, muestra.TpmstId, resultados);

        // 2) Reutilizar el overload existente que ya sabe pintar y exportar PDF
        return await CrearCertificadoAsync(db, muestra, proyectados, version, outputDir);
    }

// Proyección: completa unidad desde norma si falta; alias = nombre del parámetro normalizado
    private static async Task<List<ResultadoParaDocumentoDto>> ProyectarResultadosAsync(
        MasterDbContext db,
        int tpmstId,
        IEnumerable<ResultadoPrueba> resultados)
    {
        var paramIds = resultados.Select(r => r.IdParametro).Distinct().ToList();

        var normas = await db.ParametroNormas
            .AsNoTracking()
            .Where(n => n.TpmstId == tpmstId && paramIds.Contains(n.IdParametro))
            .ToDictionaryAsync(n => n.IdParametro);

        var list = new List<ResultadoParaDocumentoDto>();

        foreach (var r in resultados)
        {
            normas.TryGetValue(r.IdParametro, out var norma);

            // alias: nombre del parámetro (norma) normalizado; si no hay norma, dejamos vacío y caerá en tu lógica
            var alias = norma?.NombreParametro;

            // unidad: prioriza la del resultado; si no tiene, usa la de la norma
            var unidad = string.IsNullOrWhiteSpace(r.Unidad) ? norma?.Unidad : r.Unidad;

            list.Add(new ResultadoParaDocumentoDto() {
                IdParametro = r.IdParametro,
                ValorObtenido = r.ValorObtenido.Value,
                Unidad = unidad,
                Alias = alias
            });
        }

        return list;
    }


    private static string NormalizarAlias(string s)
    {
        s = s.ToLowerInvariant();
        s = s.Replace(" ", "_")
             .Replace(".", "")
             .Replace(",", "")
             .Replace("-", "_")
             .Replace("á","a").Replace("é","e").Replace("í","i").Replace("ó","o").Replace("ú","u")
             .Replace("ü","u").Replace("ñ","n");
        if (s.Contains("ph")) return "ph";
        if (s.Contains("solidos") && s.Contains("totales")) return "solidos_totales";
        if (s.Contains("recuento") && s.Contains("microorgan")) return "recuento_microorganismos";
        if (s.Contains("coliformes") && s.Contains("totales")) return "coliformes_totales";
        if (s.Contains("e.") && s.Contains("coli")) return "e_coli";
        if (s.Contains("salmonella")) return "salmonella_spp";
        if (s.Contains("estafilococos")) return "estafilococos_aureus";
        if (s.Contains("esterilidad") && s.Contains("comercial")) return "esterilidad_comercial";
        if (s.Contains("listeria")) return "listeria_monocytogenes";
        if (s.Contains("grado") && s.Contains("alcohol")) return "grado_alcoholico";
        return s;
    }

    private static string Trunc(decimal? v)
        => v.HasValue ? decimal.Round(v.Value, 6).ToString() : "";
}
