using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDAD.DTOs.Documentos
{
    public partial class CreateDocumentoDto
    {

        public string IdMuestra { get; set; } = null!;
        public byte IdTipoDoc { get; set; }

        public int Version { get; set; }

        public string? RutaArchivo { get; set; }

        public byte[]? DocPdf { get; set; }

        public DateTime FechaCreacion { get; set; }
        public int IdEstadoDocumento { get; set; }
    }

}
