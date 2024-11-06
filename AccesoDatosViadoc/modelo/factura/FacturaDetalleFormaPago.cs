using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatosViadoc.modelo.factura
{
    public class FacturaDetalleFormaPago
    {
        public string ciCompania { get; set; }
        public string txEstablecimiento { get; set; }
        public string txPuntoEmision { get; set; }
        public string txSecuencial { get; set; }
        public string txFormaPago { get; set; }
        public string txPlazo { get; set; }
        public string txUnidadTiempo { get; set; }
        public string qnTotal { get; set; }
    }
}
