using System;
using System.Collections.Generic;

namespace Models;

public partial class TipoMuestraTipoPrueba
{
    public byte TpmstId { get; set; }

    public int TipoPruebaId { get; set; }

    public int Orden { get; set; }

    public virtual TipoMuestra Tpmst { get; set; } = null!;

    public virtual TipoPrueba TipoPrueba { get; set; } = null!;
}
