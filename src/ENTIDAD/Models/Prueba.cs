using System;
using System.Collections.Generic;

namespace Models;

public partial class Prueba
{
    public int IdPrueba { get; set; }
    public string IdMuestra { get; set; }

    public int? TipoPruebaId { get; set; }

    public string NombrePrueba { get; set; } = null!;

    public virtual Muestra IdMuestraNavigation { get; set; }

    public virtual TipoPrueba? TipoPrueba { get; set; }

    public virtual ICollection<ResultadoPrueba> ResultadoPruebas { get; set; } = new List<ResultadoPrueba>();
}
