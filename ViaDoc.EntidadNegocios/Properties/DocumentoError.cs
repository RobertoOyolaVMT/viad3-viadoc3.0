using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios
{
    public class DocumentoError
    {
        public string Fase { get; set; }
        public string CodigoTipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string RucCompania { get; set; }
        public string NombreCompania { get; set; }
        public string ClaveAcceso { get; set; }
        public string NumeroDocumento { get; set; }
        public string FechaEmision { get; set; }
        public string FechaProcesando { get; set; }
        public string ErrorDocumento { get; set; }

        public DocumentoError()
        {
            Fase = "";
            CodigoTipoDocumento = "";
            TipoDocumento = "";
            RucCompania = "";
            NombreCompania = "";
            ClaveAcceso = "";
            NumeroDocumento = "";
            FechaEmision = "";
            FechaProcesando = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("HH:mm:ss");
            ErrorDocumento = "";
        }
    }
}
