using System;
using System.Collections.Generic;

namespace Models;

public partial class Prueba
{
    public int IdPrueba { get; set; }

    public string NombrePrueba { get; set; } = null!;

    public byte TipoMuestraAsociada { get; set; }

    public string? NormaReferencia { get; set; }

    public virtual ICollection<ParametroNorma> ParametroNormas { get; set; } = new List<ParametroNorma>();

    public virtual ICollection<ResultadoPrueba> ResultadoPruebas { get; set; } = new List<ResultadoPrueba>();

    public virtual TipoMuestra TipoMuestraAsociadaNavigation { get; set; } = null!;
}
