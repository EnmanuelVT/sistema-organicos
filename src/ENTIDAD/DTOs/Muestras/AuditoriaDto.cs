using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDAD.DTOs.Muestras
{
    public class AuditoriaDto
    {
        public string idAuditoria { get; set; } = null!;
        public string idUsuario { get; set; }
        public string Accion { get; set; }
        public DateTime fechaAcción { get; set; }
        public string descripcion { get; set; }
    }
}
