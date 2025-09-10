using ENTIDAD.DTOs.Documentos;

namespace ENTIDAD.DTOs.Muestras;

public class EvaluarPruebaResponseDto
{
    public MuestraDto Muestra { get; set; } = default!;
    public DocumentoDto? Documento { get; set; }
    public int IdPrueba { get; set; }
}