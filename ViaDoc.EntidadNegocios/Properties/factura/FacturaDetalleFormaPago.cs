using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.factura
{
    [DataContract]
    public class FacturaDetalleFormaPago
    {
        [DataMember]
        public string formaPago { get; set; }
        [DataMember]
        public string plazo { get; set; }
        [DataMember]
        public string unidadTiempo { get; set; }
        [DataMember]
        public decimal total { get; set; }
    }
}
 