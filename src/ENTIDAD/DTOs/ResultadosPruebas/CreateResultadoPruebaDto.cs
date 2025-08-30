namespace ENTIDAD.DTOs.ResultadosPruebas;

public class CreateResultadoPruebaDto
{
    public string IdMuestra { get; set; } = null!;
    public int IdPrueba { get; set; }
    public int IdParametro { get; set;}
    public decimal? ValorObtenido { get; set; }
    public string? Unidad { get; set; }
    public string? ValidadoPor { get; set; }
}