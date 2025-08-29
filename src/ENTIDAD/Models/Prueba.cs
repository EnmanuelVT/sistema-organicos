using System;
using System.Collections.Generic;

namespace Models;

public partial class Prueba
{
    public int IdPrueba { get; set; }
    public string IdMuestra { get; set; }
    public int IdParametroNorma { get; set; }

    public string NombrePrueba { get; set; } = null!;

    public byte TipoMuestraAsociada { get; set; }

    public virtual Muestra IdMuestraNavigation { get; set; }

    public ParametroNorma ParametroNorma { get; set; }

    public virtual ICollection<ResultadoPrueba> ResultadoPruebas { get; set; } = new List<ResultadoPrueba>();

    public virtual TipoMuestra TipoMuestraAsociadaNavigation { get; set; } = null!;
}
