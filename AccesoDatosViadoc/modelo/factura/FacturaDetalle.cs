using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatosViadoc.modelo.factura
{
    public class FacturaDetalle
    {
        public string ciCompania { get; set; }
        public string txEstablecimiento { get; set; }
        public string txPuntoEmision { get; set; }
        public string txSecuencial { get; set; }
        public string txCodigoPrincipal { get; set; }
        public string txCodigoAuxiliar { get; set; }
        public string txDescripcion { get; set; }
        public string qnCantidad { get; set; }
        public decimal qnPrecioUnitario { get; set; }
        public string qnDescuento { get; set; }
        public string qnPrecioTotalSinImpuesto { get; set; }

        public List<FacturaDetalleAdicional> detalleAdicional = new List<FacturaDetalleAdicional>();

        public List<FacturaDetalleImpuesto> detalleimpuesto = new List<FacturaDetalleImpuesto>();
    }
}
