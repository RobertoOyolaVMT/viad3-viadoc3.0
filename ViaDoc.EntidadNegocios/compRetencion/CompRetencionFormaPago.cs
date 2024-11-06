using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.compRetencion
{
    public class CompRetencionFormaPago
    {
        [DataMember]
        public string formaPago { get; set; }
        [DataMember]
        public string qnTotal { get; set; } 
    }
}
