using System;
using System.Collections.Generic;
using ENTIDAD.Models;

namespace Models;

public partial class RolUsuario
{
    public byte IdRol { get; set; }

    public string NombreRol { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
