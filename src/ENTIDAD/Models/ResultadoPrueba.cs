using System;
using System.Collections.Generic;
using ENTIDAD.Models;

namespace Models;

public partial class ResultadoPrueba
{
    public long IdResultado { get; set; }

    public int IdPrueba { get; set; }

    public string IdMuestra { get; set; } = null!;

    public decimal? ValorObtenido { get; set; }

    public string? Unidad { get; set; }

    public bool? CumpleNorma { get; set; }

    public DateTime FechaRegistro { get; set; }

    public string? ValidadoPor { get; set; }
    public string? EstadoValidacion { get; set; }

    public virtual Muestra IdMuestraNavigation { get; set; } = null!;

    public virtual Prueba IdPruebaNavigation { get; set; } = null!;

    public virtual Usuario? ValidadoPorNavigation { get; set; }
}
