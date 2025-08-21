namespace DB.Datos.DocumentoD.Models;

public class CreateDocumentoDto
{
    public string IdMuestra { get; set; } = null!;

    public byte IdTipoDoc { get; set; }

    public int Version { get; set; }

    public string? RutaArchivo { get; set; }

    public DateTime FechaCreacion { get; set; }
}