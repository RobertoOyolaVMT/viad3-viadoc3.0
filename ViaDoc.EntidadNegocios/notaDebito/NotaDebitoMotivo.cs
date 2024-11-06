using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.notaDebito
{
    [DataContract]
    public class NotaDebitoMotivo
    {
        [DataMember]
        public string razon { get; set; }
        [DataMember]
        public decimal valor { get; set; }
    }
}
