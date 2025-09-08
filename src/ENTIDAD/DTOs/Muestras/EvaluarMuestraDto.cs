namespace ENTIDAD.DTOs.Muestras;

public class EvaluarMuestraDto
{
    public string MuestraId { get; set; } = null!;
    public bool Aprobado { get; set; }
    public string? Observaciones { get; set; }
}
