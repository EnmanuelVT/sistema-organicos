using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDAD.DTOs.Muestras
{
    public class HistorialDto
    {
        public long IdBitacora { get; set; }

        public string IdMuestra { get; set; } = null!;

        public string IdAnalista { get; set; } = null!;

        public int Estado { get; set; }
        public DateTime FechaCambio { get; set; }

        public string? Observaciones { get; set; }
    }
}
