namespace ENTIDAD.DTOs.ResultadosPruebas;

public class ValidarResultadoDto
{
    public int IdResultado { get; set; }
    public string IdUsuario { get; set; } = null!;
    public string Accion { get; set;  }
    public string? Observaciones { get; set; }
}