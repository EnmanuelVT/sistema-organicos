using System;
using System.Collections.Generic;

namespace Models;

public partial class Notificacion
{
    public long IdNotificacion { get; set; }

    public string IdMuestra { get; set; } = null!;

    public string TipoAlerta { get; set; } = null!;

    public string Destinatario { get; set; } = null!;

    public bool Enviado { get; set; }

    public DateTime? FechaEnvio { get; set; }

    public string? Detalle { get; set; }

    public virtual Muestra IdMuestraNavigation { get; set; } = null!;
}
