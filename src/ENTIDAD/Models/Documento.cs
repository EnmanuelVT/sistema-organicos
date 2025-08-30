using System;
using System.Collections.Generic;
using ENTIDAD.Models;

namespace Models;

public partial class Documento
{

    public long IdDocumento { get; set; }
    public string IdMuestra { get; set; } = null!;
    public byte IdTipoDoc { get; set; }
    public int IdEstadoDocumento { get; set; }

    public int Version { get; set; }

    public string? RutaArchivo { get; set; }

    public byte[]? DocPdf { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Muestra IdMuestraNavigation { get; set; } = null!;

    public virtual TipoDocumento IdTipoDocNavigation { get; set; } = null!;
    public virtual EstadoDocumento IdEstadoDocumentoNavigation { get; set; } = null!;
}
