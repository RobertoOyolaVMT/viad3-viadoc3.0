using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.guiaRemision
{
    [DataContract]
    public class GuiaRemisionDestinatarioDetalle
    {
        [DataMember]
        public string identificacionDestinatario { get; set; }
        [DataMember]
        public string codigoInterno { get; set; }
        [DataMember]
        public string codigoAdicional { get; set; }
        [DataMember]
        public string descripcion { get; set; }
        [DataMember]
        public decimal cantidad { get; set; }
        [DataMember]
        public List<GuiaRemisionDestinatarioDetalleAdicional> adicional { get; set; }

        public GuiaRemisionDestinatarioDetalle()
        {
            adicional = new List<GuiaRemisionDestinatarioDetalleAdicional>();
        }
    }
}
