using System;
using System.Collections.Generic;

namespace Models;

public partial class Auditorium
{
    public long IdAuditoria { get; set; }

    public string IdUsuario { get; set; } = null!;

    public string Accion { get; set; } = null!;

    public DateTime FechaAccion { get; set; }

    public string? Descripcion { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
