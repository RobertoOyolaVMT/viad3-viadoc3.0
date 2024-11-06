using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.guiaRemision
{
    [DataContract]
    public class GuiaRemisionDestinatario
    {
        [DataMember]
        public string identificacionDestinatario { get; set; }
        [DataMember]
        public string razonSocialDestinatario { get; set; }
        [DataMember]
        public string direccionDestinatario { get; set; }
        [DataMember]
        public string motivoTraslado { get; set; }
        [DataMember]
        public string documentoAduaneroUnico { get; set; }
        [DataMember]
        public string codigoEstablecimientoDestino { get; set; }
        [DataMember]
        public string ruta { get; set; }
        [DataMember]
        public string tipoDocumentoSustento { get; set; }
        [DataMember]
        public string numeroDocumentoSustento { get; set; }
        [DataMember]
        public string numeroAutorizacionDocumentoSustento { get; set; }
        [DataMember]
        public string fechaEmisionDocumentoSustento { get; set; }

        [DataMember]
        public List<GuiaRemisionDestinatarioDetalle> detalle { get; set; }

        public GuiaRemisionDestinatario()
        {
            detalle = new List<GuiaRemisionDestinatarioDetalle>();
        }
    }
}
