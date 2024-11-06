using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class ReembolsoGasto
    {
        public string establecimiento { get; set; }
        public string puntoemision { get; set; }
        public string secuencial { get; set; }

        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string FechaEmision { get; set; }
        public string IdentificacionProveedor { get; set; }
        public string Autorizacion { get; set; }
        public string Detalle { get; set; }
        public string SubTotalNoIva { get; set; }
        public string SubTotalIvaCero { get; set; }
        public string SubTotalIva { get; set; }
        public string SubTotalIva5 { get; set; }
        public string SubTotalExcentoIva { get; set; }
        public string ImpuestoIva { get; set; }
        public string ImpuestoIva5 { get; set; }
        public string ImpuestoIce { get; set; }
        public string ImpuestoIRBPNR { get; set; }
        public string ValBase { get; set; }
        public string ValTotal { get; set; }
    }
}
