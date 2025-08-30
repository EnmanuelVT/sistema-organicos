namespace ENTIDAD.DTOs.Documentos;

public class CambiarEstadoDocumentoDto
{
    public int IdDocumento { get; set; }
    public int IdEstadoDocumento { get; set; }
    public string Observaciones { get; set; }
}