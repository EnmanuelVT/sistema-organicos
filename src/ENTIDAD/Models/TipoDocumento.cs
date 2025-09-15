using System;
using System.Collections.Generic;

namespace Models;

public partial class TipoDocumento
{
    public byte IdTipoDoc { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();
}
