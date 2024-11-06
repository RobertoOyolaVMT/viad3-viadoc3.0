using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.notaDebito
{
    [DataContract]
    public class NotaDebito
    {
        [DataMember (Order =1)]
        public int idNotaDebito { get; set; }
        [DataMember(Order = 2)]
        public int compania { get; set; }
        [DataMember(Order = 3)]
        public int tipoEmision { get; set; }
        [DataMember(Order = 4)]
        public string claveAcceso { get; set; }
        [DataMember(Order = 5)]
        public string tipoDocumento { get; set; }
        [DataMember(Order = 6)]
        public string establecimiento { get; set; }
        [DataMember(Order = 7)]
        public string puntoEmision { get; set; }
        [DataMember(Order = 8)]
        public string secuencial { get; set; }
        [DataMember(Order = 9)]
        public string fechaEmision { get; set; }
        [DataMember(Order = 10)]
        public string tipoIdentificacionComprador { get; set; }
        [DataMember(Order = 11)]
        public string razonSocialComprador { get; set; }
        [DataMember(Order = 12)]
        public string identificacionComprador { get; set; }
        [DataMember(Order = 13)]
        public string rise { get; set; }
        [DataMember(Order = 14)]
        public string tipoDocumentoModificado { get; set; }
        [DataMember(Order = 15)]
        public string numeroDocumentoModificado { get; set; }
        [DataMember(Order = 16)]
        public string fechaEmisionDocumentoModificado { get; set; }
        [DataMember(Order = 17)]
        public decimal totalSinImpuestos { get; set; }
        [DataMember(Order = 18)]
        public decimal valorTotal { get; set; }
        [DataMember(Order = 19)]
        public int contingenciaDet { get; set; }
        [DataMember(Order = 20)]
        public string email { get; set; }
        [DataMember(Order = 21)]
        public string estado { get; set; }
        [DataMember(Order = 22)]
        public string ambiente { get; set; }
        [DataMember(Order = 23)]
        public string codigoNumerico { get; set; }
        [DataMember(Order = 24)]
        public string ruc { get; set; }

        [DataMember(Order = 25)]
        public List<NotaDebitoImpuesto> impuesto { get; set; }
        [DataMember(Order = 26)]
        public List<NotaDebitoInfoAdicional> infoAdicional { get; set; }
        [DataMember(Order = 27)]
        public List<NotaDebitoMotivo> motivo { get; set; }

        public NotaDebito()
        {
            impuesto = new List<NotaDebitoImpuesto>();
            motivo = new List<NotaDebitoMotivo>();
            infoAdicional = new List<NotaDebitoInfoAdicional>();
        }
    }

}
