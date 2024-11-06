using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.factura
{
    [DataContract]
    public class FacturaDetalle
    {
        [DataMember]
        public string codigoPrincipal { get; set; }
        [DataMember]
        public string codigoAuxiliar { get; set; }
        [DataMember]
        public string descripcion { get; set; }
        [DataMember]
        public int cantidad { get; set; }
        [DataMember]
        public decimal precioUnitario { get; set; }
        [DataMember]
        public decimal descuento { get; set; }
        [DataMember]
        public decimal precioTotalSinImpuesto { get; set; }
        [DataMember]
        public List<FacturaDetalleImpuesto> detalleImpuesto = new List<FacturaDetalleImpuesto>();
        [DataMember]
        public List<FacturaDetalleAdicional> detalleAdicional = new List<FacturaDetalleAdicional>();

        public FacturaDetalle()
        {
            detalleImpuesto = new List<FacturaDetalleImpuesto>();
            detalleAdicional = new List<FacturaDetalleAdicional>();
        }
    }
}
