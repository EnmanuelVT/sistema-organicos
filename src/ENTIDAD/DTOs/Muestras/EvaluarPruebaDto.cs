namespace ENTIDAD.DTOs.Muestras;

public class EvaluarPruebaDto
{
    public int IdPrueba { get; set; }             // La prueba a evaluar
    public bool Aprobado { get; set; }            // Aprobada/Rechazada por el evaluador
    public string? Observaciones { get; set; }    // Observaciones del evaluador
}