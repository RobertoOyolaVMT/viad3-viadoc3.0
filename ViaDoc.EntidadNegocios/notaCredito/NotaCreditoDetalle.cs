using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.notaCredito
{
    [DataContract]
    public class NotaCreditoDetalle
    {
        [DataMember]
        public string codigoInterno { get; set; }
        [DataMember]
        public string codigoAdicional { get; set; }
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
        public List<NotaCreditoDetalleAdicional> detalleAdicional { get; set; }
        [DataMember]
        public List<NotaCreditoDetalleImpuesto> detalleImpuesto { get; set; }

        public NotaCreditoDetalle()
        {
            detalleAdicional = new List<NotaCreditoDetalleAdicional>();
            detalleImpuesto = new List<NotaCreditoDetalleImpuesto>();
        }
    }
}
