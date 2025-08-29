namespace ENTIDAD.DTOs.Parametros;

public class CreateParametroAPruebaDto
{
    public int IdPrueba { get; set; }

    public string NombreParametro { get; set; } = null!;

    public decimal? ValorMin { get; set; }

    public decimal? ValorMax { get; set; }

    public string? Unidad { get; set; }
}