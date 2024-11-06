using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.notaDebito
{
    [DataContract]
    public class NotaDebitoInfoAdicional
    {
        [DataMember]
        public string nombre { get; set; }
        [DataMember]
        public string valor { get; set; }
    }
}
