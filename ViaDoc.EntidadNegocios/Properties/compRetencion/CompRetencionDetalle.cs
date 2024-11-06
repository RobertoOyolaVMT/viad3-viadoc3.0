using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.compRetencion
{
    [DataContract]
    public class CompRetencionDetalle
    {
        [DataMember]
        public int impuesto { get; set; }
        [DataMember]
        public string codRetencion { get; set; }
        [DataMember]
        public decimal baseImponible { get; set; }
        [DataMember]
        public decimal porcentajeRetener { get; set; }
        [DataMember]
        public decimal valorRetenido { get; set; }
        [DataMember]
        public string codDocSustento { get; set; }
        [DataMember]
        public string numDocSustento { get; set; }
        [DataMember]
        public string fechaEmisionDocSustento { get; set; }

    }                    
}
