using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ViaDoc.EntidadNegocios.Liquidacion
{
    [DataContract]
     public class LiquidacionDetalleFormaPago
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
