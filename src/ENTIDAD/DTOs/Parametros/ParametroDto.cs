namespace ENTIDAD.DTOs.Parametros;

public class ParametroDto
{
    public int IdParametro { get; set; }

    public byte? TpmstId { get; set; }

    public int? TipoPruebaId { get; set; }

    public string NombreParametro { get; set; } = null!;

    public decimal? ValorMin { get; set; }

    public decimal? ValorMax { get; set; }

    public string? Unidad { get; set; }
}