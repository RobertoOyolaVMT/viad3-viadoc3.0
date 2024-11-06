using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.compRetencion
{
    [DataContract]
    public class CompRetencion
    {
        [DataMember(Order = 1)]
        public int idCompRetencion { get; set; }
        [DataMember(Order = 2)]
        public int compania { get; set; }
        [DataMember(Order = 3)]
        public string establecimiento { get; set; }
        [DataMember(Order = 4)]
        public string puntoEmision { get; set; }
        [DataMember(Order = 5)]
        public string secuencial { get; set; }
        [DataMember(Order = 6)]
        public string fechaEmision { get; set; }
        [DataMember(Order = 7)]
        public string claveAcceso { get; set; }
        [DataMember(Order = 8)]
        public int tipoEmision { get; set; }
        [DataMember]
        public string identificacionSujetoRetenido { get; set; }
        [DataMember]
        public string periodoFiscal { get; set; }
        [DataMember]
        public string razonSocialSujetoRetenido { get; set; }
        [DataMember]
        public string tipoIdentificacionSujetoRetenido { get; set; }
        [DataMember(Order = 20)]
        public int contingenciaDet { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string ambiente { get; set; }
        [DataMember]
        public string codigoNumerico { get; set; }
        [DataMember]
        public string ruc { get; set; }
        [DataMember]
        public List<CompRetencionDetalle> detalleRetencion { get; set; }
        [DataMember]
        public List<CompRetencionInfoAdicional> infoAdicional { get; set; }
        [DataMember]
        public List<CompRetencionDocSustento> docSustento { get; set; }
        [DataMember]
        public List<CompRetencionFormaPago> formasPago { get; set; }

        public CompRetencion()
        {
            detalleRetencion = new List<CompRetencionDetalle>();
            infoAdicional = new List<CompRetencionInfoAdicional>();
            docSustento = new List<CompRetencionDocSustento>();
            formasPago = new List<CompRetencionFormaPago>();
        }
    }
}
