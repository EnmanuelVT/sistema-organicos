using System;
using System.Collections.Generic;
using ENTIDAD.Models;

namespace Models;

public partial class Muestra
{
    public string MstCodigo { get; set; } = null!;

    public byte TpmstId { get; set; }

    public string? Nombre { get; set; }

    public DateTime FechaRecepcion { get; set; }

    public string Origen { get; set; } = null!;

    public DateTime? FechaSalidaEstimada { get; set; }

    public string? CondicionesAlmacenamiento { get; set; }

    public string? CondicionesTransporte { get; set; }

    public string IdUsuarioSolicitante { get; set; } = null!;

    public byte EstadoActual { get; set; }

    public virtual ICollection<BitacoraMuestra> BitacoraMuestras { get; set; } = new List<BitacoraMuestra>();

    public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();

    public virtual EstadoMuestra EstadoActualNavigation { get; set; } = null!;

    public virtual ICollection<HistorialTrazabilidad> HistorialTrazabilidads { get; set; } = new List<HistorialTrazabilidad>();

    public virtual Usuario IdUsuarioSolicitanteNavigation { get; set; } = null!;

    public virtual ICollection<Notificacion> Notificacions { get; set; } = new List<Notificacion>();
    public virtual ICollection<Prueba> Pruebas { get; set; } = new List<Prueba>();

    public virtual ICollection<ResultadoPrueba> ResultadoPruebas { get; set; } = new List<ResultadoPrueba>();

    public virtual TipoMuestra Tpmst { get; set; } = null!;
}
