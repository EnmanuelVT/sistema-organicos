using System;
using System.Collections.Generic;

namespace Models;

public partial class TipoMuestra
{
    public byte TpmstId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Muestra> Muestras { get; set; } = new List<Muestra>();

    public virtual ICollection<Prueba> Pruebas { get; set; } = new List<Prueba>();
    public virtual ICollection<ParametroNorma> ParametroNormas { get; set; } = new List<ParametroNorma>();
}
