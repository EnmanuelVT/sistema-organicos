using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Models;

namespace ENTIDAD.Models;

public partial class Usuario : IdentityUser
{
    // Campos comentandos estan en IdentityUser
    public string? UsCedula { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? RazonSocial { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Contacto { get; set; }

    public virtual ICollection<Auditorium> Auditoria { get; set; } = new List<Auditorium>();

    public virtual ICollection<BitacoraMuestra> BitacoraMuestras { get; set; } = new List<BitacoraMuestra>();

    public virtual ICollection<HistorialTrazabilidad> HistorialTrazabilidads { get; set; } = new List<HistorialTrazabilidad>();

    public virtual ICollection<Muestra> MuestraIdAnalistaNavigations { get; set; } = new List<Muestra>();

    public virtual ICollection<Muestra> MuestraIdUsuarioSolicitanteNavigations { get; set; } = new List<Muestra>();

    public virtual ICollection<ResultadoPrueba> ResultadoPruebas { get; set; } = new List<ResultadoPrueba>();
}
