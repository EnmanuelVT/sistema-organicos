using System;
using System.Collections.Generic;

namespace Models;

public partial class EstadoMuestra
{
    public byte IdEstado { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<HistorialTrazabilidad> HistorialTrazabilidads { get; set; } = new List<HistorialTrazabilidad>();

    public virtual ICollection<Muestra> Muestras { get; set; } = new List<Muestra>();
}
