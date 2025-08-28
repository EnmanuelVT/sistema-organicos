using ENTIDAD.Models;
using Models;

namespace ENTIDAD.DTOs.Muestras;

public class CreateMuestraDto
{
    public string MstCodigo { get; set; } = null!;

    public byte TpmstId { get; set; }

    public string? Nombre { get; set; }

    public string Origen { get; set; } = null!;

    public string? CondicionesAlmacenamiento { get; set; }

    public string? CondicionesTransporte { get; set; }

    public byte EstadoActual { get; set; }
}
