using System;
using System.Collections.Generic;
using ENTIDAD.Models;

namespace Models;

public partial class HistorialTrazabilidad
{
    public long IdHistorial { get; set; }

    public string IdMuestra { get; set; } = null!;

    public string IdUsuario { get; set; } = null!;

    public byte Estado { get; set; }

    public DateTime FechaCambio { get; set; }

    public string? Observaciones { get; set; }

    public virtual EstadoMuestra EstadoNavigation { get; set; } = null!;

    public virtual Muestra IdMuestraNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
