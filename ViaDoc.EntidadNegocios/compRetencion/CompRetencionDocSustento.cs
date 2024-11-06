using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.compRetencion
{
   public class CompRetencionDocSustento
    {

        [DataMember]
        public string fechaEmision { get; set; }
        [DataMember]
        public string codSustento { get; set; }
        [DataMember]
        public string codDocSustento { get; set; }
        [DataMember]
        public string fechaRegistroContable { get; set; }
        [DataMember]
        public string codImpuestoDocSustento { get; set; }
        [DataMember]
        public string codigoPorcentaje { get; set; }
        [DataMember]
        public string totalSinImpuesto { get; set; }
        [DataMember]
        public string importeTotal { get; set; }
        [DataMember]
        public string baseImponible { get; set; }
        [DataMember]
        public string tarifa { get; set; }
        [DataMember]
        public string valorImpuesto { get; set; }  


    }
}
