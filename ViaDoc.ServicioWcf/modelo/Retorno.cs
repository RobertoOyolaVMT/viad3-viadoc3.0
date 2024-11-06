using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ViaDoc.ServicioWcf.modelo
{
    [DataContract]
    public class Retorno
    {
        [DataMember]
        public int codigoRetorno { get; set; }
        [DataMember]
        public string mensajeRetorno { get; set; }

       
    }

    [DataContract]
    public class DocumentosProcesados
    {
        [DataMember]
        public int ciDocumento { get; set; }
        [DataMember]
        public string tipoDocumento { get; set; }
        [DataMember]
        public string claveAcceso { get; set; }
        [DataMember]
        public int compania { get; set; }
        [DataMember]
        public string puntoEmision { get; set; }
        [DataMember]
        public string establecimiento { get; set; }
        [DataMember]
        public string secuencial { get; set; }
        [DataMember]
        public string codigoNumerico { get; set; }
        [DataMember]
        public int codigoRetorno { get; set; }
        [DataMember]
        public string descripcionRetorno { get; set; }
        [DataMember]
        public string tablaMurano { get; set; }
    }
}