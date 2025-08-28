using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDAD.DTOs.Muestras
{
    public class MuestraDto
    {
        public string MstCodigo { get; set; } = null!;

        public byte TpmstId { get; set; }

        public string? Nombre { get; set; }

        public string Origen { get; set; } = null!;

        public string? CondicionesAlmacenamiento { get; set; }

        public string? CondicionesTransporte { get; set; }

        public byte EstadoActual { get; set; }
        public string? IdAnalista { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public DateTime? FechaSalidaEstimada { get; set; }
    }
}
