namespace ENTIDAD.DTOs.ResultadosPruebas;

public class ResultadoPruebaDto
{
    public long IdResultado { get; set; }

    public int IdPrueba { get; set; }

    public string IdMuestra { get; set; } = null!;

    public decimal? ValorObtenido { get; set; }

    public string? Unidad { get; set; }

    public bool? CumpleNorma { get; set; }

    public DateTime FechaRegistro { get; set; }

    public string? ValidadoPor { get; set; }
    public string? EstadoValidacion { get; set; }
}