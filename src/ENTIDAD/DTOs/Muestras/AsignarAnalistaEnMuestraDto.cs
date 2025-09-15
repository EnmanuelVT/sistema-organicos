using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDAD.DTOs.Muestras
{
    public class AsignarAnalistaEnMuestraDto
    {
        public string MstCodigo { get; set; } = null!;
        public required string IdAnalista { get; set; }
        public required string Observaciones { get; set; }
    }
}
