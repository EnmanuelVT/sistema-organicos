namespace ENTIDAD.DTOs.Parametros;

public class CreateParametroATipoDto
{
    public byte? TpmstId { get; set; }

    public string NombreParametro { get; set; } = null!;

    public decimal? ValorMin { get; set; }

    public decimal? ValorMax { get; set; }

    public string? Unidad { get; set; }
}