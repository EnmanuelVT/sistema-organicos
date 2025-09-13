using System;
using System.Collections.Generic;
using ENTIDAD.Models;

namespace Models;

public partial class BitacoraMuestra
{
    public long IdBitacora { get; set; }

    public string IdMuestra { get; set; } = null!;

    public string IdAnalista { get; set; } = null!;

    public DateTime FechaAsignacion { get; set; }

    public string? Observaciones { get; set; }

    public virtual Usuario IdAnalistaNavigation { get; set; } = null!;

    public virtual Muestra IdMuestraNavigation { get; set; } = null!;
}
