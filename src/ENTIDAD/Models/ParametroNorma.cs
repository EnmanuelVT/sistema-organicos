using System;
using System.Collections.Generic;

namespace Models;

public partial class ParametroNorma
{
    public int IdParametro { get; set; }

    public byte? TpmstId { get; set; }

    public string NombreParametro { get; set; } = null!;

    public decimal? ValorMin { get; set; }

    public decimal? ValorMax { get; set; }

    public string? Unidad { get; set; }

    public virtual TipoMuestra? TipoMuestraAsociadaNavigation { get; set; } = null!;
    public ICollection<ResultadoPrueba> ResultadoPruebas { get; set; } = new List<ResultadoPrueba>();
}
