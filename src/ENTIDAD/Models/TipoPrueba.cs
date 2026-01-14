using System;
using System.Collections.Generic;

namespace Models;

public partial class TipoPrueba
{
    public int IdTipoPrueba { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Prueba> Pruebas { get; set; } = new List<Prueba>();

    public virtual ICollection<ParametroNorma> ParametroNormas { get; set; } = new List<ParametroNorma>();

    public virtual ICollection<TipoMuestraTipoPrueba> TipoMuestraTipoPruebas { get; set; } = new List<TipoMuestraTipoPrueba>();
}
