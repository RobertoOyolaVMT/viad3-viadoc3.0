using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.Liquidacion
{
    [DataContract]
    public class LiquidacionTotalImpuesto
    {
        [DataMember]
        public string codigo { get; set; }
        [DataMember]
        public string codigoPorcentaje { get; set; }
        [DataMember]
        public string tarifa { get; set; }
        [DataMember]
        public decimal descuentoAdicional { get; set; }
        [DataMember]
        public decimal baseImponible { get; set; }
        [DataMember]
        public decimal valor { get; set; }
    }
}
