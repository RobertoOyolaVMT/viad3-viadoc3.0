using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.guiaRemision
{
    [DataContract]
    public class GuiaRemisionDestinatarioDetalleAdicional
    {
        [DataMember]
        public string identificacionDestinatario { get; set; }
        [DataMember]
        public string codigoInterno { get; set; }
        [DataMember]
        public string nombre { get; set; }
        [DataMember]
        public string valor { get; set; }
    }
}
