using Models;

namespace ENTIDAD.Models;

public class EstadoDocumento
{
    public int IdEstadoDocumento { get; set; }
    public string Nombre { get; set; }
    public ICollection<Documento> Documentos = new List<Documento>();
}