using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDAD.DTOs.Muestras
{
    public class AsignarEstadoMuestraDto
    {
        public string MstCodigo { get; set; } = null!;
        public int estadoMuestra { get; set; }
        public required string Observaciones { get; set; }
    }
}
